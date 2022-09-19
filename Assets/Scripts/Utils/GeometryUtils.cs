using UnityEngine;

namespace HexGame.Utils
{
    public static class GeometryUtils
    {
        public static bool IsPointInsidePolygon(Vector3 point, Vector3[] corners)
        {
            var polygonLength = corners.Length;
            var i = 0;
            var isInside = false;
            // x, y for tested point.
            float pointX = point.x, pointY = point.z;
            // start / end point for the current polygon segment.
            float startX, startY, endX, endY;
            var endPoint = corners[polygonLength - 1];
            endX = endPoint.x;
            endY = endPoint.z;
            while (i < polygonLength)
            {
                startX = endX;
                startY = endY;
                endPoint = corners[i++];
                endX = endPoint.x;
                endY = endPoint.z;
                isInside ^= endY > pointY ^ startY > pointY /* ? pointY inside [startY;endY] segment ? */
                          && /* if so, test if it is under the segment */
                          (pointX - endX) < (pointY - endY) * (startX - endX) / (startY - endY);
            }
                
            return isInside;
        }
    }
}