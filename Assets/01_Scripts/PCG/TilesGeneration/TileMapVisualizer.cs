using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapVisualizer : MonoBehaviour
{
    [SerializeField] Tilemap floorTilemap;
    [SerializeField] Tilemap wallTilemap;
    
    [SerializeField] private RuleTile floorTile;
    [SerializeField] private RuleTile wallTop;

    //Painting floor and walls use different functions to faciliate the need of intaking new parameters 
    
    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        //Paint a floor tile via a different function
        PaintTiles(floorPositions, floorTilemap, floorTile);
    }
    
    public void PaintSingleBasicWall(Vector2Int position)
    {
        //Paint a wall tile via a different function
        PaintSingleTile(wallTilemap, wallTop, position);
    }

    private void PaintTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tile)
    {
        //Goes through every position and place a tile for it
        foreach (var position in positions)
        {
            PaintSingleTile(tilemap, tile, position);
        }
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        //Set a tile based on the position given
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePosition, tile);
    }


    public void Clear()
    {
        //Removes all the tiles on the map
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
    }
}
