using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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
        private PlayerMitten _playerMitten;
        private List<Platform> _platformsForMove;
        private bool _canMoveMitten;

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
            _inputManager.TapStarted += OnTapStarted;
            _inputManager.TapEnded += OnTapEnded;
            _inputManager.LongTap += OnLongTap;
            
            _player = _gameplayService.Player;
            _player.Movement.Moved += OnPlayerMoved;

            _playerMitten = _gameplayService.PlayerMitten;
            
            OnPlayerMoved();
        }

        public override void Exit()
        {
            _inputManager.TapStarted -= OnTapStarted;
            _inputManager.TapEnded -= OnTapEnded;
            _inputManager.LongTap -= OnLongTap;
            _player.Movement.Moved -= OnPlayerMoved;
        }

        public override void Update()
        {
            if (_gameplayService.IsWin())
                GameStateMachine.ChangeState(GameStateType.Win);

            if (_canMoveMitten)
            {
                var newMittenPosition = _gameCamera.Camera.ScreenToWorldPoint(_inputManager.PointerPosition);
                newMittenPosition.z = 0;
                _playerMitten.UpdatePositionAndRotate(newMittenPosition);
            }
        }

        private void OnTapStarted(Vector2 pointerPosition)
        {
            var worldPos = _gameCamera.Camera.ScreenToWorldPoint(pointerPosition);
            var coordinates = _gridService.WorldToCoordinates(worldPos);
            if (CanPlayerMoveToCoords(coordinates, out var clickedPlatform))
            {
                if (_gameplayService.TryGetElementUnderPlayer(out Button button))
                    button.Release();
                
                if (_gameplayService.TryGetElementUnderPlayer(out Platform playerPlatform))
                    playerPlatform.SubtractDurability();
                
                _player.Movement.Move(clickedPlatform.Coordinates);
            }
        }

        private void OnTapEnded(Vector2 pointerPosition)
        {
            PullMittenToPlayer().Forget();
        }

        private async UniTask PullMittenToPlayer()
        {
            _canMoveMitten = false;
            await _playerMitten.PullToPlayer();
            _playerMitten.Hide();
        }
        
        private void OnLongTap(Vector2 pointerPosition)
        {
            var worldPos = _gameCamera.Camera.ScreenToWorldPoint(pointerPosition);
            var coordinates = _gridService.WorldToCoordinates(worldPos);
            if (_player.Coordinates == coordinates)
            {
                _playerMitten.Show();
                _canMoveMitten = true;
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

        private bool CanPlayerMoveToCoords(HexCoordinates coordinates, out Platform platform)
        {
            platform = null;
            return !_player.Movement.IsMoving && _gameplayService.TryGetElement(coordinates, out platform)
                    && _platformsForMove.Contains(platform);
        }

        private void UpdatePlatformsForMove()
        {
            _platformsForMove = new List<Platform>();
            var neighbors = _gameplayService.GetNeighbors(_player.Coordinates);
            foreach (var platform in neighbors)
            {
                _gameplayService.TryGetElement(platform.Coordinates, out Door door);
                
                if (!platform.IsBroken && door && door.IsOpen || !door && !platform.IsBroken)
                    _platformsForMove.Add(platform);
            }
        }
    }
}