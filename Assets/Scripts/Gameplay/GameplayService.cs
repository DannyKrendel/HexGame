using Zenject;

namespace HexGame.Gameplay
{
    public class GameplayService
    {
        private readonly HexGrid _hexGrid;
        private readonly ISpawnPoint<Player> _playerSpawnPoint;
        private readonly IFactory<Player> _playerFactory;

        public Player Player { get; private set; }
        
        public GameplayService(HexGrid hexGrid, ISpawnPoint<Player> playerSpawnPoint, IFactory<Player> playerFactory)
        {
            _hexGrid = hexGrid;
            _playerSpawnPoint = playerSpawnPoint;
            _playerFactory = playerFactory;
        }

        public void SpawnPlayer()
        {
            if (Player == null)
                Player = _playerFactory.Create();
            
            _playerSpawnPoint.Spawn(Player);
        }

        public void RestartLevel()
        {
            foreach (var cell in _hexGrid.Cells)
                cell.ResetState();
            
            _playerSpawnPoint.Spawn(Player);
        }
    }
}