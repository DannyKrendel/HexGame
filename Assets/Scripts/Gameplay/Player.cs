using UnityEngine;

namespace HexGame.Gameplay
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerMovement _movement;

        public PlayerMovement Movement => _movement;
    }
}