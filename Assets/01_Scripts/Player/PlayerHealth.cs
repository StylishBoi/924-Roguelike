using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 5;
    [SerializeField] private GameObject floatingTextPrefab;
    [SerializeField] private float knockbackForce = 500f;
    [SerializeField] private float invincibilityTimer=1f;
    public bool dead;

    public int currentHealth;
    private float _invincibilityCooldown;
    private Rigidbody2D _rb;
    private DamageFlash _flash;
    private HealthUI _healthUi;
    private Collider2D _collider2D;
    
    public int SeekHealth    {
        get => maxHealth;
        set => maxHealth = value;
    }
    
    public int GainHealth    {
        get => currentHealth;
        set => currentHealth = value;
    }
    
    private void Awake()
    {
        currentHealth = maxHealth;
        
        if(TryGetComponent(out _rb)) {}
        if(TryGetComponent(out _flash)) {}
        if(TryGetComponent(out _collider2D)){}
        
        if(GameObject.FindGameObjectWithTag("HealthUI").TryGetComponent(out HealthUI ui))
        {
            _healthUi = ui;
        }
        
    }

    void Update()
    {
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        
        if (currentHealth <= 0 && !dead)
        {
            AudioManager.Instance.PlaySfx(AudioManager.Instance.playerDeathSFX);
            _collider2D.enabled = false;
            dead = true;
        }
        
        _invincibilityCooldown+=Time.deltaTime;
    }
    
    public void DamageTaken(int damage, Vector2 damagePosition)
    {
        if (invincibilityTimer < _invincibilityCooldown)
        {
            AudioManager.Instance.PlaySfx(AudioManager.Instance.playerHitSFX);
            _healthUi.LowerHealth(damage, currentHealth);
            UIManager.Instance.Hurt();
            _flash.Flash(invincibilityTimer);
            currentHealth -= damage;

            Vector2 playerPosition = new Vector2(transform.position.x, transform.position.y);
            Vector2 direction =(playerPosition-damagePosition).normalized;
            Vector2 knockback = direction * knockbackForce;
        
            _rb.AddForce(knockback, ForceMode2D.Impulse);
            
            _invincibilityCooldown = 0f;
            
            if (floatingTextPrefab)
            {
                ShowFloatingText(damage);
            }
        }
    }
    
    public void HealthGained(int health)
    {
        currentHealth += health;
        UIManager.Instance.Healed();
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        
        if (floatingTextPrefab)
        {
            ShowFloatingText(health);
        }
        
        _healthUi.IncreaseHealth(health, currentHealth);
    }
    
    void ShowFloatingText(int textNumber)
    {
        var go = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity, transform);
        if (go.TryGetComponent<TextMesh>(out TextMesh textMesh))
        {
            textMesh.text=textNumber.ToString();
        }
    }
}
