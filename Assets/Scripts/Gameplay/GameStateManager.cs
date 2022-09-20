using UnityEngine;

namespace HexGame.Gameplay
{
    public class GameStateManager : MonoBehaviour
    {
        [SerializeField] private HexGrid _hexGrid;
        [SerializeField] private CameraController _cameraController;

        private void Start()
        {
            _cameraController.FitCameraToBounds(_hexGrid.Bounds);
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