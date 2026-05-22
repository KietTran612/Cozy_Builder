using UnityEngine;

namespace CozyBuilder.Camera
{
    public sealed class CameraService
    {
        private Vector3 pivot;
        private float distance;
        private float yaw;
        private float pitch;
        private float minDistance;
        private float maxDistance;
        private float minPitch;
        private float maxPitch;

        public Vector3 Pivot => pivot;
        public float Distance => distance;
        public float Yaw => yaw;
        public float Pitch => pitch;

        public void Reset(
            Vector3 pivot,
            float distance,
            float yaw,
            float pitch,
            float minDistance,
            float maxDistance,
            float minPitch,
            float maxPitch)
        {
            this.minDistance = minDistance;
            this.maxDistance = maxDistance;
            this.minPitch = minPitch;
            this.maxPitch = maxPitch;
            this.pivot = pivot;
            this.distance = Mathf.Clamp(distance, minDistance, maxDistance);
            this.yaw = yaw;
            this.pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        }

        public void Orbit(float yawDelta, float pitchDelta)
        {
            yaw += yawDelta;
            pitch = Mathf.Clamp(pitch + pitchDelta, minPitch, maxPitch);
        }

        public void Pan(Vector2 screenDelta, float unitsPerPixel)
        {
            var yawRotation = Quaternion.Euler(0f, yaw, 0f);
            var right = yawRotation * Vector3.right;
            var forward = yawRotation * Vector3.forward;
            pivot -= (right * screenDelta.x + forward * screenDelta.y) * unitsPerPixel;
        }

        public void Zoom(float distanceDelta)
        {
            distance = Mathf.Clamp(distance + distanceDelta, minDistance, maxDistance);
        }

        public void ApplyTo(Transform cameraTransform)
        {
            var rotation = Quaternion.Euler(pitch, yaw, 0f);
            cameraTransform.SetPositionAndRotation(pivot + rotation * new Vector3(0f, 0f, -distance), rotation);
        }
    }
}
