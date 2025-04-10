using System;
using UnityEngine;

public class MushroomFSM : MonoBehaviour
{
    enum FSM_State
    {
        Empty,
        Wander,
        Chase,
        Attack,
        Dead
    }

    private Transform _playerCoordinates;
    private float _distanceToPlayer;
    
    private FSM_State _currentState = FSM_State.Empty;
    private MushroomAI _mushroom;
    private EnemyHealth _enemyHealth;

    private void Start()
    {
        _playerCoordinates=GameObject.FindGameObjectWithTag("Player").transform;
        
        if(TryGetComponent(out _enemyHealth))
        {
            Debug.Log("EnemyHealth attached");
        }
        if(TryGetComponent(out _mushroom))
        {
            Debug.Log("Mushroom attached");
        }
        
        SetState(FSM_State.Wander);
    }

    private void Update()
    {
        CheckTransitions(_currentState);
        OnStateUpdate(_currentState);
    }
    private void CheckTransitions(FSM_State state)
    {
        if (_enemyHealth.Dead)
        {
            SetState(FSM_State.Dead);  
            return;
        }
        
        switch (state)
        {
            case FSM_State.Wander:
                if(_mushroom.hasDetected)
                    SetState(FSM_State.Chase);
                break;
            
            case FSM_State.Chase:
                if (_distanceToPlayer > _mushroom.escapeCircle)
                {
                    _mushroom.hasDetected = false;
                    SetState(FSM_State.Wander);
                }
                else if(_distanceToPlayer<_mushroom.stopDistance)
                    SetState(FSM_State.Attack);
                break;
            
            case FSM_State.Attack:
                if (_distanceToPlayer>_mushroom.stopDistance && !_mushroom.attackAnimationPlaying)
                    SetState(FSM_State.Chase);
                break;
            
            case FSM_State.Dead:
            case FSM_State.Empty:
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }


    private void OnStateEnter(FSM_State state)
    {
        //Debug.Log($"OnEnter : {state}");
        
        switch (state)
        {
            case FSM_State.Wander:
                _mushroom.WanderFactor = 1f;
                break;
            case FSM_State.Chase:
                _mushroom.SeekFactor = 1f;
                break;
            case FSM_State.Attack:
                _mushroom.AttackFactor = 1f;
                break;
            case FSM_State.Dead:
                _mushroom.DeathFactor = 1f;
                break;
            case FSM_State.Empty:
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }

    }
    private void OnStateExit(FSM_State state)
    {
        //Debug.Log($"OnExit : {state}");
        
        switch (state)
        {
            case FSM_State.Wander:
                _mushroom.WanderFactor = 0f;
                break;
            case FSM_State.Chase:
                _mushroom.SeekFactor = 0f;
                break;
            case FSM_State.Attack:
                _mushroom.AttackFactor = 0f;
                break;
            case FSM_State.Dead:
                _mushroom.DeathFactor = 0f;
                break;
            case FSM_State.Empty:
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
    private void OnStateUpdate(FSM_State state)
    {
        //Debug.Log($"OnUpdate : {state}");

        switch (state)
        {
            case FSM_State.Chase:
            case FSM_State.Attack: 
                _distanceToPlayer=Vector2.Distance(transform.position,_playerCoordinates.transform.position);
                break;
            case FSM_State.Wander:
            case FSM_State.Dead:
                break;
            case FSM_State.Empty:
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    private void SetState(FSM_State newState)
    {
        if (newState == FSM_State.Empty) return;
        if(_currentState != FSM_State.Empty) OnStateExit(_currentState);
        
        _currentState = newState;
        OnStateEnter(_currentState);
        
    }
}
