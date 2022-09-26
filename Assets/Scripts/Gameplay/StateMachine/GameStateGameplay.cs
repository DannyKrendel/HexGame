using System.Collections.Generic;
using HexGame.Input;
using UnityEngine;

namespace HexGame.Gameplay.StateMachine
{
    public class GameStateGameplay : GameStateBase
    {
        public override GameStateType Type => GameStateType.Gameplay;

        private readonly PlatformManager _platformManager;
        private readonly FishManager _fishManager;
        private readonly GameCamera _gameCamera;
        private readonly InputManager _inputManager;
        private readonly PlatformHighlighter _platformHighlighter;
        private readonly GameplayService _gameplayService;
        private readonly GridService _gridService;

        private Player _player;
        private List<Platform> _platformsForMove;

        public GameStateGameplay(GameStateMachine gameStateMachine, PlatformManager platformManager,
            FishManager fishManager, GameCamera gameCamera, InputManager inputManager, 
            PlatformHighlighter platformHighlighter, GameplayService gameplayService, GridService gridService) 
            : base(gameStateMachine)
        {
            _platformManager = platformManager;
            _fishManager = fishManager;
            _gameCamera = gameCamera;
            _inputManager = inputManager;
            _platformHighlighter = platformHighlighter;
            _gameplayService = gameplayService;
            _gridService = gridService;
        }

        public override void Enter()
        {
            _inputManager.Click += OnClick;
            
            _player = _gameplayService.Player;
            _player.Movement.Moved += OnPlayerMoved;

            OnPlayerMoved();
        }

        public override void Exit()
        {
            _inputManager.Click -= OnClick;
            _player.Movement.Moved -= OnPlayerMoved;
        }

        public override void Update()
        {
            if (_gameplayService.IsWin())
                GameStateMachine.ChangeState(GameStateType.Win);
        }

        private void OnClick(Vector2 pointerPosition)
        {
            var worldPos = _gameCamera.Camera.ScreenToWorldPoint(pointerPosition);
            var coordinates = _gridService.WorldToCoordinates(worldPos);
            if (_platformManager.TryGetElement(coordinates, out var clickedPlatform) && 
                _platformManager.TryGetElement(_player.Coordinates, out var playerPlatform) && 
                CanPlayerMoveToPlatform(clickedPlatform))
            {
                playerPlatform.SubtractDurability();
                _player.Movement.Move(clickedPlatform.Coordinates);
            }
        }

        private void OnPlayerMoved()
        {
            UpdatePlatformsForMove();
            _platformHighlighter.Highlight(_platformsForMove);

            if (_fishManager.TryGetElement(_player.Coordinates, out var fish) &&
                fish.Coordinates == _player.Coordinates)
            {
                fish.Consume();
            }
        }

        private bool CanPlayerMoveToPlatform(Platform platform)
        {
            return !_player.Movement.IsMoving && _platformsForMove.Contains(platform);
        }

        private void UpdatePlatformsForMove()
        {
            _platformsForMove = new List<Platform>();
            var neighbors = _platformManager.GetNeighbors(_player.Coordinates);
            foreach (var platform in neighbors)
            {
                if (platform.Durability > 0)
                    _platformsForMove.Add(platform);
            }
        }
    }
}