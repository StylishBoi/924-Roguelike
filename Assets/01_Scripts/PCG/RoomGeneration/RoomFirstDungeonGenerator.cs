using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class RoomFirstDungeonGenerator : AbstractDungeonGenerator
{
    [Header("Room Generation Settings")]
    [SerializeField] private int minRoomWidth = 4;
    [SerializeField] private int minRoomHeight = 4;
    [SerializeField] private int numberOfRooms = 5;
    [SerializeField] private GameObject roomHierarchySpawn;
    [SerializeField] [Range(0,5)] private int distanceBetweenRooms = 1;
    [SerializeField] private bool generateCorridors = true;
    [SerializeField] private List<GameObject> roomPrefabs = new List<GameObject>();
    //Moved up here for Gizmos drawing
    private List<BoundsInt> roomList;
    AstarPath astar;

    [Header("Dungeon Generation Settings")]
    [SerializeField] [Range(0,100)] private int dungeonWidth = 20;
    [SerializeField] [Range(0,100)] private int dungeonHeight = 20;

    [FormerlySerializedAs("hierarchySpawn")]
    [Header("Player Generation Settings")]
    [SerializeField] private GameObject playerHierarchySpawn;
    [SerializeField] private GameObject playerPrefab;
    private bool _playerSpawned;
    
    public static RoomFirstDungeonGenerator Instance;

    private void Awake()
    {
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        AudioManager.Instance.PlayBGM(AudioManager.Instance.gameOST);
        RunProceduralGeneration();
    }

    //Override the abstract 'RunProcesduralGeneration' in its root-node class so it generates via its own method
    public override void RunProceduralGeneration()
    {
        //Clears the tilemap of the previous dungeon
        tileMapVisualizer.Clear();
        
        //Destroys the previous player prefab
        if (playerHierarchySpawn.transform.childCount != 0 && !_playerSpawned)
        {
            DestroyImmediate(playerHierarchySpawn.transform.GetChild(0).gameObject);
        }
        
        //Destroys the previous rooms prefab
        if (roomHierarchySpawn.transform.childCount != 0)
        {
            //Create a local variable otherwise the loops gets updated in real time with the number of children
            int numberOfRooms = roomHierarchySpawn.transform.childCount;
            for (int i = 0; i < numberOfRooms; i++)
            {
                if (Application.isPlaying)
                {
                    Debug.Log("Destroy in play mode");
                    foreach (Transform child in roomHierarchySpawn.transform)
                    {
                        Destroy(child.gameObject);
                    }
                    Destroy(roomHierarchySpawn.transform.GetChild(0).gameObject);
                }
                else
                {
                    Debug.Log("Destroy in editor mode");
                    DestroyImmediate(roomHierarchySpawn.transform.GetChild(0).gameObject);
                }
            }
            /*foreach (Transform child in roomHierarchySpawn.transform)
            {
                if (Application.isPlaying)
                {
                    Debug.Log("Destroy in play mode");
                    Destroy(child.gameObject);
                }
                else
                {
                    Debug.Log("Destroy in editor mode");
                    DestroyImmediate(child.gameObject);
                }
            }*/
        }
        
        //Starts the PCG for rooms
        CreateRooms();
    }

    private void CreateRooms()
    {
        //Creates a list of all the rooms in the dungeon
        roomList = new List<BoundsInt>();
        
        //Keeps generating and dividing rooms until one generation has more rooms than asked
        //Additional failsafe to avoid infinite loops
        int maxLoops=0;
        do
        {
            roomList=ProceduralGenerationAlgorithms.BinarySpacePartitioning(new BoundsInt((Vector3Int)startPosition,
                new Vector3Int(dungeonWidth, dungeonHeight, 0)), minRoomWidth, minRoomHeight);
            
            //Failsafe that stop programs if too many loops occur
            maxLoops++;
            if (maxLoops > 100000)
            {
                Debug.Log("Max number of loops have been reached for generations, please try different input parameters.");
                return;
            }
        } while (roomList.Count < numberOfRooms);
        
        Debug.Log("Total number of rooms: " + roomList.Count);
        
        //Creates a 'list' of the floors coordinates in the rooms
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        floor = CreateSimpleRooms(roomList);

        if (generateCorridors)
        {
            
            //Creates a list of the room centers
            List<Vector2Int> roomCenters = new List<Vector2Int>();
            //Goes through every room in the list and find the center
            foreach (var room in roomList)
            {
                roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
            }
            
            //Creates a 'list' of the coordinates of the cooridors and adds it to the floor 'list'
            HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);
            floor.UnionWith(corridors);
        }

        //Creates the walls and floor based on the floor list
        tileMapVisualizer.PaintFloorTiles(floor);
        WallGenerator.CreateWalls(floor, tileMapVisualizer);
        
        //Updates the graph for the AIs
        AstarPath.active.UpdateGraphs(new Bounds(new Vector3(dungeonWidth/2, dungeonHeight/2, 0), 
            new Vector3(dungeonWidth, dungeonHeight, 0)));
        
        //Initialize the player in the middle of a room after everything is set
        if (playerHierarchySpawn.transform.childCount == 0)
        {
            Debug.Log("Creating player");
            Instantiate(playerPrefab, roomList[0].center, Quaternion.identity, playerHierarchySpawn.transform);
            _playerSpawned = true;
        }
        //Moves the player if he already exists
        else
        {
            Debug.Log("Moving player");
            playerHierarchySpawn.transform.GetChild(0).position = roomList[0].center;
            _playerSpawned = true;
        }
        
        //Creates gameobjects for each rooms
        CreateRoomObjects();
    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCentersToBeFound)
    {
        //Creates a 'list' of the corridors coordinates
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        List<Vector2Int> allRoomCenters = roomCentersToBeFound;
        
        //Takes a random room center point, stores it in a local variable while also removing it from the list
        var currentRoomCenter = roomCentersToBeFound[Random.Range(0, roomCentersToBeFound.Count)];
        roomCentersToBeFound.Remove(currentRoomCenter);

        //Goes through every room center points
        while (roomCentersToBeFound.Count > 0)
        {
            //Finds the closest room based on the center then stores it in a local variable before removing it from the list 
            Vector2Int closest = FindClosestPointTo(currentRoomCenter, allRoomCenters);
            roomCentersToBeFound.Remove(closest);
            
            //Creates a new corridor with the current room center and the one closest to it
            HashSet<Vector2Int> newCorridor=CreateCorridor(currentRoomCenter, closest);
            
            //Makes the closest point the new room center
            currentRoomCenter = closest;
            
            //Adds the corridor coordinates to the 'list'
            corridors.UnionWith(newCorridor);
        }
        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        //Creates a 'list' of the corridors coordinates
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        
        //Makes the current position of the corridor the room center and adds it to the corridors 'list'
        var position=currentRoomCenter;
        corridors.Add(position);

        //Keeps updating the path until the position reaches the same height as the destination
        while (position.y != destination.y)
        {
            //Either moves the path up or down based on the destination direction
            if (destination.y > position.y)
            {
                position+=Vector2Int.up;
            }
            else if (destination.y < position.y)
            {
                position+=Vector2Int.down;
            }
            //Adds the current position of the corridor path to the 'list' of the corridor coordinates
            corridors.Add(position);
            
            //Doubles the size of the corridor by adding tiles in a pre-determined direction
            foreach (var direction in Direction2D.corridorDirectionList)
            {
                var neighbourPosition = position + direction;
                corridors.Add(neighbourPosition);
            }
        }

        //Keeps updating the path until the position reaches the same X axis as the destination
        while (position.x != destination.x)
        {
            //Either moves the path left or right based on the destination direction
            if (destination.x > position.x)
            {
                position+=Vector2Int.right;
            }
            else if (destination.x < position.x)
            {
                position+=Vector2Int.left;
            }
            //Adds the current position of the corridor path to the 'list' of the corridor coordinates
            corridors.Add(position);

            //Doubles the size of the corridor by adding tiles in a pre-determined direction
            foreach (var direction in Direction2D.corridorDirectionList)
            {
                var neighbourPosition = position + direction;
                corridors.Add(neighbourPosition);
            }
        }
        return corridors;
    }

    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        //Creates a local value for the closest center point from the room center
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;
        
        //Goes through every room centers
        foreach (var position in roomCenters)
        {
            //Calculates the distance between the current room center and its position
            float currentDistance = Vector2.Distance(position, currentRoomCenter);
            //If the current distance is shorter than the previous one, it will update the shortest distance and the closest point
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = position;
            }
        }
        //Returns the poitn of the closest room center
        return closest;
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomList)
    {
        //Creates a 'list' of the floor positions
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        
        //Goes through every room and intakes the floor positions based on the room size
        foreach (var room in roomList)
        {
            for (int col = distanceBetweenRooms; col < room.size.x - distanceBetweenRooms; col++)
            {
                for (int row = distanceBetweenRooms; row < room.size.y - distanceBetweenRooms; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(position);
                }
            }
        }
        //Returns the 'list' of the floor positions
        return floor;
    }

    private void CreateRoomObjects()
    {
        //Creates local variable for the new rooms and the furthest room from the spawn room
        GameObject newRoom;
        int furthestRoom = 0;
        float furthestDistance=0;

        //Goes through every room and check their distances to decide the boss room
        for (int x = 0; x < roomList.Count; x++)
        {
            float currentDistance;
            
            currentDistance = Vector3.Distance(roomList[0].center, roomList[x].center);
            
            if (furthestDistance < currentDistance)
            {
                furthestDistance = currentDistance;
                furthestRoom = x;
            }
        }
        
        //Goes through every room and set them up
        for (int i = 0; i < roomList.Count; i++)
        {
            if (i == 0)
            {
                //Creates the spawn room
                newRoom = Instantiate(roomPrefabs[0], roomList[i].center, Quaternion.identity, roomHierarchySpawn.transform);
                newRoom.name=("(Spawn) Room Number : " + (i+1).ToString());
            }
            else if (i == furthestRoom)
            {
                //Creates the boss room
                newRoom = Instantiate(roomPrefabs[3], roomList[i].center, Quaternion.identity, roomHierarchySpawn.transform);
                newRoom.name=("(Boss) Room Number : " + (i+1).ToString());
            }
            else
            {
                //Decides randomly if it's an enemy room or an item room
                if (Random.value < 0.75f)
                {
                    //Creates the enemy rooms
                    newRoom = Instantiate(roomPrefabs[1], roomList[i].center, Quaternion.identity, roomHierarchySpawn.transform);
                    newRoom.name=("(Enemy) Room Number : " + (i+1).ToString());
                }
                else
                {
                    //Creates an item rooms
                    newRoom = Instantiate(roomPrefabs[2], roomList[i].center, Quaternion.identity, roomHierarchySpawn.transform);
                    newRoom.name=("(Item) Room Number : " + (i+1).ToString());
                }
            }
            
            //Insert the room bounds to the item
            if (newRoom.gameObject.TryGetComponent(out RoomControl outRoomControl))
            {
                outRoomControl.SetBounds(roomList[i], distanceBetweenRooms);
            }
        }
    }
    
    private void OnDrawGizmos()
    {

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(new Vector3(dungeonWidth/2, dungeonHeight/2, 0), new Vector3(dungeonWidth, dungeonHeight, 0));
        
        
        //Gizmos.DrawWireCube(room.center, new Vector3(room.size.x-(distanceBetweenRooms+1),room.size.y-(distanceBetweenRooms+1),0f));
    }
}
