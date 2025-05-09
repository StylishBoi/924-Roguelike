using UnityEngine;

public class BatProjectile : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    
    private Transform player;
    private Rigidbody2D _rb;

    private Vector2 direction;

    void Start()
    {
        //Gets the commpoments
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if(TryGetComponent(out _rb)) {}
        
        //Creates the destination
        direction = player.position - transform.position;
        
        //Makes the bullet move
        _rb.linearVelocity = direction.normalized * speed;
        
        //Make bullet sound
        AudioManager.Instance.PlaySfx(AudioManager.Instance.batProjectileSFX);
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
