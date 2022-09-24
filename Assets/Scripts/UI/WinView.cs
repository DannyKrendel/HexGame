using HexGame.Gameplay;
using HexGame.Gameplay.StateMachine;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace HexGame.UI
{
    public class WinView : MonoBehaviour
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _mainMenuButton;
        
        private GameStateMachine _gameStateMachine;
        private GameplayService _gameplayService;

        [Inject]
        private void Construct(GameStateMachine gameStateMachine, GameplayService gameplayService)
        {
            _gameStateMachine = gameStateMachine;
            _gameplayService = gameplayService;
        }
        
        private void Awake()
        {
            _restartButton.onClick.AddListener(OnRestartButtonPressed);
            _mainMenuButton.onClick.AddListener(OnMainMenuButtonPressed);
        }

        private void OnRestartButtonPressed()
        {
            gameObject.SetActive(false);
            _gameplayService.RestartLevel();
            _gameStateMachine.ChangeState(GameStateType.Gameplay);
        }

        private void OnMainMenuButtonPressed()
        {
            
        }
    }
}