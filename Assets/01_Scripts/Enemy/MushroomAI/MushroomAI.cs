using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Pathfinding;
using UnityEngine.Serialization;

public class MushroomAI : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] private float speed = 1f;
    [SerializeField] private float detectionCircle = 5f;
    [SerializeField] private float stopDistance = 1f;
    [SerializeField] private LayerMask detectionMask;
    private ContactFilter2D _contactFilter;
    
    [Header("Chase")]
    [SerializeField] [Range(0, 1)] private float chaseFactor=1f;
    
    [SerializeField] private float escapeCircle = 7f;
    public bool HasDetected;

    [Header("Wander")]
    [SerializeField] [Range(0, 1)] private float wanderFactor=1f;
    
    [SerializeField] private float wanderRadius;
    [SerializeField] private float wanderPause;
    private float wanderTimer;
    
    [Header("Attack")]
    [SerializeField] [Range(0, 1)] private float attackFactor=1f;

    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange=0.5f;
    [SerializeField] private int damagePoint = 1;
    [SerializeField] float attackDelay = 2f;
    private float passedTime;
    private PlayerHealth _player;
    public bool InAttackRange;
    
    [SerializeField] private LayerMask playerMask;
    
    private Transform target;
    private float distance;
    private AIPath path;
    private EnemyHealth _enemyHealth;
    

    private Vector2 _targetDirection;
    private Vector2 hitPosition;
    
    
    private Rigidbody2D _rb;
    private Animator _animator;
    
    private const string _horizontal = "Horizontal";
    private const string _lastHorizontal = "LastHorizontal";
    private const string _attack = "Attack";
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
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        
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
        
        if (path.desiredVelocity != Vector3.zero)
        {
            _animator.SetFloat(_lastHorizontal, path.desiredVelocity.x);
        }
        
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
            //If the player escapes the enemy, he stops chasing
            if (escapeCircle < distance)
            {
                HasDetected = false;
                path.destination = RandomWanderPoint();
            }
            
            if (distance > stopDistance && !InAttackRange)
            {
                path.maxSpeed=speed;
                path.destination = target.transform.position;
            }
            else
            {
                path.maxSpeed = 0;
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
        //Stop AI movement
        if (passedTime >= attackDelay)
        {
            _animator.SetBool(_attack, true);
            
            passedTime = 0;
            
        }

        else if (distance > stopDistance)
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                _animator.SetBool(_attack, false);
                InAttackRange = false;
            }
        }
        else
        {
            _animator.SetBool(_attack, false);
        }
    }
    
    void Damage()
    {
        if (Physics2D.OverlapCircle(attackPoint.position, attackRange, playerMask))
        {
            _player.DamageTaken(3, new Vector2(attackPoint.position.x,attackPoint.position.y));
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
            Destroy(gameObject, _animator.GetCurrentAnimatorStateInfo(0).length);
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionCircle);
        Gizmos.DrawWireSphere(transform.position, escapeCircle);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private Vector2 RandomWanderPoint()
    {
        var point = Random.insideUnitCircle * wanderRadius;
        
        return point;
    }
}
