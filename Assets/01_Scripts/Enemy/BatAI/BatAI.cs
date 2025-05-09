using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Animator))]
public class BatAI : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] private float detectionCircle = 15f;
    [SerializeField] private LayerMask detectionMask;
    
    [Header("Observation")]
    [SerializeField] [Range(0, 1)] private float observationFactor=1f;
    public bool hasDetected;
    
    [Header("Attack")]
    [SerializeField] [Range(0, 1)] private float attackFactor=1f;
    [SerializeField] private float shotTimer=3f;
    [SerializeField] private GameObject projectilePrefab;
    private float _shotCooldown;
    
    [Header("Death")]
    [SerializeField] [Range(0, 1)] private float deathFactor=1f;
    
    private Transform _target;
    private float _distance;
    
    private Vector2 hitPosition;
    
    private ContactFilter2D _contactFilter;
    private Animator _animator;
    
    private const string Horizontal = "Horizontal";
    private const string _attack = "Attack";
    private const string Dead = "Dead";
    
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
        
        if(TryGetComponent(out _animator))
        {
            //Debug.Log("Animator attached");
        }
        
        _contactFilter.SetLayerMask(detectionMask);
        
        _shotCooldown=shotTimer;
    }

    private void Update()
    {
        //Animation
        _animator.SetFloat(Horizontal, _target.position.x - transform.position.x);
        
        //FSM States
        if (deathFactor > 0)
        {
            Death();
        }
        if (observationFactor > 0)
        {
            Observation();
        }
        if (attackFactor > 0)
        {
            _shotCooldown += Time.deltaTime;
            Attack();
            Observation();
        }
    }
    
    void Observation()
    {
        //Player detection
        List<Collider2D> colliders = new List<Collider2D>();
        Physics2D.OverlapCircle(new Vector2(transform.position.x,transform.position.y), detectionCircle, _contactFilter, colliders);
        
        _distance=Vector2.Distance(transform.position,_target.transform.position);
        
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
                        return;
                    }
                }
            }
        }
        hasDetected = false;
    }
    
    void Attack()
    {
        if (_shotCooldown > shotTimer)
        {
            _animator.SetBool(_attack, true);
            _shotCooldown = 0;
        }
        
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            _animator.SetBool(_attack, false);
        }
    }

    void ProjectileThrow()
    {
        Instantiate(projectilePrefab, transform.position, Quaternion.identity, transform);
    }
    
    void Death()
    {
        _animator.SetBool(Dead, true);

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

