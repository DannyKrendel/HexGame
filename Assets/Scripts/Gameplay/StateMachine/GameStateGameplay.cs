using HexGame.Input;
using UnityEngine;

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
            _gameCamera.FitCameraToBounds(_hexGrid.Bounds);
            _player = _playerSpawnPoint.Spawn();
        }

        public override void Update()
        {
            var mousePos = _gameInput.UI.Point.ReadValue<Vector2>();
            var worldMousePos = _gameCamera.Camera.ScreenToWorldPoint(mousePos);
            
            if (_hexGrid.TryGetCellByWorldPosition(worldMousePos, out var cell))
                cell.Select();
        }
    }
}