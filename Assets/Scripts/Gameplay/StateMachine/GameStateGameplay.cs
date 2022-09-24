using System.Collections.Generic;
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
        private readonly GridHighlighter _gridHighlighter;

        private Player _player;
        private List<HexCell> _cellsForMove;
        
        public GameStateGameplay(GameStateMachine gameStateMachine, HexGrid hexGrid, GameCamera gameCamera, 
            GameInput gameInput, ISpawnPoint<Player> playerSpawnPoint, GridHighlighter gridHighlighter) : base(gameStateMachine)
        {
            _hexGrid = hexGrid;
            _gameCamera = gameCamera;
            _gameInput = gameInput;
            _playerSpawnPoint = playerSpawnPoint;
            _gridHighlighter = gridHighlighter;
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
            if (_hexGrid.TryGetCell(worldMousePos, out var clickedCell) && _cellsForMove.Contains(clickedCell))
                _player.Movement.Move(clickedCell.Coordinates);
        }

        private void OnPlayerMoved()
        {
            HighlightNeighborCells();
        }

        private void HighlightNeighborCells()
        {
            _cellsForMove = _hexGrid.GetNeighbors(_player.Movement.Coordinates);
            _gridHighlighter.Highlight(_cellsForMove);
        }
    }
}