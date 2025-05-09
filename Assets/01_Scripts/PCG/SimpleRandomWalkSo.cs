using UnityEngine;

[CreateAssetMenu(fileName="SimpleRandomWalkParamters_",menuName = "RoomGeneration/SimpleRandomWalkData")]
public class SimpleRandomWalkSO : ScriptableObject
{
    //Object used to create and save presets for room designs
    [SerializeField] public int iterations = 10;
    [SerializeField] public int walkLength = 10;
    [SerializeField] public bool startRandomlyEachIteration = true;
}
