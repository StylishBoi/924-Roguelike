using UnityEngine;

public class BossScript : MonoBehaviour
{
    [SerializeField] private GameObject exit;
    
    private EnemyHealth _enemyHealth;
    private bool _exitSpawned;
    
    void Start()
    {
        if(TryGetComponent(out _enemyHealth)) {}
    }

    void Update()
    {
        if (_enemyHealth.dead)
        {
            if (!_exitSpawned)
            {
                Instantiate(exit, transform.position, Quaternion.identity, transform.parent);
                _exitSpawned = true;
            }
        }
    }
}
