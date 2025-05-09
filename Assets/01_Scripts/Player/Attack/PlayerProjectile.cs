using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    private Vector3 mousePos;
    private Camera mainCam;
    private Rigidbody2D _rb;
    private PlayerManager _player;

    private Vector2 direction;

    void Start()
    {
        //Gets the compenents
        mainCam=GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        _player=GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        if(TryGetComponent(out _rb)) {}
        
        //Intakes all the positions
        mousePos=mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        Vector3 rotation = transform.position - mousePos;
        
        //Makes the bullet move
        _rb.linearVelocity = new Vector2(direction.x, direction.y).normalized * (PlayerStats.SeekSpeed+2);
        
        //Fix bullet sprite rotation
        /*float rot =Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation=Quaternion.Euler(0,0,rot+90);*/
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name);
        
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy Hit");
            DestroyProjectile();
            other.gameObject.GetComponent<EnemyHealth>().DamageTaken(PlayerStats.SeekDamage,new Vector2(transform.position.x,transform.position.y));
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
