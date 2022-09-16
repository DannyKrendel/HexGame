using System;
using UnityEngine;

namespace HexGame
{
    [Serializable]
    public struct HexCoordinates
    {
        [SerializeField] private int _x;
        [SerializeField] private int _z;

        public int X => _x;
        public int Z => _z;
        public int Y => -X - Z;

        public HexCoordinates(int x, int z)
        {
            _x = x;
            _z = z;
        }
        
        public override string ToString() => $"({X}, {Y}, {Z})";
        public string ToStringOnSeparateLines() => $"{X}\n{Y}\n{Z}";
        
        public static HexCoordinates FromOffsetCoordinates(int x, int z) => new(x - z / 2, z);
    }
}