using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Vector2 _movement;
    private InputAction attack;

    private Rigidbody2D _rb;
    private Animator _animator;
    private PlayerHealth _playerHealth;

    [Header("Weapon")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRange=0.5f;
    [SerializeField] private float attackDelay = 0.8f;
    [SerializeField] private int damagePoint=1;
    [SerializeField] private LayerMask enemyLayers;
    private float attackCooldown;
    
    Collider2D swordCollider;
    
    private const string _horizontal = "Horizontal";
    private const string _vertical = "Vertical";
    private const string _lastHorizontal = "LastHorizontal";
    private const string _lastVertical = "LastVertical";
    private const string _attack = "Attack";
    private const string _dead = "Dead";

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _playerHealth = GetComponent<PlayerHealth>();
    }

    private void Update()
    {
        //Player attack
        if (InputManager.Attack)
        {
            Attack();
        }
        
        //Player movement, blocks if attacking
        if (!_playerHealth.Dead && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            _movement.Set(InputManager.Movement.x, InputManager.Movement.y);
        
            _rb.linearVelocity = _movement * moveSpeed;
        }
        //Player death
        else if (_playerHealth.Dead)
        {
            Death();
        }
        
        //Player animation
        _animator.SetFloat(_horizontal, _movement.x);
        _animator.SetFloat(_vertical, _movement.y);

        if (_movement != Vector2.zero)
        {
            _animator.SetFloat(_lastHorizontal, _movement.x);
            _animator.SetFloat(_lastVertical, _movement.y);
        }
        
        //Attack cooldown
        attackCooldown+=Time.deltaTime;
    }

    public void Attack()
    {
        if (attackCooldown >= attackDelay)
        {
            _animator.SetBool(_attack, true);
            
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<EnemyHealth>().DamageTaken(damagePoint,new Vector2(attackPoint.position.x,attackPoint.position.y));
            }
            attackCooldown = 0;
        }
        
        else
        {
            _animator.SetBool(_attack, false);
        }
    }
    
    void Death()
    {
        _animator.SetBool(_dead, true);

        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            Destroy(gameObject, _animator.GetCurrentAnimatorStateInfo(0).length);
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    
}
