using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static Vector2 Movement;
    public static bool Attack;
    
    private static InputManager instance;
    
    private PlayerInput _playerInput;
    
    private InputAction _moveAction;
    private InputAction _attackAction;
    
    private bool attackPressed = false;
    
    void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        
        _moveAction = _playerInput.actions["Move"];
        _attackAction = _playerInput.actions["Attack"];
    }
    
    void FixedUpdate()
    {
        Movement=_moveAction.ReadValue<Vector2>();
        Attack = _attackAction.IsPressed();
    }
}
