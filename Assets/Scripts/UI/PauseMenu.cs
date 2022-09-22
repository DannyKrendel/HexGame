using HexGame.Gameplay.StateMachine;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace HexGame.UI
{
    public class PauseMenu : MenuBase
    {
        [SerializeField] private Button _resumeButton;

        public override MenuType Type => MenuType.PauseMenu;
        
        private GameStateMachine _gameStateMachine;

        [Inject]
        private void Construct(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }
        
        private void Awake()
        {
            _resumeButton.onClick.AddListener(OnResumeButtonPressed);
        }

        private void OnResumeButtonPressed()
        {
            _gameStateMachine.GoToPreviousState();
            MenuManager.HideCurrent();
        }
    }
}