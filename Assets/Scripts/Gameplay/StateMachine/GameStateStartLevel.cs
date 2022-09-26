using HexGame.Input;

namespace HexGame.Gameplay.StateMachine
{
    public class GameStateStartLevel : GameStateBase
    {
        public override GameStateType Type => GameStateType.StartLevel;

        private readonly InputManager _inputManager;
        private readonly GameplayService _gameplayService;
        private readonly GameCamera _gameCamera;
        private readonly GameSettings _gameSettings;

        public GameStateStartLevel(GameStateMachine gameStateMachine, InputManager inputManager, 
            GameplayService gameplayService, GameCamera gameCamera, GameSettings gameSettings) : base(gameStateMachine)
        {
            _inputManager = inputManager;
            _gameplayService = gameplayService;
            _gameCamera = gameCamera;
            _gameSettings = gameSettings;
        }

        public override void Enter()
        {
            _inputManager.Enable();
            _gameplayService.SpawnPlayer();
            _gameCamera.FitCameraToBounds(_gameplayService.GetLevelBounds(), _gameSettings.CameraPadding);
            GameStateMachine.ChangeState(GameStateType.Gameplay);
        }
    }
}