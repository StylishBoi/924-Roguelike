using UnityEngine;

public class HealthRestore : MonoBehaviour
{
    [Header("Stats Modifier")]
    [SerializeField] [Range(1,3)] private int healthGain = 1;
    
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
        AudioManager.Instance.PlaySfx(AudioManager.Instance.healthSFX);
        ScoreUI.Instance.ScoreUpdate(5);
        _playerHealth.HealthGained(healthGain);
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
