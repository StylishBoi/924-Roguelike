using System.Collections;
using Unity.VisualScripting;
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
    private float _attackCooldown;
    
    private Collider2D _swordCollider;
    
    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";
    private const string LastHorizontal = "LastHorizontal";
    private const string LastVertical = "LastVertical";
    private const string _attack = "Attack";
    private const string Dead = "Dead";
    
    public int SeekDamage    {
        get => damagePoint;
        set => damagePoint = value;
    }

    private void Start()
    {
        if(TryGetComponent(out _rb))
        {
            Debug.Log("Rigidbody attached");
        }
        if(TryGetComponent(out _animator))
        {
            Debug.Log("Animator attached");
        }
        if(TryGetComponent(out _playerHealth))
        {
            Debug.Log("Player Health attached");
        }
    }

    private void Update()
    {
        //Player attack
        if (InputManager.Attack)
        {
            Attack();
        }
        
        //Player movement, blocks if attacking
        if (!_playerHealth.dead && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            _movement.Set(InputManager.Movement.x, InputManager.Movement.y);
        
            _rb.linearVelocity = _movement * moveSpeed;
        }
        //Player death
        else if (_playerHealth.dead)
        {
            Death();
        }
        
        //Player animation
        _animator.SetFloat(Horizontal, _movement.x);
        _animator.SetFloat(Vertical, _movement.y);

        if (_movement != Vector2.zero)
        {
            _animator.SetFloat(LastHorizontal, _movement.x);
            _animator.SetFloat(LastVertical, _movement.y);
        }
        
        //Attack cooldown
        _attackCooldown+=Time.deltaTime;
    }

    public void Attack()
    {
        if (_attackCooldown >= attackDelay)
        {
            _animator.SetBool(_attack, true);
            
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

            foreach (Collider2D enemy in hitEnemies)
            {
                if(enemy.TryGetComponent(out EnemyHealth enemyHealth))
                {
                    enemyHealth.DamageTaken(damagePoint,new Vector2(attackPoint.position.x,attackPoint.position.y));
                }
            }
            _attackCooldown = 0;
        }
        
        else
        {
            _animator.SetBool(_attack, false);
        }
    }
    
    void Death()
    {
        _animator.SetBool(Dead, true);

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
