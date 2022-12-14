using Cysharp.Threading.Tasks;
using HexGame.Gameplay;
using HexGame.Gameplay.StateMachine;
using HexGame.UI.Animation;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;
using Button = UnityEngine.UI.Button;

namespace HexGame.UI
{
    public class WinScreen : MonoBehaviour, IScreen
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private FadeAnimation _fadeAnimation;
        
        private GameStateMachine _gameStateMachine;
        private GameplayService _gameplayService;
        private SceneService _sceneService;

        [Inject]
        private void Construct(GameStateMachine gameStateMachine, GameplayService gameplayService,
            SceneService sceneService)
        {
            _gameStateMachine = gameStateMachine;
            _gameplayService = gameplayService;
            _sceneService = sceneService;
        }
        
        private void Awake()
        {
            _restartButton.onClick.AddListener(OnRestartButtonPressed);
            _mainMenuButton.onClick.AddListener(OnMainMenuButtonPressed);
        }
        
        public async UniTask Show()
        {
            gameObject.SetActive(true);
            await _fadeAnimation.PlayFadeIn();
        }

        public async UniTask Hide()
        {
            await _fadeAnimation.PlayFadeOut();
            gameObject.SetActive(false);
        }

        private void OnRestartButtonPressed()
        {
            Hide().Forget();
            _gameplayService.RestartLevel();
            _gameStateMachine.ChangeState(GameStateType.Gameplay);
        }

        private void OnMainMenuButtonPressed()
        {
            GoToMainMenu().Forget();
        }

        private async UniTask GoToMainMenu()
        {
            await _sceneService.SwitchSceneAsync(1, true);
        }
    }
}