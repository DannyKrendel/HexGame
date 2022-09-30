using HexGame.Input;
using UnityEngine;

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
            Time.timeScale = 0;
            _inputManager.Disable();
        }

        public override void Exit()
        {
            Time.timeScale = 1;
            _inputManager.Enable();
        }
    }
}