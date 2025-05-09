using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput _playerInput;
    
    public static Vector2 Movement;
    public static bool Attack;
    public static bool Interact;
    
    private InputAction _moveAction;
    private InputAction _attackAction;
    private InputAction _interactAction;

    private float _interactCooldown=0f;
    
    void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        
        _moveAction = _playerInput.actions["Move"];
        _attackAction = _playerInput.actions["Attack"];
        _interactAction= _playerInput.actions["Interact"];
    }
    
    //Why does fixed update throw it off ?
    void Update()
    {
        _interactCooldown -= Time.deltaTime;
        
        Movement=_moveAction.ReadValue<Vector2>();
        Attack = _attackAction.IsPressed();
        
        if (_interactCooldown <= 0f && _interactAction.IsPressed())
        {
            _interactCooldown = 1f;
            Interact = _interactAction.IsPressed();
        }
        else
        {
            Interact = false;
        }
    }
}
