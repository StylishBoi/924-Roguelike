using System;
using UnityEngine;

public class SlimeFSM : MonoBehaviour
{
    enum FSM_State
    {
        Empty,
        Wander,
        Chase,
        Dead
    }
    
    private Transform _playerCoordinates;
    private float _distanceToPlayer;
    
    private FSM_State _currentState = FSM_State.Empty;
    private SlimeAI _slime;
    private EnemyHealth _enemyHealth;

    private void Start()
    {
        _playerCoordinates=GameObject.FindGameObjectWithTag("Player").transform;
        
        if(TryGetComponent(out _enemyHealth))
        {
            //Debug.Log("EnemyHealth attached");
        }

        if (TryGetComponent(out _slime))
        {
            //Debug.Log("Slime attached");
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
        if (_enemyHealth.dead)
        {
            SetState(FSM_State.Dead);  
            return;
        }
        
        switch (state)
        {
            case FSM_State.Wander:
                if(_slime.hasDetected)
                    SetState(FSM_State.Chase);
                break;
            case FSM_State.Chase:
                if(_slime.stopDistance > _distanceToPlayer)
                    SetState(FSM_State.Dead);
                break;
            case FSM_State.Dead:
                break;
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
                _slime.WanderFactor = 1f;
                break;
            case FSM_State.Chase:
                _slime.SeekFactor = 1f;
                break;
            case FSM_State.Dead:
                _slime.DeathFactor = 1f;
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
                _slime.WanderFactor = 0f;
                break;
            case FSM_State.Chase:
                _slime.SeekFactor = 0f;
                break;
            case FSM_State.Dead:
                _slime.DeathFactor = 0f;
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
