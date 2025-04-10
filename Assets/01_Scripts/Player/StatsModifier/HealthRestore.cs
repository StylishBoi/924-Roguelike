using UnityEngine;

public class HealthRestore : MonoBehaviour
{
    [Header("Stats Modifier")]
    [SerializeField] private int healthGain = 1;
    
    private PlayerHealth _playerHealth;
    
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player").TryGetComponent(out PlayerHealth outPlayer))
        {
            _playerHealth=outPlayer;
        }
    }

    void Heal()
    {
        _playerHealth.GainHealth+=healthGain;
        Destroy(gameObject);
    }
    
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Stat Increased");
            Heal();
        }
    }
}
