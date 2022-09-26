using UnityEngine;

namespace HexGame.Gameplay
{
    public class Player : MonoBehaviour, IHexGridElement
    {
        [SerializeField] private HexGridElementMovement _movement;

        public HexGridElementMovement Movement => _movement;
        public HexCoordinates Coordinates { get; set; }
    }
}