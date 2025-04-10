using UnityEngine;

public class StatAmelioration : MonoBehaviour
{
    [Header("Stats Modifier")]
    [SerializeField] private int damageIncrease = 1;
    [SerializeField] private int healthIncrease = 1;
    
    private PlayerControls _playerControls;
    private PlayerHealth _playerHealth;
    
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player").TryGetComponent(out PlayerControls outControls))
        {
            _playerControls=outControls;
        }
        
        if (GameObject.FindGameObjectWithTag("Player").TryGetComponent(out PlayerHealth outPlayer))
        {
            _playerHealth=outPlayer;
        }
    }

    private void IncreaseStats()
    {
        _playerControls.SeekDamage+=damageIncrease;
        _playerHealth.SeekHealth+=healthIncrease;
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Stat Increased");
            IncreaseStats();
        }
    }
}
