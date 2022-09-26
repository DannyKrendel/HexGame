using UnityEngine;
using Zenject;

namespace HexGame.Gameplay
{
    public class GameplayService
    {
        private readonly PlatformManager _platformManager;
        private readonly FishManager _fishManager;
        private readonly ISpawnPoint<Player> _playerSpawnPoint;
        private readonly IFactory<Player> _playerFactory;
        private readonly GridService _gridService;

        public Player Player { get; private set; }

        public GameplayService(PlatformManager platformManager, FishManager fishManager, 
            ISpawnPoint<Player> playerSpawnPoint, IFactory<Player> playerFactory, GridService gridService)
        {
            _platformManager = platformManager;
            _fishManager = fishManager;
            _playerSpawnPoint = playerSpawnPoint;
            _playerFactory = playerFactory;
            _gridService = gridService;
        }

        public void SpawnPlayer()
        {
            if (Player == null)
                Player = _playerFactory.Create();
            
            _playerSpawnPoint.Spawn(Player);
        }

        public void RestartLevel()
        {
            foreach (var platform in _platformManager.Elements)
                platform.ResetState();
            
            _playerSpawnPoint.Spawn(Player);
        }

        public bool IsWin()
        {
            var activePlatformsCount = 0;
            foreach (var platform in _platformManager.Elements)
            {
                if (platform.gameObject.activeSelf)
                    activePlatformsCount++;
            }
            return activePlatformsCount == 1;
        }
        
        public Bounds GetLevelBounds()
        {
            var cellSize = _gridService.CellSize;
            var bounds = new Bounds();

            foreach (var platform in _platformManager.Elements)
                bounds.Encapsulate(new Bounds(platform.transform.position, cellSize));

            return bounds;
        }
    }
}