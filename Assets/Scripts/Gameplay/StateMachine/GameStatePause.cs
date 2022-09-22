using HexGame.Input;

namespace HexGame.Gameplay.StateMachine
{
    public class GameStatePause : GameStateBase
    {
        private readonly GameInput _gameInput;
        
        public override GameStateType Type => GameStateType.Pause;

        public GameStatePause(GameStateMachine gameStateMachine, GameInput gameInput) : base(gameStateMachine)
        {
            _gameInput = gameInput;
        }

        public override void Enter()
        {
            _gameInput.Disable();
        }

        public override void Exit()
        {
            _gameInput.Enable();
        }
    }
}