using UnityEngine;

namespace HexGame
{
    public class HexCell : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private HexCoordinates _coordinates;

        public HexCoordinates Coordinates { get => _coordinates; set => _coordinates = value; }
        public SpriteRenderer SpriteRenderer => _spriteRenderer;

        public void SetLocalScale(float localScale)
        {
            transform.localScale = Vector3.one * localScale;
        }
    }
}