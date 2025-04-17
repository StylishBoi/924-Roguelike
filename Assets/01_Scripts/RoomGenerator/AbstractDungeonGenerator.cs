using UnityEngine;

public abstract class AbstractDungeonGenerator : MonoBehaviour
{
    [Header("General Generation Settings")]
    [SerializeField] protected TileMapVisualizer tileMapVisualizer=null;
    [SerializeField] protected Vector2Int startPosition=Vector2Int.zero;

    public void GenerateDungeon()
    {
        //Clears the previous dungeon and generates a new one
        tileMapVisualizer.Clear();
        
        //Runs the generation procedure of the given script
        //So if it is being called for 'RoomsFirstDungeonGenerator', it will generate via its procedure 
        RunProceduralGeneration();
    }
    
    //Protected - Can only be accessed by children of this class
    //Abstract - Does not contain any code, the code is given by children of this class
    protected abstract void RunProceduralGeneration();
}