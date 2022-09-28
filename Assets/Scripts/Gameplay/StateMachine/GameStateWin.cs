using Cysharp.Threading.Tasks;
using HexGame.Input;

namespace HexGame.Gameplay.StateMachine
{
    public class GameStateWin : GameStateBase
    {
        public override GameStateType Type => GameStateType.Win;

        private readonly GameplayService _gameplayService;
        private readonly InputManager _inputManager;
        private readonly LevelManager _levelManager;
        private readonly SaveManager _saveManager;

        public GameStateWin(GameStateMachine gameStateMachine, GameplayService gameplayService, 
            InputManager inputManager, LevelManager levelManager, SaveManager saveManager) : base(gameStateMachine)
        {
            _gameplayService = gameplayService;
            _inputManager = inputManager;
            _levelManager = levelManager;
            _saveManager = saveManager;
        }
        
        public override void Enter()
        {
            _inputManager.Disable();
            _saveManager.UpdateLevelData(
                _levelManager.CurrentLevelId, 
                true, 
                _gameplayService.ConsumedFishCount,
                _gameplayService.AllFishCount);
            _saveManager.Save().Forget();
        }

        public override void Exit()
        {
            _inputManager.Enable();
        }
    }
}