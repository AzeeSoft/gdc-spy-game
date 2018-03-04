using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class StateMachine <T>
{
    private readonly T _owner;
    private State _currentState;

    public StateMachine(T owner)
    {
        _owner = owner;
    }

    public void SwitchState(State newState)
    {
        if (_currentState != null)
        {
            _currentState.Exit(_owner);
        }

        _currentState = newState;
        _currentState.Enter(_owner);
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
        void Enter(T owner);
        void Update(T owner);
        void Exit(T owner);
    }
}

