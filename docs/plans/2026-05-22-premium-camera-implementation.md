# Premium Camera Interactions & Mobile Touch Gestures Implementation Plan

> **For Antigravity:** REQUIRED WORKFLOW: Use `.agent/workflows/execute-plan.md` to execute this plan in single-flow mode.

**Goal:** Triển khai làm mượt Camera (Smooth Damping), cử chỉ cảm ứng Mobile đa chạm chuyên nghiệp (1-ngón xoay, 2-ngón pan/pinch), phân biệt Tap vs Drag và Nhấp đúp để lấy nét (Double-Tap Focus).

**Architecture:** Nâng cấp `CameraService.cs` quản lý trạng thái mục tiêu (Target) và thực tế (Current) bằng `SmoothDamp`. Cập nhật `PrototypeCameraInputDriver.cs` để tính toán cử chỉ cảm ứng di động tối ưu không tạo GC, và cập nhật `PrototypePlacementInputDriver.cs` xử lý máy trạng thái Tap/Double-Tap để tránh đặt nhầm block.

**Tech Stack:** Unity 6000.3.11f1, URP, Unity Input System, VContainer.

---

## Bảng Theo Dõi Tiến Độ (Task Tracker)

Chúng ta sẽ cập nhật tiến độ trực tiếp vào `docs/plans/task.md` theo danh sách nhiệm vụ dưới đây:

### Task 1: Nâng cấp `CameraService.cs` ( SmoothDamp & Bounding)
**Files:**
- Modify: `c:\1.SOURCE\Unity\Source\Cozy_Builder\Cozy_Builder\Assets\CozyBuilder\Runtime\Camera\CameraService.cs`

**Step 1: Cập nhật cấu trúc lớp, biến lưu trữ trạng thái Target, Current và Vận tốc SmoothDamp**
Thay đổi toàn bộ trường dữ liệu và nâng cấp các phương thức để hỗ trợ cộng dồn Target:
```csharp
using UnityEngine;

namespace CozyBuilder.Camera
{
    public sealed class CameraService
    {
        // Trạng thái hiện tại
        private Vector3 currentPivot;
        private float currentDistance;
        private float currentYaw;
        private float currentPitch;

        // Trạng thái mục tiêu
        private Vector3 targetPivot;
        private float targetDistance;
        private float targetYaw;
        private float targetPitch;

        // Cấu hình giới hạn biên
        private float minDistance;
        private float maxDistance;
        private float minPitch;
        private float maxPitch;
        private float maxPivotRadius = 15f; // Giới hạn biên 15m

        // Vận tốc SmoothDamp
        private Vector3 pivotVelocity;
        private float distanceVelocity;
        private float yawVelocity;
        private float pitchVelocity;

        // Thời gian làm mượt
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

            // Thiết lập giá trị ban đầu cho cả hai trạng thái
            this.targetPivot = pivot;
            this.targetDistance = Mathf.Clamp(distance, minDistance, maxDistance);
            this.targetYaw = yaw;
            this.targetPitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            this.currentPivot = targetPivot;
            this.currentDistance = targetDistance;
            this.currentYaw = targetYaw;
            this.currentPitch = targetPitch;

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
            
            // Giới hạn biên Pivot quanh tâm (0,0,0)
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
            // Giới hạn biên điểm lấy nét mới
            if (position.magnitude > maxPivotRadius)
            {
                position = position.normalized * maxPivotRadius;
            }
            targetPivot = position;
        }

        public void ApplyTo(Transform cameraTransform)
        {
            // Nội suy mượt mà
            currentPivot = Vector3.SmoothDamp(currentPivot, targetPivot, ref pivotVelocity, pivotSmoothTime);
            currentDistance = Mathf.SmoothDamp(currentDistance, targetDistance, ref distanceVelocity, zoomSmoothTime);
            currentYaw = Mathf.SmoothDampAngle(currentYaw, targetYaw, ref yawVelocity, orbitSmoothTime);
            currentPitch = Mathf.SmoothDamp(currentPitch, targetPitch, ref pitchVelocity, orbitSmoothTime);

            var rotation = Quaternion.Euler(currentPitch, currentYaw, 0f);
            cameraTransform.SetPositionAndRotation(currentPivot + rotation * new Vector3(0f, 0f, -currentDistance), rotation);
        }
    }
}
```

**Step 2: Kiểm tra biên dịch**
Đảm bảo Assembly `CozyBuilder.Camera` biên dịch thành công.

---

