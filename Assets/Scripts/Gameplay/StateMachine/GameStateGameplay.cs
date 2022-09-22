namespace HexGame.Gameplay.StateMachine
{
    public class GameStateGameplay : GameStateBase
    {
        private readonly HexGrid _hexGrid;
        private readonly GameCamera _gameCamera;
        
        public override GameStateType Type => GameStateType.Gameplay;

        public GameStateGameplay(GameStateMachine gameStateMachine, HexGrid hexGrid, GameCamera gameCamera) : base(gameStateMachine)
        {
            _hexGrid = hexGrid;
            _gameCamera = gameCamera;
        }

        public override void Enter()
        {
            _gameCamera.FitCameraToBounds(_hexGrid.Bounds);
        }
    }
}