using HexGame.Gameplay;
using HexGame.Gameplay.StateMachine;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace HexGame.UI
{
    public class GameView : MonoBehaviour
    {
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _restartButton;

        private GameStateMachine _gameStateMachine;
        private MenuManager _menuManager;
        private GameplayService _gameplayService;

        [Inject]
        private void Construct(GameStateMachine gameStateMachine, MenuManager menuManager, GameplayService gameplayService)
        {
            _gameStateMachine = gameStateMachine;
            _menuManager = menuManager;
            _gameplayService = gameplayService;
        }

        private void Awake()
        {
            _pauseButton.onClick.AddListener(OnPauseButtonPressed);
            _restartButton.onClick.AddListener(OnRestartButtonPressed);
        }

        private void OnPauseButtonPressed()
        {
            _gameStateMachine.ChangeState(GameStateType.Pause);
            _menuManager.Show(MenuType.PauseMenu);
        }

        private void OnRestartButtonPressed()
        {
            _gameplayService.RestartLevel();
        }
    }
}