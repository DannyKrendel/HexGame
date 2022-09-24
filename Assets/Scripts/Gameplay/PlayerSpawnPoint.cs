using System;
using UnityEngine;
using Zenject;

namespace HexGame.Gameplay
{
    public class PlayerSpawnPoint : MonoBehaviour, ISpawnPoint<Player>
    {
        [SerializeField] private HexCoordinates _coordinates;
        
        public event Action<Player> Spawned;
        
        private IFactory<Player> _playerFactory;

        [Inject]
        private void Construct(IFactory<Player> playerFactory)
        {
            _playerFactory = playerFactory;
        }

        public Player Spawn()
        {
            var player = _playerFactory.Create();
            player.Movement.Move(_coordinates, true);
            Spawned?.Invoke(player);
            return player;
        }
    }
}