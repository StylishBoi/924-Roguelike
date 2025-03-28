using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Pathfinding;
using UnityEngine.Serialization;

public class BatAI : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] private float detectionCircle = 15f;
    [SerializeField] private LayerMask detectionMask;
    
    [Header("Observation")]
    [SerializeField] [Range(0, 1)] private float observationFactor=1f;
    public bool HasDetected;
    
    [Header("Attack")]
    [SerializeField] [Range(0, 1)] private float attackFactor=1f;
    [SerializeField] private float shotTimer=3f;
    [SerializeField] private GameObject projectilePrefab;
    private float shotCooldown;
    
    private Transform target;
    private float distance;
    private EnemyHealth _enemyHealth;
    
    private Vector2 hitPosition;
    
    private ContactFilter2D _contactFilter;
    
    private Rigidbody2D _rb;
    private Animator _animator;
    
    private const string _horizontal = "Horizontal";
    private const string _attack = "Attack";
    private const string _dead = "Dead";
    
    public float WanderFactor
    {
        get => observationFactor;
        set => observationFactor = value;
    }
    
    public float AttackFactor
    {
        get => attackFactor;
        set => attackFactor = value;
    }

    
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _enemyHealth = GetComponent<EnemyHealth>();
        
        _contactFilter.SetLayerMask(detectionMask);
        
        shotCooldown=shotTimer;
    }

    private void Update()
    {
        //Animation
        _animator.SetFloat(_horizontal, target.position.x - transform.position.x);
        
        //FSM States
        if (_enemyHealth.Dead)
        {
            Death();
        }
        if (observationFactor > 0)
        {
            Observation();
        }
        if (attackFactor > 0)
        {
            Attack();
        }

        if (HasDetected)
        {
            shotCooldown += Time.deltaTime;
        }
    }
    
    void Observation()
    {
        //Player detection
        List<Collider2D> colliders = new List<Collider2D>();
        Physics2D.OverlapCircle(new Vector2(transform.position.x,transform.position.y), detectionCircle, _contactFilter, colliders);
       
        
        distance=Vector2.Distance(transform.position,target.transform.position);
        
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
        if (shotCooldown > shotTimer)
        {
            _animator.SetBool(_attack, true);
            shotCooldown = 0;
        }
        
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            _animator.SetBool(_attack, false);
        }
    }

    void ProjectileThrow()
    {
        Instantiate(projectilePrefab, transform.position, Quaternion.identity);
    }
    
    void Death()
    {
        _animator.SetBool(_dead, true);

        //Destroy the enemy after the death animation is played
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            Destroy(gameObject, _animator.GetCurrentAnimatorStateInfo(0).length);
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionCircle);
    }
}

