using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Pathfinding;

[RequireComponent(typeof(Animator))]
public class MushroomAI : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] private float speed = 1f;
    [SerializeField] private float detectionCircle = 5f;
    public float stopDistance = 1f;
    [SerializeField] private LayerMask detectionMask;
    [SerializeField] private LayerMask playerMask;
    private ContactFilter2D _contactFilter;
    
    [Header("Chase")]
    [SerializeField] [Range(0, 1)] private float chaseFactor=1f;
    public float escapeCircle = 7f;
    public bool hasDetected;

    [Header("Wander")]
    [SerializeField] [Range(0, 1)] private float wanderFactor=1f;
    [SerializeField] private float wanderRadius;
    [SerializeField] private float wanderPause;
    private float _wanderTimer;
    
    [Header("Attack")]
    [SerializeField] [Range(0, 1)] private float attackFactor=1f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange=0.5f;
    [SerializeField] private int damagePoint = 1;
    [SerializeField] float attackDelay = 2f;
    public bool attackAnimationPlaying;
    private float _passedTime;
    private PlayerHealth _player;
    
    [Header("Death")]
    [SerializeField] [Range(0, 1)] private float deathFactor=1f;
    
    private Transform _target;
    private float _distanceToPlayer;
    private AIPath _path;

    private Vector2 _targetDirection;
    private Vector2 _hitPosition;
    
    private Animator _animator;
    
    private const string Horizontal = "Horizontal";
    private const string LastHorizontal = "LastHorizontal";
    private const string _attack = "Attack";
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
    
    public float AttackFactor
    {
        get => attackFactor;
        set => attackFactor = value;
    }
    public float DeathFactor
    {
        get => deathFactor;
        set => deathFactor = value;
    }

    
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player").TryGetComponent(out Transform outTarget))
        {
            _target = outTarget;
        }
        if (GameObject.FindGameObjectWithTag("Player").TryGetComponent(out PlayerHealth outPlayer))
        {
            _player=outPlayer;
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
        
        if (_path.desiredVelocity != Vector3.zero)
        {
            _animator.SetFloat(LastHorizontal, _path.desiredVelocity.x);
        }
        
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
        if (attackFactor > 0)
        {
            Attack();
            _passedTime += Time.deltaTime;
        }
    }

    void Chase()
    {
        _distanceToPlayer=Vector2.Distance(transform.position,_target.transform.position);
            
        //Continues chasing the player till he gets close enough
        if (_distanceToPlayer > stopDistance)
        {
            _path.maxSpeed=speed;
            _path.destination = _target.transform.position;
        }
    }
    
    void Wander()
    {
        _distanceToPlayer=Vector2.Distance(transform.position,_target.transform.position);
        
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
            
            if (_distanceToPlayer < detectionCircle)
            {
                List<RaycastHit2D> hit = new List<RaycastHit2D>();
                if (Physics2D.Raycast(new Vector2(transform.position.x,transform.position.y), goodObjectDistance, _contactFilter, hit, detectionCircle) > 0)
                {
                    _hitPosition=hit[0].point;
                    if (hit[0].collider == goodObject)
                    {
                        hasDetected = true;
                    }
                }
            }
        }
    }
    
    void Attack()
    {
        //Disable movement when in attack mode
        _path.destination = transform.position;
        
        //Start the AI attack animation
        if (_passedTime >= attackDelay)
        {
            _animator.SetBool(_attack, true);
            attackAnimationPlaying = true;
            
            _passedTime = 0;
            
        }
        
        else if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) 
        {
            _animator.SetBool(_attack, false);
            attackAnimationPlaying = false;
        }
        
        //Makes animation go back to idle after attack animation is played
        else
        {
            _animator.SetBool(_attack, false);
        }
    }
    
    void Damage()
    {
        //If the player is in the attack circle, he will take damage
        if (Physics2D.OverlapCircle(attackPoint.position, attackRange, playerMask))
        {
            _player.DamageTaken(damagePoint, new Vector2(attackPoint.position.x,attackPoint.position.y));
        }
    }
    
    void Death()
    {
        _animator.SetBool(Dead, true);
        
        //Blocks enemy movement when dying
        _path.destination = transform.position;

        //Destroy the enemy after the death animation is played
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            Destroy(gameObject, _animator.GetCurrentAnimatorStateInfo(0).length);
        }
    }
    
    private void OnDrawGizmos()
    {
        //Draws the detection and escape circle
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionCircle);
        Gizmos.DrawWireSphere(transform.position, escapeCircle);
        
        //Draws the attack circle
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private Vector2 RandomWanderPoint()
    {
        var point = Random.insideUnitCircle * wanderRadius;
        point = new Vector2(transform.position.x + point.x,transform.position.y + point.y);
        
        return point;
    }
}
