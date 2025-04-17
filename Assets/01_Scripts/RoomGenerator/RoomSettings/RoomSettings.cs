using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName="RoomSettings_",menuName = "RoomGeneration/RoomSettings")]
public class RoomSettings : ScriptableObject
{
    public BoundsInt bounds;
    public bool hasEnemies;
    public List<GameObject> enemies; //0 - Mushroom, 1 - Slime, 2 - Bat, 3 - Blue Slime, 4 - Giant Mushroom
    public bool isBossRoom;
}
