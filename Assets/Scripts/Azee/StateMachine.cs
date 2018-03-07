using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[Serializable]
public class StateMachine <T>
{
    private readonly T _owner;
    private State _previousState;
    private State _currentState;

    public StateMachine(T owner)
    {
        _owner = owner;
    }

    public State GetCurrentState()
    {
        return _currentState;
    }

    public State GetPreviousState()
    {
        return _previousState;
    }

    public void SwitchState(State newState, params object[] args)
    {
        if (_currentState != null)
        {
            _currentState.Exit(_owner);
            _previousState = _currentState;
        }

        _currentState = newState;
        _currentState.Enter(_owner, args);
    }

    public void Update()
    {
        if (_currentState != null)
        {
            _currentState.Update(_owner);
        }
    }

    public interface State
    {
        void Enter(T owner, params object[] args);
        void Update(T owner);
        void Exit(T owner);
    }
}

