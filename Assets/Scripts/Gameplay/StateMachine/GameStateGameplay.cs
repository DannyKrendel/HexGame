using HexGame.Input;

namespace HexGame.Gameplay.StateMachine
{
    public class GameStateGameplay : GameStateBase
    {
        private readonly HexGrid _hexGrid;
        private readonly GameCamera _gameCamera;
        private readonly GameInput _gameInput;
        
        public override GameStateType Type => GameStateType.Gameplay;

        public GameStateGameplay(GameStateMachine gameStateMachine, HexGrid hexGrid, GameCamera gameCamera, GameInput gameInput) 
            : base(gameStateMachine)
        {
            _hexGrid = hexGrid;
            _gameCamera = gameCamera;
            _gameInput = gameInput;
        }

        public override void Enter()
        {
            _gameCamera.FitCameraToBounds(_hexGrid.Bounds);
        }
    }
}