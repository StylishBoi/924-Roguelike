using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private float knockbackForce = 500f;
    [SerializeField] private float invincibilityTimer=1f;
    public bool dead;

    public int currentHealth;
    private float _invincibilityCooldown;
    private Rigidbody2D _rb;
    private DamageFlash _flash;
    
    public int SeekHealth    {
        get => maxHealth;
        set => maxHealth = value;
    }
    
    public int GainHealth    {
        get => currentHealth;
        set => currentHealth = value;
    }
    
    private void Start()
    {
        if(TryGetComponent(out _rb))
        {
            Debug.Log("Rigidbody attached");
        }
        if(TryGetComponent(out _flash))
        {
            Debug.Log("Flash attached");
        }
        
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        
        if (currentHealth <= 0)
        {
            dead = true;
        }
        
        _invincibilityCooldown+=Time.deltaTime;
    }
    
    public void DamageTaken(int damage, Vector2 damagePosition)
    {
        if (invincibilityTimer < _invincibilityCooldown)
        {
            _flash.Flash();
            currentHealth -= damage;

            Vector2 playerPosition = new Vector2(transform.position.x, transform.position.y);
            Vector2 direction =(playerPosition-damagePosition).normalized;
            Vector2 knockback = direction * knockbackForce;
        
            _rb.AddForce(knockback, ForceMode2D.Impulse);
            
            _invincibilityCooldown = 0f;
        }
    }
    
    public void HealthGained(int health)
    {
        currentHealth -= health;
    }
}
