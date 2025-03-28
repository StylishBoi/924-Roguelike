using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [Header("Spawn Points")]
    [SerializeField] private GameObject mushroomSpawnPoint;
    [SerializeField] private GameObject slimeSpawnPoint;
    [SerializeField] private GameObject batSpawnPoint;
    [SerializeField] private GameObject giantSpawnPoint;
    [SerializeField] private GameObject blueSlimeSpawnPoint;

    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject mushroomPrefab;
    [SerializeField] private GameObject slimePrefab;
    [SerializeField] private GameObject batPrefab;
    [SerializeField] private GameObject giantPrefab;
    [SerializeField] private GameObject blueSlimePrefab;
    
    public void SpawnMushroom()
    {
        GameObject newEnemy = Instantiate(mushroomPrefab, mushroomSpawnPoint.transform.position, Quaternion.identity);
    }

    public void SpawnSlime()
    {
        GameObject newEnemy = Instantiate(slimePrefab, slimeSpawnPoint.transform.position, Quaternion.identity);
    }
    
    public void SpawnBat()
    {
        GameObject newEnemy = Instantiate(batPrefab, batSpawnPoint.transform.position, Quaternion.identity);
    }
    
    public void SpawnGiant()
    {
        GameObject newEnemy = Instantiate(giantPrefab, giantSpawnPoint.transform.position, Quaternion.identity);
    }
    
    public void SpawnBlueSlime()
    {
        GameObject newEnemy = Instantiate(blueSlimePrefab, blueSlimeSpawnPoint.transform.position, Quaternion.identity);
    }
}
