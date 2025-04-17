using UnityEngine;

public class RoomControl : MonoBehaviour
{
    [SerializeField] private RoomSettings roomSettings;
    public Bounds bounds_;

    void Start()
    {
        if (roomSettings.isBossRoom)
        {
            Instantiate(roomSettings.listOfEnemies[0], transform.position, Quaternion.identity, transform);
            return;
        }

        if (roomSettings.hasItem)
        {
            Instantiate(roomSettings.listOfItems[Random.Range(0,1)], transform.position, Quaternion.identity, transform);
            return;
        }
        if (roomSettings.hasEnemies)
        {
            RoomEnemySpawn();
        }
    }

    public void SetBounds(BoundsInt bounds, int roomShrinkage)
    {
        bounds_ = new Bounds(bounds.center, new Vector3(bounds.size.x - roomShrinkage*2, bounds.size.y - roomShrinkage*2, bounds.size.z));
    }
    
    void RoomEnemySpawn()
    {
        Vector2 spawnPos = new Vector2(Random.Range(bounds_.min.x, bounds_.max.x), Random.Range(bounds_.min.y, bounds_.max.y));
        int random = Random.Range(0, 4);
        switch (random)
        {
            case 0:
                Instantiate(roomSettings.listOfEnemies[0], spawnPos, Quaternion.identity, transform);
                Instantiate(roomSettings.listOfEnemies[0], spawnPos, Quaternion.identity, transform);
                break;
            case 1:
                Instantiate(roomSettings.listOfEnemies[1], spawnPos, Quaternion.identity, transform);
                Instantiate(roomSettings.listOfEnemies[2], spawnPos, Quaternion.identity, transform);
                break;
            case 2:
                Instantiate(roomSettings.listOfEnemies[3], spawnPos, Quaternion.identity, transform);
                break;
            case 3:
                Instantiate(roomSettings.listOfEnemies[1], spawnPos, Quaternion.identity, transform);
                Instantiate(roomSettings.listOfEnemies[3], spawnPos, Quaternion.identity, transform);
                break;
            case 4:
                Instantiate(roomSettings.listOfEnemies[2], spawnPos, Quaternion.identity, transform);
                Instantiate(roomSettings.listOfEnemies[2], spawnPos, Quaternion.identity, transform);
                Instantiate(roomSettings.listOfEnemies[2], spawnPos, Quaternion.identity, transform);
                break;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(bounds_.center, 1);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(bounds_.center, bounds_.size);
        
        
        //Gizmos.DrawWireCube(room.center, new Vector3(room.size.x-(distanceBetweenRooms+1),room.size.y-(distanceBetweenRooms+1),0f));
    }
}
