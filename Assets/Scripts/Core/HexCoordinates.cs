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

        public static HexCoordinates FromOffsetCoordinates(int x, int z) => new(x - z / 2, z);
        public static Vector3Int ToOffsetCoordinates(int x, int z) => new(x + z / 2, z);
        
        public override string ToString() => $"({X}, {Y}, {Z})";
        public string ToStringOnSeparateLines() => $"{X}\n{Y}\n{Z}";

        public static HexCoordinates operator -(HexCoordinates left, HexCoordinates right)
        {
            return new HexCoordinates(left.X - right.X, left.Z - right.Z);
        }
        
        public bool Equals(HexCoordinates other)
        {
            return _x == other._x && _z == other._z;
        }

        public override bool Equals(object obj)
        {
            return obj is HexCoordinates other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_x, _z);
        }

        public static bool operator ==(HexCoordinates left, HexCoordinates right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(HexCoordinates left, HexCoordinates right)
        {
            return !left.Equals(right);
        }
    }
}