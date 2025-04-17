using System.Collections.Generic;
using UnityEngine;

public static class WallGenerator
{
    public static void CreateWalls(HashSet<Vector2Int> floorPositions, TileMapVisualizer tileMapVisualizer)
    {
        //Intakes all the positions of the walls where they can placed
        var basicWallPositions = FindWallsInDirection(floorPositions, Direction2D.cardinalDirectionList);
        
        //Place every wall possible
        foreach (var position in basicWallPositions)
        {
            tileMapVisualizer.PaintSingleBasicWall(position);
        }
    }

    private static HashSet<Vector2Int> FindWallsInDirection(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionList)
    {
        //Create a "list" where the coordinates of placable walls will be stored
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
        
        //Goes through every floor tiles and check if walls can be placed around them
        foreach (var position in floorPositions)
        {
            foreach (var direction in directionList)
            {
                //Verify if the tile is empty and stores the position so a wall can be placed
                var neighbourPosition = position + direction;
                if (floorPositions.Contains(neighbourPosition) == false)
                {
                    wallPositions.Add(neighbourPosition);
                }
            }
        }

        //Returns the "list" of placable wall coordinates
        return wallPositions;
    }
}
