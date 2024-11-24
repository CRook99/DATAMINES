using UnityEngine.Serialization;

namespace System
{
    using UnityEngine;

    public class Parallax : MonoBehaviour
    {
        public Transform CameraTransform;
        public Transform PlayerTransform;
        public Vector2 ParallaxEffectMultiplier;

        private float _lastCameraY;
        private float _lastPlayerX;

        private void Start()
        {
            _lastCameraY = CameraTransform.position.y;
        }

        private void LateUpdate()
        {
            float xDelta = PlayerTransform.position.x - _lastPlayerX;
            float yDelta = CameraTransform.position.y - _lastCameraY;
            
            transform.position += new Vector3(xDelta * ParallaxEffectMultiplier.x,
                yDelta * ParallaxEffectMultiplier.y,
                0);

            _lastPlayerX = PlayerTransform.position.x;
            _lastCameraY = CameraTransform.position.y;
        }
    }
}