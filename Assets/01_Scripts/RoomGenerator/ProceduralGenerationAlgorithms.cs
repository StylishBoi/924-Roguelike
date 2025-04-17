using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;

public class ProceduralGenerationAlgorithms
{
    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPosition, int walkLength)
    {
        //Creates a local 'list' of the path coordinates
        HashSet<Vector2Int> path = new HashSet<Vector2Int>();

        //Adds the origin point of the path in the 'list'
        path.Add(startPosition);
        var previousPosition = startPosition;

        //Makes the path randomly walk until the given amount
        for (int i = 0; i < walkLength; i++)
        {
            //Finds a new random path point via the last added point and adds it to the 'list'
            var newPosition = previousPosition + Direction2D.GetRandomCardinalDirection();
            path.Add(newPosition);
            previousPosition = newPosition;
        }
        return path;
    }

    public static List<Vector2Int> RandomWalkCorridor(Vector2Int startPosition, int corridorLength)
    {
        //Creates a local list to remember the position of the corridor
        List<Vector2Int> corridor = new List<Vector2Int>();
        
        //Intakes a random direction for the corridor
        var direction = Direction2D.GetRandomCardinalDirection();
        
        //Saves the starting point in the list
        var currentPosition = startPosition;
        corridor.Add(currentPosition);
        
        //Makes the corridor walk until the given amount
        for (int i = 0; i < corridorLength; i++)
        {
            //Saves all the positions in the direction given
            currentPosition += direction;
            corridor.Add(currentPosition);
        }
        return corridor;
    }
    
    public static List<BoundsInt> BinarySpacePartitioning(BoundsInt spaceToSplit, int minWidth, int minHeight)
    {
        //Keeps a local queue of all the rooms that needs to be divided
        Queue<BoundsInt> roomsQueue = new Queue<BoundsInt>();
        
        //Keeps a local list of all the rooms that have been made
        List<BoundsInt> roomsList = new List<BoundsInt>();
        
        //Enqueue the original parameter to start dividing from that point
        roomsQueue.Enqueue(spaceToSplit);
        
        //Only stops dividing when all the rooms can't be divided any further
        while (roomsQueue.Count > 0)
        {
            //Each iteration, a room is locally stored while being removed from the queue
            var room = roomsQueue.Dequeue();
            
            //Divides the room if it is still bigger than the width and height requirements
            if (room.size.x >= minWidth && room.size.y >= minHeight)
            {
                //Adds randomness to avoid the rooms always splitting in the same manner
                if (Random.value < 0.5f)
                {
                    //Divides the room horizontally if the room height is two times bigger than the minimum requirement
                    if (room.size.y >= minHeight * 2)
                    {
                        SplitHorizontally(minHeight, roomsQueue, room);
                    }
                    //Divides the room vertically if the room width is two times bigger than the minimum requirement
                    else if (room.size.x >= minWidth * 2)
                    {
                        SplitVertically(minWidth, roomsQueue, room);
                    }
                    //Stores the room in the list as it can't be divided any further
                    else if (room.size.x >= minWidth && room.size.y >= minHeight)
                    {
                        roomsList.Add(room);
                    }
                }
                else
                {
                    //Divides the room vertically if the room height is two times bigger than the minimum requirement
                    if (room.size.x >= minWidth * 2)
                    {
                        SplitVertically(minWidth, roomsQueue, room);
                    }
                    //Divides the room horizontally if the room width is two times bigger than the minimum requirement
                    else if (room.size.y >= minHeight * 2)
                    {
                        SplitHorizontally(minHeight, roomsQueue, room);
                    }
                    //Stores the room in the list as it can't be divided any further
                    else if (room.size.x >= minWidth && room.size.y >= minHeight)
                    {
                        roomsList.Add(room);
                    }
                }
            }
        }
        //Return a list of all the rooms that have been created
        return roomsList;
    }

    private static void SplitVertically(int minWidth, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        //Choose the point where the room will be split at
        var xSplit = Random.Range(1, room.size.x);
        
        //Divide vertically the room in two based on the split
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(xSplit, room.size.y, room.size.z));
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x + xSplit, room.min.y, room.min.z),
            new Vector3Int(room.size.x-xSplit, room.size.y, room.size.z));
        
        //Adds the split rooms into the room queue to be split again
        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }

    private static void SplitHorizontally(int minHeight, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        //Choose the point where the room will be split at
        var ySplit = Random.Range(1, room.size.y);
        
        //Divide horizontally the room in two based on the split
        BoundsInt room1= new BoundsInt(room.min, new Vector3Int(room.size.x, ySplit, room.size.z));
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x, room.min.y + ySplit, room.min.z),
            new Vector3Int(room.size.x, room.size.y-ySplit, room.size.z));
        
        //Adds the split rooms into the room queue to be split again
        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }
}

public static class Direction2D
{
    //List of directions that will be checked so a wall can be placed
        public static List<Vector2Int> cardinalDirectionList = new List<Vector2Int>
        {
            new Vector2Int(0, 1),
            new Vector2Int(1, 1),
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(-1, 1),
            new Vector2Int(0, -1),
            new Vector2Int(-1, -1),
            new Vector2Int(1, -1),
            new Vector2Int(0, 2),
            new Vector2Int(2, 2),
            new Vector2Int(2, 0),
            new Vector2Int(-2, 0),
            new Vector2Int(-2, 2),
            new Vector2Int(0, -2),
            new Vector2Int(-2, -2),
            new Vector2Int(2, -2)
        };
        
    //List of directions that will be checked so a corridor coordinate can be placed
        public static List<Vector2Int> corridorDirectionList = new List<Vector2Int>
        {
            new Vector2Int(-1, 0),
            new Vector2Int(-1, -1),
            new Vector2Int(0, -1)
        };

    //Returns a random direction position
        public static Vector2Int GetRandomCardinalDirection()
        {
            return cardinalDirectionList[Random.Range(0, cardinalDirectionList.Count)];
        }
}
