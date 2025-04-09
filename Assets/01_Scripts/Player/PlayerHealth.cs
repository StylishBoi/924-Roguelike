using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private float knockbackForce = 500f;
    [SerializeField] private float _invincibilityTimer=3f;
    public bool Dead;

    public int currentHealth;
    private float _invincibilityCooldown;
    private Rigidbody2D _rb;
    private DamageFlash flash;
    
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
        flash=GetComponent<DamageFlash>();
        _rb = GetComponent<Rigidbody2D>();
        
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
            Dead = true;
        }
        
        _invincibilityCooldown+=Time.deltaTime;
    }
    
    public void DamageTaken(int damage, Vector2 damagePosition)
    {
        if (_invincibilityTimer < _invincibilityCooldown)
        {
            flash.Flash();
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