### Task 2: Cập nhật `PrototypeCameraInputDriver.cs` (Cử chỉ cảm ứng đa điểm & Zero GC)
**Files:**
- Modify: `c:\1.SOURCE\Unity\Source\Cozy_Builder\Cozy_Builder\Assets\CozyBuilder\Runtime\Camera\PrototypeCameraInputDriver.cs`

**Step 1: Viết lại phương thức `HandleTouch()` để bắt chính xác cử chỉ 1 ngón hoặc 2 ngón và triệt tiêu GC Allocations**
```csharp
        private void HandleTouch()
        {
            var touchscreen = Touchscreen.current;
            if (touchscreen == null)
            {
                return;
            }

            var first = touchscreen.touches[0];
            var second = touchscreen.touches[1];

            bool firstPressed = first.press.isPressed;
            bool secondPressed = second.press.isPressed;

            // 1. Quản lý trạng thái khởi đầu vuốt đè lên UI
            if (first.press.wasPressedThisFrame)
            {
                wasTouchStartedOverUI = IsPointerOverUI(first.position.ReadValue());
            }
            if (second.press.wasPressedThisFrame && !wasTouchStartedOverUI)
            {
                wasTouchStartedOverUI = IsPointerOverUI(second.position.ReadValue());
            }

            if (!firstPressed)
            {
                wasTouchStartedOverUI = false;
                return;
            }

            // Nếu chạm xuất phát từ vùng UI, bỏ qua toàn bộ cử chỉ vuốt
            if (wasTouchStartedOverUI)
            {
                return;
            }

            if (firstPressed && !secondPressed)
            {
                // Cử chỉ 1 NGÓN TAY: Xoay camera (Orbit)
                // Lọc delta để tránh giật hình khi vừa chạm
                if (!first.press.wasPressedThisFrame)
                {
                    var delta = first.delta.ReadValue();
                    cameraService.Orbit(delta.x * orbitDegreesPerPixel, -delta.y * orbitDegreesPerPixel);
                }
            }
            else if (firstPressed && secondPressed)
            {
                // Cử chỉ 2 NGÓN TAY: Pan di chuyển và Zoom nhúm (Pinch) kết hợp
                Vector2 firstPos = first.position.ReadValue();
                Vector2 secondPos = second.position.ReadValue();
                Vector2 firstDelta = first.delta.ReadValue();
                Vector2 secondDelta = second.delta.ReadValue();

                // Pan: Tính trung bình chuyển dịch 2 ngón
                Vector2 panDelta = (firstDelta + secondDelta) * 0.5f;
                cameraService.Pan(panDelta, GetPanUnitsPerPixel());

                // Zoom Pinch: Khoảng cách dịch chuyển
                Vector2 prevFirst = firstPos - firstDelta;
                Vector2 prevSecond = secondPos - secondDelta;
                float currentDist = Vector2.Distance(firstPos, secondPos);
                float prevDist = Vector2.Distance(prevFirst, prevSecond);
                
                // Thay đổi khoảng cách zoom theo tỉ lệ khoảng cách camera hiện tại (responsiveness)
                float zoomDelta = (currentDist - prevDist) * touchPinchZoomUnits;
                cameraService.Zoom(-zoomDelta);
            }
        }
```

---

### Task 3: Nâng cấp `PrototypePlacementInputDriver.cs` (Nhận diện Tap & Double-Tap Focus)
**Files:**
- Modify: `c:\1.SOURCE\Unity\Source\Cozy_Builder\Cozy_Builder\Assets\CozyBuilder\Runtime\Town\Placement\PrototypePlacementInputDriver.cs`

