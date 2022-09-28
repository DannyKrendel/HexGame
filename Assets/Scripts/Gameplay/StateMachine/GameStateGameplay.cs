using System.Collections.Generic;
using HexGame.Input;
using UnityEngine;

namespace HexGame.Gameplay.StateMachine
{
    public class GameStateGameplay : GameStateBase
    {
        public override GameStateType Type => GameStateType.Gameplay;
        
        private readonly GameCamera _gameCamera;
        private readonly InputManager _inputManager;
        private readonly PlatformHighlighter _platformHighlighter;
        private readonly GameplayService _gameplayService;
        private readonly GridService _gridService;

        private Player _player;
        private List<Platform> _platformsForMove;

        public GameStateGameplay(GameStateMachine gameStateMachine, GameCamera gameCamera, InputManager inputManager, 
            PlatformHighlighter platformHighlighter, GameplayService gameplayService, GridService gridService) 
            : base(gameStateMachine)
        {
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
            if (_gameplayService.TryGetElement(coordinates, out Platform clickedPlatform) &&
                CanPlayerMoveToPlatform(clickedPlatform))
            {
                if (_gameplayService.TryGetElementUnderPlayer(out Button button))
                    button.Release();
                
                if (_gameplayService.TryGetElementUnderPlayer(out Platform playerPlatform))
                    playerPlatform.SubtractDurability();
                
                _player.Movement.Move(clickedPlatform.Coordinates);
            }
        }

        private void OnPlayerMoved()
        {
            UpdatePlatformsForMove();
            _platformHighlighter.Highlight(_platformsForMove);

            if (_gameplayService.TryGetElementUnderPlayer(out Fish fish))
                fish.Consume();

            if (_gameplayService.TryGetElementUnderPlayer(out Button button))
                button.Press();
        }

        private bool CanPlayerMoveToPlatform(Platform platform)
        {
            return !_player.Movement.IsMoving && _platformsForMove.Contains(platform);
        }

        private void UpdatePlatformsForMove()
        {
            _platformsForMove = new List<Platform>();
            var neighbors = _gameplayService.GetNeighbors(_player.Coordinates);
            foreach (var platform in neighbors)
            {
                if (platform.Durability > 0)
                    _platformsForMove.Add(platform);
            }
        }
    }
}