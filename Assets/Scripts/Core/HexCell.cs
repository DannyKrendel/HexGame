using UnityEngine;

namespace HexGame
{
    public class HexCell : MonoBehaviour
    {
        [SerializeField] private HexCoordinates _coordinates;
        
        public const float OuterRadius = 10f;
        public const float InnerRadius = OuterRadius * 0.866025404f;
        
        public HexCoordinates Coordinates { get => _coordinates; set => _coordinates = value; }
    }
}