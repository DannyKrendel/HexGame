using HexGame.Gameplay.StateMachine;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace HexGame.UI
{
    public class GameView : MonoBehaviour
    {
        [SerializeField] private Button _pauseButton;

        private GameStateMachine _gameStateMachine;
        private MenuManager _menuManager;

        [Inject]
        private void Construct(GameStateMachine gameStateMachine, MenuManager menuManager)
        {
            _gameStateMachine = gameStateMachine;
            _menuManager = menuManager;
        }

        private void Awake()
        {
            _pauseButton.onClick.AddListener(OnPauseButtonPressed);
        }

        private void OnPauseButtonPressed()
        {
            _gameStateMachine.ChangeState(GameStateType.Pause);
            _menuManager.Show(MenuType.PauseMenu);
        }
    }
}