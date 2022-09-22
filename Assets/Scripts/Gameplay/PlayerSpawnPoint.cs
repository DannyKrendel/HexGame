using System;
using UnityEngine;
using Zenject;

namespace HexGame.Gameplay
{
    public class PlayerSpawnPoint : MonoBehaviour
    {
        [SerializeField] private HexCoordinates _coordinates;
        
        private IFactory<PlayerController> _playerFactory;

        [Inject]
        private void Construct(IFactory<PlayerController> playerFactory)
        {
            _playerFactory = playerFactory;
        }

        private void Start()
        {
            Spawn();
        }

        public void Spawn()
        {
            var player = _playerFactory.Create();
            player.MoveTo(_coordinates);
        }
    }
}