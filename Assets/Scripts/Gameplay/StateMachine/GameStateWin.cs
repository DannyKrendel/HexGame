using HexGame.Input;

namespace HexGame.Gameplay.StateMachine
{
    public class GameStateWin : GameStateBase
    {
        public override GameStateType Type => GameStateType.Win;
        
        private readonly InputManager _inputManager;

        public GameStateWin(GameStateMachine gameStateMachine, InputManager inputManager) : base(gameStateMachine)
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