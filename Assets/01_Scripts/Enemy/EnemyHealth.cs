using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private GameObject floatingTextPrefab;
    [SerializeField] private GameObject healthItem;
    [SerializeField] private int maxHealth = 3;
    //[SerializeField] private float knockbackForce = 500f;
    
    private int _currentHealth;
    private Collider2D _collider2D;
    public bool dead;
    
    private DamageFlash flash;
    
    private void Start()
    {
        //Gets the components
        if(TryGetComponent(out flash)){}
        if(TryGetComponent(out _collider2D)){}
        
        //Health is based on their base health and the floor the player is currently on
        if (GameManager.FloorReached == 1)
        {
            _currentHealth = maxHealth;
        }
        else
        {
            _currentHealth = maxHealth * (GameManager.FloorReached / 2);
        }
    }

    void Update()
    {
        if (_currentHealth <= 0 && !dead)
        {
            AudioManager.Instance.PlaySfx(AudioManager.Instance.enemyDeathSFX);
            _collider2D.enabled = false;
            
            dead = true;
            
            ScoreUI.Instance.ScoreUpdate(10);
            
            /*
            if (gameObject.IsDestroyed() && Random.value < 0.85f && healthItem)
            {
                Instantiate(healthItem, transform.position, Quaternion.identity, transform);
            }*/
        }
    }
    
    public void DamageTaken(int damage, Vector2 damagePosition)
    {
        if (_currentHealth > 0)
        {
            AudioManager.Instance.PlaySfx(AudioManager.Instance.enemyHitSFX);
            _currentHealth -= damage;
            flash.Flash(0.15f);

            if (floatingTextPrefab)
            {
                ShowFloatingText(damage);
            }
        }
    }

    void ShowFloatingText(int damage)
    {
        var go = Instantiate(floatingTextPrefab, transform.position, Quaternion.identity, transform);
        if (go.TryGetComponent<TextMesh>(out TextMesh textMesh))
        {
            textMesh.text=damage.ToString();
        }
    }
}
