using System.Collections.Generic;
using System.Linq;
using HexGame.Gameplay.StateMachine;
using UnityEngine;
using Zenject;

namespace HexGame.Gameplay
{
    public class GameplayService
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly HexGridElementManager<Platform> _platformManager;
        private readonly HexGridElementManager<Fish> _fishManager;
        private readonly HexGridElementManager<Button> _buttonManager;
        private readonly HexGridElementManager<Door> _doorManager;
        private readonly ISpawnPoint<Player> _playerSpawnPoint;
        private readonly LevelFinish _levelFinish;
        private readonly IFactory<Player> _playerFactory;
        private readonly IFactory<PlayerMitten> _playerMittenFactory;
        private readonly GridService _gridService;

        public Player Player { get; private set; }
        public PlayerMitten PlayerMitten { get; private set; }
        public int AllFishCount => _fishManager.Elements.Count;
        public int ConsumedFishCount => _fishManager.Elements.Count(x => x.IsConsumed);

        public GameplayService(GameStateMachine gameStateMachine, 
            HexGridElementManager<Platform> platformManager, HexGridElementManager<Fish> fishManager,
            HexGridElementManager<Button> buttonManager, HexGridElementManager<Door> doorManager,
            ISpawnPoint<Player> playerSpawnPoint, LevelFinish levelFinish, 
            IFactory<Player> playerFactory, IFactory<PlayerMitten> playerMittenFactory, GridService gridService)
        {
            _gameStateMachine = gameStateMachine;
            _platformManager = platformManager;
            _fishManager = fishManager;
            _buttonManager = buttonManager;
            _doorManager = doorManager;
            _playerSpawnPoint = playerSpawnPoint;
            _levelFinish = levelFinish;
            _playerFactory = playerFactory;
            _playerMittenFactory = playerMittenFactory;
            _gridService = gridService;

            AttachToPlatform(buttonManager.Elements);
            AttachToPlatform(doorManager.Elements);
        }

        public void SpawnPlayer()
        {
            if (Player == null)
                Player = _playerFactory.Create();
            
            _playerSpawnPoint.Spawn(Player);
        }

        public void SpawnPlayerMitten()
        {
            if (PlayerMitten == null)
                PlayerMitten = _playerMittenFactory.Create();

            PlayerMitten.SetPlayer(Player);
            PlayerMitten.Hide();
            PlayerMitten.PullToPlayerImmediate();
        }

        public bool TryGetElementUnderPlayer<T>(out T element) where T : HexGridElement
        {
            return TryGetElement(Player.Coordinates, out element);
        }
        
        public bool TryGetElement<T>(HexCoordinates coordinates, out T element) where T : HexGridElement
        {
            element = null;
            
            switch (typeof(T).Name)
            {
                case nameof(Platform):
                    _platformManager.TryGetElement(coordinates, out var platform);
                    element = (T)(object)platform;
                    break;
                case nameof(Fish):
                    _fishManager.TryGetElement(coordinates, out var fish);
                    element = (T)(object)fish;
                    break;
                case nameof(Button):
                    _buttonManager.TryGetElement(coordinates, out var button);
                    element = (T)(object)button;
                    break;
                case nameof(Door):
                    _doorManager.TryGetElement(coordinates, out var door);
                    element = (T)(object)door;
                    break;
            }

            return element != null;
        }
        
        public List<Platform> GetNeighbors(HexCoordinates coordinates, int range = 1)
        {
            var neighbors = new List<Platform>();

            foreach (var platform in _platformManager.Elements)
            {
                if (platform.Coordinates == coordinates) continue;
                
                var diff = platform.Coordinates - coordinates;
                var distance = Mathf.Max(Mathf.Abs(diff.X), Mathf.Abs(diff.Y), Mathf.Abs(diff.Z));
                if (distance <= range) neighbors.Add(platform);
            }

            return neighbors;
        }

        public void RestartLevel()
        {
            ResetLevel();
            
            _gameStateMachine.ChangeState(GameStateType.StartLevel);
        }

        public bool IsWin()
        {
            return Player.Coordinates == _levelFinish.Coordinates;
        }
        
        public Bounds GetLevelBounds()
        {
            var cellSize = _gridService.CellSize;
            var bounds = new Bounds();

            foreach (var platform in _platformManager.Elements)
                bounds.Encapsulate(new Bounds(platform.transform.position, cellSize));

            return bounds;
        }

        private void AttachToPlatform(IEnumerable<IAttachedToPlatform> elements)
        {
            foreach (var button in elements)
            {
                if (_platformManager.TryGetElement(button.Coordinates, out var platform))
                    button.AttachToPlatform(platform);
            }
        }

        private void ResetLevel()
        {
            foreach (var platform in _platformManager.Elements)
                platform.ResetState();
            foreach (var fish in _fishManager.Elements)
                fish.ResetState();
            foreach (var button in _buttonManager.Elements)
                button.ResetState();
            foreach (var door in _doorManager.Elements)
                door.ResetState();
        }
    }
}