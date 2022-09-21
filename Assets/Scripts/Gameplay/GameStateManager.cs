using UnityEngine;
using Zenject;

namespace HexGame.Gameplay
{
    public class GameStateManager : MonoBehaviour
    {
        private HexGrid _hexGrid;
        private GameCamera _gameCamera;

        [Inject]
        private void Construct(HexGrid hexGrid, GameCamera gameCamera)
        {
            _hexGrid = hexGrid;
            _gameCamera = gameCamera;
        }
        
        private void Start()
        {
            _gameCamera.FitCameraToBounds(_hexGrid.Bounds);
        }
        
        private GameStateType _currentGameState;
        private GameStateType _previousGameState;

        public void UpdateGameState(GameStateType newGameState)
        {
            if (newGameState == _currentGameState) return;

            _previousGameState = _currentGameState;
            _currentGameState = newGameState;
        }

        public void GoToPreviousGameState()
        {
            if (_previousGameState == _currentGameState) return;

            (_previousGameState, _currentGameState) = (_currentGameState, _previousGameState);
        }
    }
}