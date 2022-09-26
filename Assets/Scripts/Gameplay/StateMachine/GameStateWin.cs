using Cysharp.Threading.Tasks;
using HexGame.Input;

namespace HexGame.Gameplay.StateMachine
{
    public class GameStateWin : GameStateBase
    {
        public override GameStateType Type => GameStateType.Win;
        
        private readonly InputManager _inputManager;
        private readonly LevelManager _levelManager;
        private readonly FishManager _fishManager;
        private readonly SaveManager _saveManager;

        public GameStateWin(GameStateMachine gameStateMachine, InputManager inputManager,
            LevelManager levelManager, FishManager fishManager, SaveManager saveManager) : base(gameStateMachine)
        {
            _inputManager = inputManager;
            _levelManager = levelManager;
            _fishManager = fishManager;
            _saveManager = saveManager;
        }
        
        public override void Enter()
        {
            _inputManager.Disable();
            _saveManager.UpdateLevelData(
                _levelManager.CurrentLevelId, 
                true, 
                _fishManager.ConsumedFishCount,
                _fishManager.AllFishCount);
            _saveManager.Save().Forget();
        }

        public override void Exit()
        {
            _inputManager.Enable();
        }
    }
}