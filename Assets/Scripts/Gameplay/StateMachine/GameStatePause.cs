﻿using HexGame.Input;

namespace HexGame.Gameplay.StateMachine
{
    public class GameStatePause : GameStateBase
    {
        public override GameStateType Type => GameStateType.Pause;
        
        private readonly InputManager _inputManager;

        public GameStatePause(GameStateMachine gameStateMachine, InputManager inputManager) : base(gameStateMachine)
        {
            _inputManager = inputManager;
        }

        public override void Enter()
        {
            _inputManager.Disable();
        }

        public override void Exit()
        {
            _inputManager.Enable();
        }
    }
}