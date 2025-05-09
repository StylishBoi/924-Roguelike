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
            //Increase item spawn rarity depending on the floor
            if (4 < GameManager.FloorReached)
            {
                Instantiate(roomSettings.listOfItems[Random.Range(4,roomSettings.listOfItems.Count)], transform.position, Quaternion.identity, transform);
            }
            else if (2 < GameManager.FloorReached)
            {
                Instantiate(roomSettings.listOfItems[Random.Range(0,8)], transform.position, Quaternion.identity, transform);
            }
            else
            {
                Instantiate(roomSettings.listOfItems[Random.Range(0,4)], transform.position, Quaternion.identity, transform);
            }
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
        //Vector2 spawnPos = new Vector2(Random.Range(bounds_.min.x+1, bounds_.max.x-1), Random.Range(bounds_.min.y+1, bounds_.max.y-1));
        int random = Random.Range(0, 4);
        
        switch (random)
        {
            case 0:
                Instantiate(roomSettings.listOfEnemies[0], new Vector2(Random.Range(bounds_.min.x+1, bounds_.max.x-1), Random.Range(bounds_.min.y+1, bounds_.max.y-1)), Quaternion.identity, transform);
                Instantiate(roomSettings.listOfEnemies[0], new Vector2(Random.Range(bounds_.min.x+1, bounds_.max.x-1), Random.Range(bounds_.min.y+1, bounds_.max.y-1)), Quaternion.identity, transform);
                Instantiate(roomSettings.listOfEnemies[0], new Vector2(Random.Range(bounds_.min.x+1, bounds_.max.x-1), Random.Range(bounds_.min.y+1, bounds_.max.y-1)), Quaternion.identity, transform);
                break;
            case 1:
                Instantiate(roomSettings.listOfEnemies[1], new Vector2(Random.Range(bounds_.min.x+1, bounds_.max.x-1), Random.Range(bounds_.min.y+1, bounds_.max.y-1)), Quaternion.identity, transform);
                Instantiate(roomSettings.listOfEnemies[2], new Vector2(Random.Range(bounds_.min.x+1, bounds_.max.x-1), Random.Range(bounds_.min.y+1, bounds_.max.y-1)), Quaternion.identity, transform);
                Instantiate(roomSettings.listOfEnemies[0], new Vector2(Random.Range(bounds_.min.x+1, bounds_.max.x-1), Random.Range(bounds_.min.y+1, bounds_.max.y-1)), Quaternion.identity, transform);
                break;
            case 2:
                Instantiate(roomSettings.listOfEnemies[3], new Vector2(Random.Range(bounds_.min.x+1, bounds_.max.x-1), Random.Range(bounds_.min.y+1, bounds_.max.y-1)), Quaternion.identity, transform);
                Instantiate(roomSettings.listOfEnemies[3], new Vector2(Random.Range(bounds_.min.x+1, bounds_.max.x-1), Random.Range(bounds_.min.y+1, bounds_.max.y-1)), Quaternion.identity, transform);
                Instantiate(roomSettings.listOfEnemies[3], new Vector2(Random.Range(bounds_.min.x+1, bounds_.max.x-1), Random.Range(bounds_.min.y+1, bounds_.max.y-1)), Quaternion.identity, transform);
                break;
            case 3:
                Instantiate(roomSettings.listOfEnemies[1], new Vector2(Random.Range(bounds_.min.x+1, bounds_.max.x-1), Random.Range(bounds_.min.y+1, bounds_.max.y-1)), Quaternion.identity, transform);
                Instantiate(roomSettings.listOfEnemies[1], new Vector2(Random.Range(bounds_.min.x+1, bounds_.max.x-1), Random.Range(bounds_.min.y+1, bounds_.max.y-1)), Quaternion.identity, transform);
                Instantiate(roomSettings.listOfEnemies[3], new Vector2(Random.Range(bounds_.min.x+1, bounds_.max.x-1), Random.Range(bounds_.min.y+1, bounds_.max.y-1)), Quaternion.identity, transform);
                break;
            case 4:
                Instantiate(roomSettings.listOfEnemies[2], new Vector2(Random.Range(bounds_.min.x+1, bounds_.max.x-1), Random.Range(bounds_.min.y+1, bounds_.max.y-1)), Quaternion.identity, transform);
                Instantiate(roomSettings.listOfEnemies[2], new Vector2(Random.Range(bounds_.min.x+1, bounds_.max.x-1), Random.Range(bounds_.min.y+1, bounds_.max.y-1)), Quaternion.identity, transform);
                Instantiate(roomSettings.listOfEnemies[2], new Vector2(Random.Range(bounds_.min.x+1, bounds_.max.x-1), Random.Range(bounds_.min.y+1, bounds_.max.y-1)), Quaternion.identity, transform);
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
