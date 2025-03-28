using UnityEngine;

public class MushroomAttackHitBox : MonoBehaviour
{
    [SerializeField] private int enemyDamage;
    
    Collider2D attackCollider;
    void Start()
    {
        attackCollider = GetComponent<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Hit");
            Vector2 damagePosition=new Vector2(transform.position.x,transform.position.y);
            other.collider.SendMessage("DamageTaken", enemyDamage);
        }
    }
}
