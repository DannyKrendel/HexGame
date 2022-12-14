using PolyternityStuff;
using UnityEngine;

namespace HexGame.Gameplay
{
    [ExecuteAlways]
    public abstract class HexGridElement : MonoBehaviour, IHexGridElement
    {
        [SerializeField, ReadOnly] private HexCoordinates _coordinates;

        public HexCoordinates Coordinates { get => _coordinates; set => _coordinates = value; }
    }
}