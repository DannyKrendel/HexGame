using HexGame.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HexGame.Gameplay.StateMachine
{
    public class GameStateGameplay : GameStateBase
    {
        public override GameStateType Type => GameStateType.Gameplay;

        private readonly HexGrid _hexGrid;
        private readonly GameCamera _gameCamera;
        private readonly GameInput _gameInput;
        private readonly ISpawnPoint<Player> _playerSpawnPoint;

        private Player _player;
        
        public GameStateGameplay(GameStateMachine gameStateMachine, HexGrid hexGrid, GameCamera gameCamera, 
            GameInput gameInput, ISpawnPoint<Player> playerSpawnPoint) : base(gameStateMachine)
        {
            _hexGrid = hexGrid;
            _gameCamera = gameCamera;
            _gameInput = gameInput;
            _playerSpawnPoint = playerSpawnPoint;
        }

        public override void Enter()
        {
            _gameInput.Enable();
            _gameInput.UI.Click.performed += OnClick;

            _player = _playerSpawnPoint.Spawn();
            _player.Movement.Moved += OnPlayerMoved;
            
            _gameCamera.FitCameraToBounds(_hexGrid.Bounds);

            OnPlayerMoved();
        }

        public override void Exit()
        {
            _gameInput.UI.Click.performed -= OnClick;
            _player.Movement.Moved -= OnPlayerMoved;
        }

        private void OnClick(InputAction.CallbackContext context)
        {
            if (context.ReadValue<float>() == 0) return;
            
            var mousePos = _gameInput.UI.Point.ReadValue<Vector2>();
            var worldMousePos = _gameCamera.Camera.ScreenToWorldPoint(mousePos);
            if (_hexGrid.TryGetCell(worldMousePos, out var clickedCell))
                _player.Movement.Move(clickedCell.Coordinates);
        }

        private void OnPlayerMoved()
        {
            ClearHighlight();
            if (_hexGrid.TryGetCell(_player.Movement.Coordinates, out var playerCell))
                HighlightNeighborCells(playerCell);
        }

        private void HighlightNeighborCells(HexCell targetCell)
        {
            var neighbors = _hexGrid.GetNeighbors(targetCell.Coordinates);
            foreach (var cell in neighbors)
                cell.Highlight();
        }

        private void ClearHighlight()
        {
            foreach (var cell in _hexGrid.Cells)
                cell.ClearHighlight();
        }
    }
}