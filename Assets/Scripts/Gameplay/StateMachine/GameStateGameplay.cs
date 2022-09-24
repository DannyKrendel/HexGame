using System.Collections.Generic;
using HexGame.Input;
using UnityEngine;

namespace HexGame.Gameplay.StateMachine
{
    public class GameStateGameplay : GameStateBase
    {
        public override GameStateType Type => GameStateType.Gameplay;

        private readonly HexGrid _hexGrid;
        private readonly GameCamera _gameCamera;
        private readonly InputManager _inputManager;
        private readonly ISpawnPoint<Player> _playerSpawnPoint;
        private readonly GridHighlighter _gridHighlighter;

        private Player _player;
        private List<HexCell> _cellsForMove;
        
        public GameStateGameplay(GameStateMachine gameStateMachine, HexGrid hexGrid, GameCamera gameCamera, 
            InputManager inputManager, ISpawnPoint<Player> playerSpawnPoint, GridHighlighter gridHighlighter)
            : base(gameStateMachine)
        {
            _hexGrid = hexGrid;
            _gameCamera = gameCamera;
            _inputManager = inputManager;
            _playerSpawnPoint = playerSpawnPoint;
            _gridHighlighter = gridHighlighter;
        }

        public override void Enter()
        {
            _inputManager.Enable();
            _inputManager.Click += OnClick;

            _player = _playerSpawnPoint.Spawn();
            _player.Movement.Moved += OnPlayerMoved;
            
            _gameCamera.FitCameraToBounds(_hexGrid.Bounds);

            OnPlayerMoved();
        }

        public override void Exit()
        {
            _inputManager.Click -= OnClick;
            _player.Movement.Moved -= OnPlayerMoved;
        }

        private void OnClick(Vector2 pointerPosition)
        {
            var worldPos = _gameCamera.Camera.ScreenToWorldPoint(pointerPosition);
            if (_hexGrid.TryGetCell(worldPos, out var clickedCell) && _cellsForMove.Contains(clickedCell))
            {
                _hexGrid.TryGetCell(_player.Movement.Coordinates, out var playerCell);
                _player.Movement.Move(clickedCell.Coordinates);
                playerCell.SubtractDurability();
            }
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