using UnityEngine;

namespace HexGame.Utils
{
    public static class HexGridUtils
    {
        public static Vector3 GetCellPosition(int x, int z, float innerRadius, float outerRadius, float margin)
        {
            var position = Vector3.zero;
            var xMult = x + z * 0.5f - z / 2;
            position.x = xMult * (innerRadius * 2f) + margin * xMult;
            position.z = z * (outerRadius * 1.5f) + margin * Mathf.Cos(30 * Mathf.Deg2Rad) * z;
            return position;
        }

        public static Vector3[] GetCellPoints(Vector3 position, float innerRadius, float outerRadius)
        {
            return new[] 
            {
                position + new Vector3(0f, 0f, outerRadius),
                position + new Vector3(innerRadius, 0f, 0.5f * outerRadius),
                position + new Vector3(innerRadius, 0f, -0.5f * outerRadius),
                position + new Vector3(0f, 0f, -outerRadius),
                position + new Vector3(-innerRadius, 0f, -0.5f * outerRadius),
                position + new Vector3(-innerRadius, 0f, 0.5f * outerRadius)
            };
        }
    }
}