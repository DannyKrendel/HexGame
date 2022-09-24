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
        private readonly GridHighlighter _gridHighlighter;
        private readonly GameplayService _gameplayService;

        private Player _player;
        private List<HexCell> _cellsForMove;
        
        public GameStateGameplay(GameStateMachine gameStateMachine, HexGrid hexGrid, GameCamera gameCamera, 
            InputManager inputManager, GridHighlighter gridHighlighter, GameplayService gameplayService) 
            : base(gameStateMachine)
        {
            _hexGrid = hexGrid;
            _gameCamera = gameCamera;
            _inputManager = inputManager;
            _gridHighlighter = gridHighlighter;
            _gameplayService = gameplayService;
        }

        public override void Enter()
        {
            _inputManager.Enable();
            _inputManager.Click += OnClick;

            _gameplayService.SpawnPlayer();
            _player = _gameplayService.Player;
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
            if (_hexGrid.TryGetCell(worldPos, out var clickedCell) && CanPlayerMoveToCell(clickedCell))
            {
                _hexGrid.TryGetCell(_player.Movement.Coordinates, out var playerCell);
                _player.Movement.Move(clickedCell.Coordinates);
                playerCell.SubtractDurability();
            }
        }

        private void OnPlayerMoved()
        {
            UpdateCellsForMove();
            _gridHighlighter.Highlight(_cellsForMove);
        }

        private bool CanPlayerMoveToCell(HexCell cell)
        {
            return _cellsForMove.Contains(cell);
        }

        private void UpdateCellsForMove()
        {
            _cellsForMove = new List<HexCell>();
            var neighbors = _hexGrid.GetNeighbors(_player.Movement.Coordinates);
            foreach (var cell in neighbors)
            {
                if (cell.Durability > 0)
                    _cellsForMove.Add(cell);
            }
        }
    }
}