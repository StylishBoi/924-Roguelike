using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimpleRandomWalkDungeonGenerator : AbstractDungeonGenerator
{

    [SerializeField] private SimpleRandomWalkSO randomWalkParameters;

    //Override the abstract 'RunProcesduralGeneration' in its root-node class so it generates via its own method
    protected override void RunProceduralGeneration()
    {
        //Creates a 'list' of floor coordinates with the help of a function
        HashSet<Vector2Int> floorPositions = RunRandomWalk();
        
        //Clears up the tile map and paint new floors and walls via the floor coordinates
        tileMapVisualizer.Clear();
        tileMapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tileMapVisualizer);
    }

    protected HashSet<Vector2Int> RunRandomWalk()
    {
        //Takes the starting point as the current position
        var currentPosition = startPosition;
        
        //Creates a local 'list' to store the floor coordinates
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        
        //Generates until the given number of walk iterations
        for (int i = 0; i < randomWalkParameters.iterations; i++)
        {
            //Creates a local 'list' to store the path coordinates and adds it to the 'list' of floor coordinates
            var path = ProceduralGenerationAlgorithms.SimpleRandomWalk(currentPosition, randomWalkParameters.walkLength);
            floorPositions.UnionWith(path);
            
            //Check if the walk should start on a random point each iteration or not
            if (randomWalkParameters.startRandomlyEachIteration)
            {
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }
        }
        //Returns the floor coordinates
        return floorPositions;
    }
}
