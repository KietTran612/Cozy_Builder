using UnityEngine;

namespace CozyBuilder.Camera
{
    public sealed class CameraService
    {
        // Trạng thái hiện tại thực tế trên Scene
        private Vector3 currentPivot;
        private float currentDistance;
        private float currentYaw;
        private float currentPitch;

        // Trạng thái mục tiêu mà đầu vào hướng tới
        private Vector3 targetPivot;
        private float targetDistance;
        private float targetYaw;
        private float targetPitch;

        // Cấu hình giới hạn biên
        private float minDistance;
        private float maxDistance;
        private float minPitch;
        private float maxPitch;
        private float maxPivotRadius = 15f; // Giới hạn bán kính biên 15m

        // Vận tốc dùng cho SmoothDamp (Bắt buộc lưu trữ cố định)
        private Vector3 pivotVelocity;
        private float distanceVelocity;
        private float yawVelocity;
        private float pitchVelocity;

        // Thời gian làm mượt (damping times)
        private float pivotSmoothTime = 0.15f;
        private float orbitSmoothTime = 0.12f;
        private float zoomSmoothTime = 0.15f;

        public Vector3 Pivot => currentPivot;
        public float Distance => currentDistance;
        public float Yaw => currentYaw;
        public float Pitch => currentPitch;
        
        public Vector3 TargetPivot => targetPivot;

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

            // Đồng bộ cả Target và Current về vị trí Reset
            this.targetPivot = pivot;
            this.targetDistance = Mathf.Clamp(distance, minDistance, maxDistance);
            this.targetYaw = yaw;
            this.targetPitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            this.currentPivot = targetPivot;
            this.currentDistance = targetDistance;
            this.currentYaw = targetYaw;
            this.currentPitch = targetPitch;

            // Reset vận tốc trượt
            this.pivotVelocity = Vector3.zero;
            this.distanceVelocity = 0f;
            this.yawVelocity = 0f;
            this.pitchVelocity = 0f;
        }

        public void Orbit(float yawDelta, float pitchDelta)
        {
            targetYaw += yawDelta;
            targetPitch = Mathf.Clamp(targetPitch + pitchDelta, minPitch, maxPitch);
        }

        public void Pan(Vector2 screenDelta, float unitsPerPixel)
        {
            var yawRotation = Quaternion.Euler(0f, targetYaw, 0f);
            var right = yawRotation * Vector3.right;
            var forward = yawRotation * Vector3.forward;
            
            Vector3 nextPivot = targetPivot - (right * screenDelta.x + forward * screenDelta.y) * unitsPerPixel;
            
            // Giới hạn biên Pivot không đi ra ngoài bán kính hòn đảo
            if (nextPivot.magnitude > maxPivotRadius)
            {
                nextPivot = nextPivot.normalized * maxPivotRadius;
            }
            targetPivot = nextPivot;
        }

        public void Zoom(float distanceDelta)
        {
            targetDistance = Mathf.Clamp(targetDistance + distanceDelta, minDistance, maxDistance);
        }

        public void FocusOn(Vector3 position)
        {
            // Giới hạn biên cho điểm lấy nét
            if (position.magnitude > maxPivotRadius)
            {
                position = position.normalized * maxPivotRadius;
            }
            targetPivot = position;
        }

        public void ApplyTo(Transform cameraTransform)
        {
            // Nội suy làm mượt (damping/inertia) từ Current sang Target
            currentPivot = Vector3.SmoothDamp(currentPivot, targetPivot, ref pivotVelocity, pivotSmoothTime);
            currentDistance = Mathf.SmoothDamp(currentDistance, targetDistance, ref distanceVelocity, zoomSmoothTime);
            currentYaw = Mathf.SmoothDampAngle(currentYaw, targetYaw, ref yawVelocity, orbitSmoothTime);
            currentPitch = Mathf.SmoothDamp(currentPitch, targetPitch, ref pitchVelocity, orbitSmoothTime);

            var rotation = Quaternion.Euler(currentPitch, currentYaw, 0f);
            cameraTransform.SetPositionAndRotation(currentPivot + rotation * new Vector3(0f, 0f, -currentDistance), rotation);
        }
    }
}