**Step 1: Định nghĩa cấu trúc máy trạng thái cảm ứng trong driver đặt block**
Thêm các tham số cấu hình và nâng cấp hàm `Update()` để lọc Tap & hỗ trợ Double-Tap lấy nét:
```csharp
        [Header("Tap Constraints")]
        [SerializeField] private float tapDurationThreshold = 0.25f;
        [SerializeField] private float tapMoveThreshold = 15f;
        [SerializeField] private float doubleTapInterval = 0.25f;

        private Vector2 touchStartPos;
        private float touchStartTime;
        private bool isPossibleTap;
        
        // Trực quan hóa biến chờ xác thực Tap
        private float lastTapReleaseTime = -999f;
        private Vector2 lastTapReleasePos;
        private bool isPendingSingleTap = false;
        private float pendingSingleTapExecuteTime = 0f;

        [Inject]
        private CameraService cameraService; // Inject CameraService để gọi FocusOn

        private void Update()
        {
            var mouse = Mouse.current;
            if (mouse != null)
            {
                HandleMouseInput(mouse);
            }

            var touchscreen = Touchscreen.current;
            if (touchscreen != null)
            {
                HandleTouchInput(touchscreen);
            }

            // Xử lý xác thực Single-Tap trễ để nhường đường cho Double-Tap
            if (isPendingSingleTap && Time.time >= pendingSingleTapExecuteTime)
            {
                ExecuteSingleTap(lastTapReleasePos);
            }
        }

        private void HandleTouchInput(Touchscreen touchscreen)
        {
            var primaryTouch = touchscreen.primaryTouch;
            var screenPos = primaryTouch.position.ReadValue();

            if (primaryTouch.press.wasPressedThisFrame)
            {
                isPossibleTap = !IsPointerOverUI(screenPos);
                touchStartPos = screenPos;
                touchStartTime = Time.time;
            }

            if (primaryTouch.press.isPressed && isPossibleTap)
            {
                if (Vector2.Distance(screenPos, touchStartPos) > tapMoveThreshold)
                {
                    isPossibleTap = false; // Ngón tay đã kéo di chuyển để xoay camera
                }
            }

            if (primaryTouch.press.wasReleasedThisFrame)
            {
                if (isPossibleTap && (Time.time - touchStartTime) <= tapDurationThreshold && !IsPointerOverUI(screenPos))
                {
                    float timeSinceLastTap = Time.time - lastTapReleaseTime;
                    if (timeSinceLastTap <= doubleTapInterval)
                    {
                        // Nhấp Đúp! Hủy lệnh đặt block đang chờ và kích hoạt Focus
                        isPendingSingleTap = false;
                        ExecuteDoubleTap(screenPos);
                    }
                    else
                    {
                        // Đưa vào hàng chờ xác thực trễ
                        lastTapReleaseTime = Time.time;
                        lastTapReleasePos = screenPos;
                        isPendingSingleTap = true;
                        pendingSingleTapExecuteTime = Time.time + 0.15f; // Trễ 0.15s chờ double-tap
                    }
                }
                isPossibleTap = false;
            }
        }

        private void ExecuteSingleTap(Vector2 screenPosition)
        {
            isPendingSingleTap = false;
            TryApplyPointer(screenPosition, placementState != null && placementState.IsDeleteMode);
        }

        private void ExecuteDoubleTap(Vector2 screenPosition)
        {
            if (cameraService == null || inputCamera == null || townGridView == null) return;

            var ray = inputCamera.ScreenPointToRay(screenPosition);
            if (gridPlane.Raycast(ray, out var distance))
            {
                var worldPosition = ray.GetPoint(distance);
                if (townGridView.TryGetCoordFromWorld(worldPosition, out var coord))
                {
                    // Lấy nét camera vào trung điểm ô đất được nhấp đúp
                    Vector3 cellWorldPos = townGridView.transform.position + new Vector3(coord.X * 2.0f, 0f, coord.Y * 2.0f); // Spacing 2m
                    cameraService.FocusOn(cellWorldPos);
                    debugState?.Select(coord);
                }
            }
        }

        private void HandleMouseInput(Mouse mouse)
        {
            var screenPos = mouse.position.ReadValue();
            if (IsPointerOverUI(screenPos))
            {
                return;
            }

            if (mouse.leftButton.wasPressedThisFrame)
            {
                var keyboard = Keyboard.current;
                var cameraModifierHeld = keyboard != null && (keyboard.leftAltKey.isPressed || keyboard.rightAltKey.isPressed);
                if (!cameraModifierHeld)
                {
                    TryApplyPointer(screenPos, placementState != null && placementState.IsDeleteMode);
                }
            }

            if (mouse.rightButton.wasPressedThisFrame)
            {
                TryApplyPointer(screenPos, true);
            }
        }
```

---

### Task 4: Cấu hình DI & Xác minh kiểm thử (Compilation & Manual Verification)
**Files:**
- Modify: `c:\1.SOURCE\Unity\Source\Cozy_Builder\Cozy_Builder\Assets\CozyBuilder\Runtime\Bootstrap\GameLifetimeScope.cs`

**Step 1: Đảm bảo CameraService được inject vào `PrototypePlacementInputDriver`**
Do `CameraService` đã được đăng ký dạng Singleton trong `GameLifetimeScope.cs`, chúng ta chỉ cần đảm bảo VContainer tiêm đúng tham chiếu này cho Input Driver.

**Step 2: Chạy kiểm thử biên dịch dự án**
Mở Unity và biên dịch mã nguồn, xác nhận 0 lỗi biên dịch.

**Step 3: Chạy `graphify update .`**
Cập nhật lại cấu trúc đồ thị mã nguồn sau khi sửa đổi.
