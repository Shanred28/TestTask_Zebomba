using System;
using System.Collections.Generic;

namespace Infrastructure.Services.GameStateMachine
{
    public class GameStateMachine : IGameStateSwitcher
    {
        public object CurrentState => _currentState;
        
        private Dictionary<Type, object> _states;
        private object _currentState;
        
        public GameStateMachine()
        {
            _states = new Dictionary<Type, object>();
        }

        public void AddState<TState>(TState state) where TState : class, IState
        {
            _states.Add(state.GetType(), state);
        }

        public void RemoveState<TState>() where TState : class, IState
        {
            _states.Remove(typeof(TState));
        }

        public void EnterState<TState>() where TState : class, IState
        {
            if (_currentState != null && typeof(TState) == _currentState.GetType()) return;

            if(_currentState is IExitableState exitableState) exitableState.Exit();

            TState state = _states[typeof(TState)] as TState;
            _currentState = state;
            if (_currentState is IEnterableState enterabletate) enterabletate.Enter();
        }

        public void ExitState<TState>() where TState : class, IState
        {
            if (_currentState is IExitableState exitableState) exitableState.Exit();

            _currentState = null;
        }
    }
}
