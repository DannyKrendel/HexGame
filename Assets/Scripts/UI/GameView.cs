using System;
using HexGame.Gameplay;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace HexGame.UI
{
    public class GameView : MonoBehaviour
    {
        [SerializeField] private Button _pauseButton;

        private GameStateManager _gameStateManager;
        private MenuManager _menuManager;

        [Inject]
        private void Construct(GameStateManager gameStateManager, MenuManager menuManager)
        {
            _gameStateManager = gameStateManager;
            _menuManager = menuManager;
        }

        private void Awake()
        {
            _pauseButton.onClick.AddListener(OnPauseButtonPressed);
        }

        private void OnPauseButtonPressed()
        {
            _gameStateManager.UpdateGameState(GameStateType.Pause);
            _menuManager.Show(MenuType.PauseMenu);
        }
    }
}