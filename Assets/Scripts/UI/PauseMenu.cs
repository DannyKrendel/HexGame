using HexGame.Gameplay;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace HexGame.UI
{
    public class PauseMenu : MenuBase
    {
        [SerializeField] private Button _resumeButton;

        public override MenuType Type => MenuType.PauseMenu;
        
        private GameStateManager _gameStateManager;

        [Inject]
        private void Construct(GameStateManager gameStateManager)
        {
            _gameStateManager = gameStateManager;
        }
        
        private void Awake()
        {
            _resumeButton.onClick.AddListener(OnResumeButtonPressed);
        }

        private void OnResumeButtonPressed()
        {
            _gameStateManager.UpdateGameState(GameStateType.Gameplay);
            MenuManager.HideCurrent();
        }
    }
}