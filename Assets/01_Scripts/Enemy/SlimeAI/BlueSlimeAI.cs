using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Pathfinding;

public class BlueSlimeAI : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private float detectionCircle = 7f;
    [SerializeField] private float stopDistance = 1f;
    [SerializeField] private LayerMask detectionMask;
    
    [Header("Chase")]
    [SerializeField] [Range(0, 1)] private float chaseFactor=1f;
    public bool HasDetected;

    [Header("Wander")]
    [SerializeField] [Range(0, 1)] private float wanderFactor=1f;
    [SerializeField] private float wanderRadius;
    [SerializeField] private float wanderPause;
    private float wanderTimer;
    
    [Header("Attack")]
    [SerializeField] [Range(0, 1)] private float attackFactor=1f;
    [SerializeField] private int explosionDamage = 3;
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private GameObject explosionCircle;
    [SerializeField] private GameObject babySlime;
    public bool InAttackRange;
    
    private float passedTime;
    private Transform target;
    private PlayerHealth playerHealth;
    private float distance;
    private AIPath path;
    private EnemyHealth _enemyHealth;
    

    private Vector2 _targetDirection;
    private Vector2 hitPosition;
    
    private ContactFilter2D _contactFilter;
    
    private Rigidbody2D _rb;
    private Animator _animator;
    
    private const string _horizontal = "Horizontal";
    private const string _dead = "Dead";
    
    public float SeekFactor    {
        get => chaseFactor;
        set => chaseFactor = value;
    }
    public float WanderFactor
    {
        get => wanderFactor;
        set => wanderFactor = value;
    }
    
    public float AttackFactor
    {
        get => attackFactor;
        set => attackFactor = value;
    }

    
    void Start()
    {
        explosionCircle.SetActive(false);
        
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _enemyHealth = GetComponent<EnemyHealth>();
        
        _contactFilter.SetLayerMask(detectionMask);
        
        path = GetComponentInParent<AIPath>();
        
        path.destination = RandomWanderPoint();
    }

    private void Update()
    {
        //Animation
        _animator.SetFloat(_horizontal, path.desiredVelocity.x);
        
        //FSM States
        if (_enemyHealth.Dead)
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
        if (attackFactor > 0)
        {
            Attack();
        }

        //Attack cooldown
        if (InAttackRange)
        {
            passedTime += Time.deltaTime;
        }
    }

    void Chase()
    {
        distance=Vector2.Distance(transform.position,target.transform.position);
        Vector2 direction=target.transform.position-transform.position; 
        
        if (HasDetected)
        {
            if (distance > stopDistance)
            {
                path.maxSpeed=speed;
                path.destination = target.transform.position;
            }
            else
            {
                InAttackRange = true;
            }
        }
    }
    
    void Wander()
    {
        //Check if the AI is close to the wander point and starts a timer if they are
        if ((path.destination - transform.position).magnitude < 1f)
        {
            wanderTimer += Time.deltaTime;
        }

        //When timer is completed, the AI wanders again and the timer reset
        if (wanderTimer > wanderPause)
        {
            path.destination = RandomWanderPoint();
            wanderTimer = 0;
        }
        
        //Player detection
        List<Collider2D> colliders = new List<Collider2D>();
        Physics2D.OverlapCircle(new Vector2(transform.position.x,transform.position.y), detectionCircle, _contactFilter, colliders);
       
        
        Collider2D goodObject = colliders.FirstOrDefault(c => c.CompareTag("Player"));
        if (goodObject != null)
        {
            Vector2 goodObjectDistance = goodObject.bounds.center - transform.position;
            
            if (distance < detectionCircle)
            {
                List<RaycastHit2D> hit = new List<RaycastHit2D>();
                if (Physics2D.Raycast(new Vector2(transform.position.x,transform.position.y), goodObjectDistance, _contactFilter, hit, detectionCircle) > 0)
                {
                    hitPosition=hit[0].point;
                    if (hit[0].collider == goodObject)
                    {
                        HasDetected = true;
                    }
                }
            }
        }
    }
    
    void Attack()
    {
        if (InAttackRange)
        {
            path.maxSpeed = 0;
            Death();
        }
    }

    void Death()
    {
        _animator.SetBool(_dead, true);
        path.destination = transform.position;

        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            explosionCircle.SetActive(true);
            Destroy(gameObject, _animator.GetCurrentAnimatorStateInfo(0).length);
        }
    }

    public void Explosion()
    {
        Instantiate(babySlime, transform.position, Quaternion.identity);
        Instantiate(babySlime, transform.position, Quaternion.identity);
        if (distance < explosionRadius)
        {
            playerHealth.DamageTaken(explosionDamage, new Vector2(transform.position.x,transform.position.y));
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
        
        return point;
    }
}
