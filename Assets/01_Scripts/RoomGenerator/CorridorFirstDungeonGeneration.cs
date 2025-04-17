using System.Collections.Generic;
using UnityEngine;

public class CorridorFirstDungeonGenerator : SimpleRandomWalkDungeonGenerator
{
    [SerializeField] private int corridorLength = 14;
    [SerializeField] private int corridorCount = 5;

    [SerializeField] [Range(0.1f, 1)] private float roomPercent = 0.8f;

    [SerializeField] public SimpleRandomWalkSO roomGenerationParameters;
    
    //Override the abstract 'RunProcesduralGeneration' in its root-node class so it generates via its own method
    protected override void RunProceduralGeneration()
    {
        //Starts the PCG for corridors
        CorridorFirstGeneration();
    }

    private void CorridorFirstGeneration()
    {
        //Creates a 'list' of the floor positions
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();

        //Generates a corridor
        CreateCorridor(floorPositions);
        
        //Adds floor and walls based on the floor coordinates
        tileMapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tileMapVisualizer);
    }

    private void CreateCorridor(HashSet<Vector2Int> floorPositions)
    {
        //Takes the starting point as the current position to start off the generation
        var currentPosition = startPosition;

        //Generates corridor until the given number of corridors
        for (int i = 0; i < corridorCount; i++)
        {
            //Generate a corridor in a random direction with a given length
            var corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currentPosition, corridorLength);
            
            //Stores the coridor in floor positions while updating the current position as the end of the current corridor
            currentPosition=corridor[corridor.Count-1];
            floorPositions.UnionWith(corridor);
        }
    }
}
