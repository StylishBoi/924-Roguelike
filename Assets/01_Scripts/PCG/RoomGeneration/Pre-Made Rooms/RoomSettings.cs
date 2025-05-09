using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName="RoomSettings_",menuName = "RoomGeneration/RoomSettings")]
public class RoomSettings : ScriptableObject
{
    [Header("Room Settings")]
    public BoundsInt bounds;
    
    [Header("Enemy Settings")]
    public bool hasEnemies;
    public List<GameObject> listOfEnemies; //0 - Mushroom, 1 - Slime, 2 - Bat, 3 - Blue Slime, 4 - Giant Mushroom
    public bool isBossRoom;
    
    [Header("Item Settings")]
    public bool hasItem;
    public List<GameObject> listOfItems; //0 - Health Recovery, 1 - Weapon Upgrade
}
