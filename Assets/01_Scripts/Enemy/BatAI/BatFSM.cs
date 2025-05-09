using System;
using UnityEngine;

public class BatFSM : MonoBehaviour
{
    enum FSM_State
    {
        Empty,
        Observation,
        Attack,
        Dead
    }
    
    private FSM_State _currentState = FSM_State.Empty;
    private BatAI _bat;
    private EnemyHealth _enemyHealth;

    private void Start()
    {
        if(TryGetComponent(out _enemyHealth))
        {
            //Debug.Log("EnemyHealth attached");
        }
        if(TryGetComponent(out _bat))
        {
            //Debug.Log("Bat attached");
        }
        
        SetState(FSM_State.Observation);
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
            case FSM_State.Observation:
                if(_bat.hasDetected)
                    SetState(FSM_State.Attack);
                break;
            case FSM_State.Attack:
                if(!_bat.hasDetected)
                    SetState(FSM_State.Observation);
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
            case FSM_State.Observation:
                _bat.WanderFactor = 1f;
                break;
            case FSM_State.Attack:
                _bat.AttackFactor = 1f;
                break;
            case FSM_State.Dead:
                _bat.DeathFactor = 1f;
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
            case FSM_State.Observation:
                _bat.WanderFactor = 0f;
                break;
            case FSM_State.Attack:
                _bat.AttackFactor = 0f;
                break;
            case FSM_State.Dead:
                _bat.DeathFactor = 0f;
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
            case FSM_State.Observation:
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
