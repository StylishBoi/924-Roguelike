using UnityEditor;
using UnityEngine;

//Makes editor appear for children of the 'Abstract Dungeon Generator' class
[CustomEditor(typeof(AbstractDungeonGenerator), true)]
public class RandomDungeonGeneratorEditor : Editor
{
    AbstractDungeonGenerator generator;

    private void Awake()
    {
        //Finds scripts using the 'Abstract Dungeon Generator' class
        generator = (AbstractDungeonGenerator)target;
    }

    public override void OnInspectorGUI()
    {
        //Creates a button which will generate a dungeon when pressed
        base.OnInspectorGUI();
        if (GUILayout.Button("Create Dungeon"))
        {
            //Will decide which method of generation to do based on the script that was the button was pressed on
            generator.GenerateDungeon();
        }
    }
}
