using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace HexGame.Gameplay
{
    public class GameplayService
    {
        private readonly ISpawnPoint<Player> _playerSpawnPoint;
        private readonly LevelFinish _levelFinish;
        private readonly IFactory<Player> _playerFactory;
        private readonly GridService _gridService;

        public Player Player { get; private set; }
        public HexGridElementManager<Platform> PlatformManager { get; }
        public HexGridElementManager<Fish> FishManager { get; }
        public int AllFishCount => FishManager.Elements.Count;
        public int ConsumedFishCount => FishManager.Elements.Count(x => x.IsConsumed);

        public GameplayService(HexGridElementManager<Platform> platformManager, HexGridElementManager<Fish> fishManager, 
            ISpawnPoint<Player> playerSpawnPoint, LevelFinish levelFinish, 
            IFactory<Player> playerFactory, GridService gridService)
        {
            PlatformManager = platformManager;
            FishManager = fishManager;
            _playerSpawnPoint = playerSpawnPoint;
            _levelFinish = levelFinish;
            _playerFactory = playerFactory;
            _gridService = gridService;
        }

        public void SpawnPlayer()
        {
            if (Player == null)
                Player = _playerFactory.Create();
            
            _playerSpawnPoint.Spawn(Player);
        }
        
        public List<Platform> GetNeighbors(HexCoordinates coordinates, int range = 1)
        {
            var neighbors = new List<Platform>();

            foreach (var platform in PlatformManager.Elements)
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
            foreach (var platform in PlatformManager.Elements)
                platform.ResetState();
            foreach (var fish in FishManager.Elements)
                fish.ResetState();

            _playerSpawnPoint.Spawn(Player);
        }

        public bool IsWin()
        {
            return Player.Coordinates == _levelFinish.Coordinates;
        }
        
        public Bounds GetLevelBounds()
        {
            var cellSize = _gridService.CellSize;
            var bounds = new Bounds();

            foreach (var platform in PlatformManager.Elements)
                bounds.Encapsulate(new Bounds(platform.transform.position, cellSize));

            return bounds;
        }
    }
}