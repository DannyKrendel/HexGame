using System;
using UnityEngine;

namespace HexGame
{
    public class HexCell : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private HexCoordinates _coordinates;

        public HexCoordinates Coordinates { get => _coordinates; set => _coordinates = value; }
        public Vector2 BoundsSize 
        {
            get
            {
                var worldBoundsSize = _spriteRenderer.bounds.size;
                return new Vector2(worldBoundsSize.x, worldBoundsSize.z) / transform.localScale.x;
            }
        }

        public void SetLocalScale(float localScale)
        {
            transform.localScale = Vector3.one * localScale;
        }
    }
}