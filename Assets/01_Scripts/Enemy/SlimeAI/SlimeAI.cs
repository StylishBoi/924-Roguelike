using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Pathfinding;

public class SlimeAI : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private float detectionCircle = 7f;
    [SerializeField] private float stopDistance = 1f;
    [SerializeField] private LayerMask detectionMask;
    private ContactFilter2D _contactFilter;
    
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
    public bool InAttackRange;
    
    private Transform target;
    private PlayerHealth playerHealth;
    private float distance;
    private AIPath path;
    private EnemyHealth _enemyHealth;

    private Vector2 _targetDirection;
    private Vector2 hitPosition;
    
    
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
        path = GetComponentInParent<AIPath>();
        
        _contactFilter.SetLayerMask(detectionMask);
        
        //Makes the enemy start by wandering
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
    }

    void Chase()
    {
        distance=Vector2.Distance(transform.position,target.transform.position);
        Vector2 direction=target.transform.position-transform.position; 
        
        if (HasDetected)
        {
            //Only stops chasing when he gets close to the player
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
        
        //Blocks enemy movement when dying
        path.destination = transform.position;

        //Destroy the enemy after the death animation is played
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            explosionCircle.SetActive(true);
            Destroy(gameObject, _animator.GetCurrentAnimatorStateInfo(0).length);
        }
    }

    public void Explosion()
    {
        //Damage the player if he's in the explosion radius
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

