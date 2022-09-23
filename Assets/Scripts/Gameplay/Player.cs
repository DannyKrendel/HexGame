using UnityEngine;

namespace HexGame.Gameplay
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerController _playerController;

        public PlayerController PlayerController => _playerController;
    }
}