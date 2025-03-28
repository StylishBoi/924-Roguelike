using UnityEngine;

public class BatProjectile : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    
    private Transform player;
    private Vector2 target;
    private Rigidbody2D _rb;

    private Vector2 direction;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        target= new Vector2(player.position.x, player.position.y);
        
        direction = (player.position - transform.position).normalized;
    }

    void FixedUpdate()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name);
        
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Hit");
            DestroyProjectile();
            other.gameObject.GetComponent<PlayerHealth>().DamageTaken(1,new Vector2(transform.position.x,transform.position.y));
        }
        if (other.CompareTag("Walls") || other.CompareTag("Roof"))
        {
            Debug.Log("Wall Hit");
            DestroyProjectile();
        }
    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
