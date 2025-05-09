using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Vector2 = UnityEngine.Vector2;

public class PlayerManager : MonoBehaviour
{
    private Vector2 _movement;

    private Rigidbody2D _rb;
    private Animator _animator;
    private PlayerHealth _playerHealth;
    
    //Animation set-up
    private const string Horizontal = "Horizontal";
    private const string Vertical = "Vertical";
    private const string LastHorizontal = "LastHorizontal";
    private const string LastVertical = "LastVertical";
    private const string _attack = "Attack";
    private const string Dead = "Dead";

    private float _deathCoolDown;
    private float _attackAnimationCoolDown;
    private bool _attackAnimation;
    private bool _onlyDieOnce;

    private void Start()
    {
        //Gets the components
        if(TryGetComponent(out _rb)) {}
        if(TryGetComponent(out _animator)) {}
        if(TryGetComponent(out _playerHealth)) {}
    }

    private void Update()
    {
        //Player death
        if (_playerHealth.dead && !_onlyDieOnce)
        {
            Death();
            _rb.linearVelocity = Vector2.zero;
        }
        
        //Player movement, blocks if attacking
        if (!_attackAnimation && !_playerHealth.dead)
        {
            _movement.Set(InputManager.Movement.x, InputManager.Movement.y);
        
            _rb.linearVelocity = _movement * PlayerStats.SeekSpeed;
            
            //Player animation
            _animator.SetFloat(Horizontal, _movement.x);
            _animator.SetFloat(Vertical, _movement.y);

            if (_movement != Vector2.zero)
            {
                _animator.SetFloat(LastHorizontal, _movement.x);
                _animator.SetFloat(LastVertical, _movement.y);
            }
        }
        else
        {
            _rb.linearVelocity = Vector2.zero;
        }
        
        if(_attackAnimation && _animator.GetCurrentAnimatorStateInfo(0).length<_attackAnimationCoolDown)
        {
            _animator.SetBool(_attack, false);
            _attackAnimation = false;
        }
    }
    
    void Death()
    {
        _animator.SetBool(Dead, true);

        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            Destroy(gameObject, _animator.GetCurrentAnimatorStateInfo(0).length);
            _deathCoolDown+=Time.deltaTime;
            
            if (_animator.GetCurrentAnimatorStateInfo(0).length <= _deathCoolDown+0.1f)
            {
                GameManager.GameEnd(ScoreUI.TotalScore);
                _onlyDieOnce = true;
            }
        }
    }
}
