using System;
using UnityEngine;
using Zenject;

namespace HexGame.Gameplay
{
    public class PlayerSpawnPoint : MonoBehaviour, ISpawnPoint<Player>
    {
        [SerializeField] private HexCoordinates _coordinates;

        public void Spawn(Player player)
        {
            player.Movement.Move(_coordinates, true);
        }
    }
}