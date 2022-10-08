using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using HexGame.Input;
using HexGame.Utils;
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
        private List<Button> _availableButtonsForMitten;
        private bool _isMittenDragged;

        private CancellationTokenSource _mittenPullCancellation;
        private CancellationTokenSource _mittenLaunchCancellation;

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

            if (_isMittenDragged)
            {
                var newMittenPosition = _gameCamera.Camera.ScreenToWorldPoint(_inputManager.PointerPosition);
                newMittenPosition.z = 0;
                var cellCenter = _gridService.GetCellCenterWorld(newMittenPosition);
                _playerMitten.UpdatePositionAndRotateToPlayer(cellCenter);
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
            if (!_isMittenDragged) return;
            
            _isMittenDragged = false;
                
            _platformHighlighter.Highlight(_platformsForMove);

            var mittenButton = _availableButtonsForMitten
                .FirstOrDefault(x => x.transform.position == _playerMitten.transform.position);
                
            if (mittenButton == null)
            {
                _playerMitten.PullToPlayerImmediate();
                _playerMitten.TogglePreviewMode(false);
                _playerMitten.Hide();
            }
            else
            {
                LaunchMittenToButton(mittenButton).Forget();
            }
        }
        
        private void OnLongTap(Vector2 pointerPosition)
        {
            var worldPos = _gameCamera.Camera.ScreenToWorldPoint(pointerPosition);
            var coordinates = _gridService.WorldToCoordinates(worldPos);
            if (_player.Coordinates != coordinates || _playerMitten.IsLaunching || _playerMitten.IsPulling) 
                return;
            
            if (_playerMitten.TargetButton == null)
            {
                _mittenPullCancellation?.Cancel();
                
                _playerMitten.Show();
                _playerMitten.TogglePreviewMode(true);
                _isMittenDragged = true;
                
                UpdateAvailableButtonsForMitten();
                _platformHighlighter.Highlight(_availableButtonsForMitten.Select(x => x.ParentPlatform).ToList());
            }
            else
            {
                PullMittenToPlayer().Forget();
            }
        }

        private async UniTask PullMittenToPlayer()
        {
            var cancellation = CancellationTokenSource.CreateLinkedTokenSource(
                UniTaskUtils.RefreshToken(ref _mittenPullCancellation),
                _playerMitten.GetCancellationTokenOnDestroy());
            
            await _playerMitten.PullToPlayer(cancellation.Token);

            if (cancellation.IsCancellationRequested) 
                return;

            _playerMitten.Hide();
        }

        private async UniTask LaunchMittenToButton(Button button)
        {
            var cancellation = CancellationTokenSource.CreateLinkedTokenSource(
                UniTaskUtils.RefreshToken(ref _mittenLaunchCancellation),
                _playerMitten.GetCancellationTokenOnDestroy());
            
            _playerMitten.TogglePreviewMode(false);
            
            await _playerMitten.LaunchToTarget(button, cancellation.Token);
            
            UpdatePlatformsForMove();
            _platformHighlighter.Highlight(_platformsForMove);
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
            return !_player.Movement.IsMoving && 
                   _gameplayService.TryGetElement(coordinates, out platform) &&
                   _platformsForMove.Contains(platform);
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

        private void UpdateAvailableButtonsForMitten()
        {
            _availableButtonsForMitten = new List<Button>();
            var buttons = _gameplayService.GetElements<Button>();
            foreach (var button in buttons)
            {
                if (button.Coordinates != _player.Coordinates &&
                    button.Coordinates.X != _player.Coordinates.X && button.Coordinates.Z == _player.Coordinates.Z ||
                    button.Coordinates.X == _player.Coordinates.X && button.Coordinates.Z != _player.Coordinates.Z ||
                    button.Coordinates.X + button.Coordinates.Z == _player.Coordinates.X + _player.Coordinates.Z)
                {
                    _availableButtonsForMitten.Add(button);
                }
            }
        }
    }
}