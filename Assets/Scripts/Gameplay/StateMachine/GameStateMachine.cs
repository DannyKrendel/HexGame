using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace HexGame.Gameplay.StateMachine
{
    public class GameStateMachine : MonoBehaviour, IStateMachine
    {
        private GameStateBase _currentState;
        private GameStateBase _previousState;
        private Dictionary<GameStateType, GameStateBase> _states;

        public GameStateType CurrentState { get; private set; }
        public event Action StateChanged;

        [Inject]
        private void Construct(List<GameStateBase> states)
        {
            _states = states.ToDictionary(state => state.Type);
        }
        
        private void Start()
        {
            Initialize();
        }
        
        public void Initialize()
        {
            ChangeState(GameStateType.Gameplay);
        }

        private void Update()
        {
            _currentState.Update();
        }

        private void FixedUpdate()
        {
            _currentState.FixedUpdate();
        }

        public void ChangeState(GameStateType gameStateType)
        {
            var newGameState = _states[gameStateType];
            
            if (newGameState == _currentState) return;

            _previousState = _currentState;
            _currentState = newGameState;

            _previousState?.Exit();
            _currentState.Enter();
            
            CurrentState = _currentState.Type;
            StateChanged?.Invoke();
        }

        public void GoToPreviousState()
        {
            if (_previousState == null || _previousState == _currentState) return;

            (_previousState, _currentState) = (_currentState, _previousState);
            
            _previousState?.Exit();
            _currentState.Enter();
            
            CurrentState = _currentState.Type;
            StateChanged?.Invoke();
        }
    }

    public enum GameStateType
    {
        Gameplay, Pause, Win
    }
}