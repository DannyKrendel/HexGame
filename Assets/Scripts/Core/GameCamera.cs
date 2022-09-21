using UnityEngine;

namespace HexGame
{
    [RequireComponent(typeof(Camera))]
    public class GameCamera : MonoBehaviour
    {
        public Camera Camera { get; private set; }

        private void Awake()
        {
            Camera = GetComponent<Camera>();
        }

        public void FitCameraToBounds(Bounds bounds)
        {
            transform.position = bounds.center - transform.forward;

            var screenRatio = Screen.width / (float)Screen.height;
            var targetRatio = bounds.size.x / bounds.size.y;
 
            if (screenRatio >= targetRatio)
            {
                Camera.orthographicSize = bounds.size.y / 2;
            }
            else
            {
                var differenceInSize = targetRatio / screenRatio;
                Camera.orthographicSize = bounds.size.y / 2 * differenceInSize;
            }
        }
    }
}