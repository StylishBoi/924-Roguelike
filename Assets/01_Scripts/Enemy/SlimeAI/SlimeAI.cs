using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Pathfinding;


[RequireComponent(typeof(Animator))]
public class SlimeAI : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private float detectionCircle = 7f;
    public float stopDistance = 1f;
    [SerializeField] private LayerMask detectionMask;
    private ContactFilter2D _contactFilter;
    [SerializeField] private bool isBlueSlime;
    
    [Header("Chase")]
    [SerializeField] [Range(0, 1)] private float chaseFactor=1f;
    public bool hasDetected;

    [Header("Wander")]
    [SerializeField] [Range(0, 1)] private float wanderFactor=1f;
    [SerializeField] private float wanderRadius;
    [SerializeField] private float wanderPause;
    private float _wanderTimer;
    
    [Header("Death")]
    [SerializeField] [Range(0, 1)] private float deathFactor=1f;
    [SerializeField] private int explosionDamage = 3;
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private GameObject explosionCircle;
    [SerializeField] private GameObject babySlime;
    
    private Transform _target;
    private PlayerHealth _playerHealth;
    private float _distance;
    private AIPath _path;

    private Vector2 _targetDirection;
    private Vector2 hitPosition;
    
    
    private Animator _animator;
    
    private const string Horizontal = "Horizontal";
    private const string Dead = "Dead";
    
    public float SeekFactor    {
        get => chaseFactor;
        set => chaseFactor = value;
    }
    public float WanderFactor
    {
        get => wanderFactor;
        set => wanderFactor = value;
    }
    
    public float DeathFactor
    {
        get => deathFactor;
        set => deathFactor = value;
    }

    
    void Start()
    {
        explosionCircle.SetActive(false);
        
        if (GameObject.FindGameObjectWithTag("Player").TryGetComponent(out Transform outTarget))
        {
            _target = outTarget;
        }

        if (GameObject.FindGameObjectWithTag("Player").TryGetComponent(out PlayerHealth outPlayer))
        {
            _playerHealth = outPlayer;
        }
        if(TryGetComponent(out _animator))
        {
            Debug.Log("Animator attached");
        }
        if(TryGetComponent(out _path))
        {
            Debug.Log("AIPath attached");
        }
        
        _contactFilter.SetLayerMask(detectionMask);
        
        //Makes the enemy start by wandering
        _path.destination = RandomWanderPoint();
    }

    private void Update()
    {
        //Animation
        _animator.SetFloat(Horizontal, _path.desiredVelocity.x);
        
        //FSM States
        if (deathFactor > 0)
        {
            Death();
        }
        if (chaseFactor > 0)
        {
            Chase();
        }
        if (wanderFactor > 0)
        {
            Wander();
        }
    }

    void Chase()
    {
        _distance=Vector2.Distance(transform.position,_target.transform.position);
        
        _path.maxSpeed=speed;
        _path.destination = _target.transform.position;
        
    }
    
    void Wander()
    {
        //Cauculate distance between enemy and player
        _distance=Vector2.Distance(transform.position,_target.transform.position);
        
        //Updates timer of wandering, does not wait for enemy to reach his destination to avoid blockage
        _wanderTimer += Time.deltaTime;

        //When timer is completed, the AI wanders again and the timer reset
        if (_wanderTimer > wanderPause)
        {
            _path.destination = RandomWanderPoint();
            _wanderTimer = 0;
        }
        
        //Player detection
        List<Collider2D> colliders = new List<Collider2D>();
        Physics2D.OverlapCircle(new Vector2(transform.position.x,transform.position.y), detectionCircle, _contactFilter, colliders);
        
        Collider2D goodObject = colliders.FirstOrDefault(c => c.CompareTag("Player"));
        if (goodObject != null)
        {
            Vector2 goodObjectDistance = goodObject.bounds.center - transform.position;
            
            if (_distance < detectionCircle)
            {
                List<RaycastHit2D> hit = new List<RaycastHit2D>();
                if (Physics2D.Raycast(new Vector2(transform.position.x,transform.position.y), goodObjectDistance, _contactFilter, hit, detectionCircle) > 0)
                {
                    hitPosition=hit[0].point;
                    if (hit[0].collider == goodObject)
                    {
                        hasDetected = true;
                    }
                }
            }
        }
    }

    void Death()
    {
        _animator.SetBool(Dead, true);
        
        //Blocks enemy movement when dying
        _path.maxSpeed = 0;
        _path.destination = transform.position;

        //Destroy the enemy after the death animation is played
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            explosionCircle.SetActive(true);
            Destroy(gameObject, _animator.GetCurrentAnimatorStateInfo(0).length);
        }
    }

    public void Explosion()
    {
        _distance=Vector2.Distance(transform.position,_target.transform.position);
        
        //Damage the player if he's in the explosion radius
        if (_distance < explosionRadius)
        {
            _playerHealth.DamageTaken(explosionDamage, new Vector2(transform.position.x,transform.position.y));
        }

        if (isBlueSlime)
        {
            Instantiate(babySlime, transform.position, Quaternion.identity);
            Instantiate(babySlime, transform.position, Quaternion.identity);
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionCircle);
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    private Vector2 RandomWanderPoint()
    {
        var point = Random.insideUnitCircle * wanderRadius;
        point = new Vector2(transform.position.x + point.x,transform.position.y + point.y);
        
        return point;
    }
}

