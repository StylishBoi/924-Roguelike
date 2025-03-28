using System;
using UnityEngine;

public class SlimeFSM : MonoBehaviour
{
    enum FSM_State
    {
        Empty,
        Wander,
        Chase,
        Attack
    }
    
    private FSM_State _currentState = FSM_State.Empty;
    private SlimeAI _slime;

    private void Start()
    {
        _slime=GetComponent<SlimeAI>();
        
        SetState(FSM_State.Wander);
    }

    private void Update()
    {
        CheckTransitions(_currentState);
        OnStateUpdate(_currentState);
    }
    private void CheckTransitions(FSM_State state)
    {
        
        switch (state)
        {
            case FSM_State.Wander:
                if(_slime.HasDetected) SetState(FSM_State.Chase);
                break;
            case FSM_State.Chase:
                if(!_slime.HasDetected)
                    SetState(FSM_State.Wander);
                if(_slime.HasDetected && _slime.InAttackRange)
                    SetState(FSM_State.Attack);
                break;
            case FSM_State.Attack:
                if(!_slime.InAttackRange)
                    SetState(FSM_State.Chase);
                break;
            case FSM_State.Empty:
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }


    private void OnStateEnter(FSM_State state)
    {
        Debug.Log($"OnEnter : {state}");
        
        switch (state)
        {
            case FSM_State.Wander:
                _slime.WanderFactor = 1f;
                break;
            case FSM_State.Chase:
                _slime.SeekFactor = 1f;
                break;
            case FSM_State.Attack:
                _slime.AttackFactor = 1f;
                break;
            case FSM_State.Empty:
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }

    }
    private void OnStateExit(FSM_State state)
    {
        Debug.Log($"OnExit : {state}");
        
        switch (state)
        {
            case FSM_State.Wander:
                _slime.WanderFactor = 0f;
                break;
            case FSM_State.Chase:
                _slime.SeekFactor = 0f;
                break;
            case FSM_State.Attack:
                _slime.AttackFactor = 0f;
                break;
            case FSM_State.Empty:
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }
    private void OnStateUpdate(FSM_State state)
    {
        Debug.Log($"OnUpdate : {state}");

        switch (state)
        {
            case FSM_State.Chase:
                break;
            
            case FSM_State.Wander:
            case FSM_State.Attack: 
                // Nothing to do yet
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
