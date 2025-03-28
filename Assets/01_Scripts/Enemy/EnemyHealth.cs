using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 3;
    //[SerializeField] private float knockbackForce = 500f;
    
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
        if (currentHealth > 0)
        {
            currentHealth -= damage;
            flash.Flash();

            //Vector2 enemyPosition = new Vector2(transform.position.x, transform.position.y);
            //Vector2 direction =(enemyPosition-damagePosition).normalized;
            
            //_rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
        }
    }
}
