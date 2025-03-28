using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private float knockbackForce = 500f;
    
    private int currentHealth;
    private Rigidbody2D _rb;
    public bool Dead;
    
    private DamageFlash flash;
    
    private void Start()
    {
        flash=GetComponent<DamageFlash>();
        _rb = GetComponent<Rigidbody2D>();
        
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            Dead = true;
        }
    }
    
    public void DamageTaken(int damage, Vector2 damagePosition)
    {
        flash.Flash();
        currentHealth -= damage;

        Vector2 playerPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 direction =(playerPosition-damagePosition).normalized;
        Vector2 knockback = direction * knockbackForce;
        
        _rb.AddForce(knockback, ForceMode2D.Impulse);
    }
    
    public void HealthGained(int health)
    {
        currentHealth -= health;
    }
}
