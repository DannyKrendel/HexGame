using System.Collections.Generic;
using UnityEngine;

namespace HexGame.Gameplay
{
    public class PlatformManager : HexGridElementManager<Platform>
    {
        public List<Platform> GetNeighbors(HexCoordinates coordinates, int range = 1)
        {
            var neighbors = new List<Platform>();

            foreach (var (platformCoords, platform) in ElementDictionary)
            {
                if (platformCoords == coordinates) continue;
                
                var diff = platformCoords - coordinates;
                var distance = Mathf.Max(Mathf.Abs(diff.X), Mathf.Abs(diff.Y), Mathf.Abs(diff.Z));
                if (distance <= range) neighbors.Add(platform);
            }

            return neighbors;
        }
    }
}