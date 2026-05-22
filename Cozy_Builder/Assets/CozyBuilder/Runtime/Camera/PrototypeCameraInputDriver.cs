using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using VContainer;
using UnityCamera = UnityEngine.Camera;
using System.Collections.Generic;

namespace CozyBuilder.Camera
{
    public sealed class PrototypeCameraInputDriver : MonoBehaviour
    {
        [SerializeField] private UnityCamera targetCamera;
        [SerializeField] private Vector3 defaultPivot = Vector3.zero;
        [SerializeField] private float defaultDistance = 13f;
        [SerializeField] private float defaultYaw;
        [SerializeField] private float defaultPitch = 32f;
        [SerializeField] private float minDistance = 4f;
        [SerializeField] private float maxDistance = 26f;
        [SerializeField] private float minPitch = 20f;
        [SerializeField] private float maxPitch = 70f;
        [SerializeField] private float orbitDegreesPerPixel = 0.25f;
        [SerializeField] private float panUnitsPerPixelAtDistance = 0.0016f;
        [SerializeField] private float mouseWheelZoomUnits = 0.02f;
        [SerializeField] private float touchPinchZoomUnits = 0.018f;

        private CameraService cameraService;
        private IReadOnlyList<ICameraInputBlocker> inputBlockers;

        private bool wasDragStartedOverUI = false;
        private bool wasTouchStartedOverUI = false;

        [Inject]
        public void Construct(
            CameraService cameraService,
            IReadOnlyList<ICameraInputBlocker> inputBlockers = null)
        {
            this.cameraService = cameraService;
            this.inputBlockers = inputBlockers;
        }

        private void Start()
        {
            if (targetCamera == null)
            {
                targetCamera = GetComponent<UnityCamera>();
            }

            ResetCamera();
        }

        private void LateUpdate()
        {
            if (cameraService == null || targetCamera == null)
            {
                return;
            }

            HandleMouse();
            HandleTouch();
            cameraService.ApplyTo(targetCamera.transform);
        }

        public void ResetCamera()
        {
            if (cameraService == null)
            {
                return;
            }

            cameraService.Reset(
                defaultPivot,
                defaultDistance,
                defaultYaw,
                defaultPitch,
                minDistance,
                maxDistance,
                minPitch,
                maxPitch);
        }

        private void HandleMouse()
        {
            var mouse = Mouse.current;
            if (mouse == null)
            {
                return;
            }

            var keyboard = Keyboard.current;
            if (keyboard != null && keyboard.rKey.wasPressedThisFrame)
            {
                ResetCamera();
            }

            var screenPos = mouse.position.ReadValue();
            var leftPressed = mouse.leftButton.isPressed;
            var middlePressed = mouse.middleButton.isPressed;

            // Kiểm tra click UI khi bắt đầu press chuột
            if (mouse.leftButton.wasPressedThisFrame || mouse.middleButton.wasPressedThisFrame)
            {
                wasDragStartedOverUI = IsPointerOverUI(screenPos);
            }

            // Reset trạng thái chặn kéo khi thả chuột ra
            if (!leftPressed && !middlePressed)
            {
                wasDragStartedOverUI = false;
            }

            // Chặn Zoom bằng chuột nếu trỏ chuột đang nằm trên UI
            var scroll = mouse.scroll.ReadValue().y;
            if (!Mathf.Approximately(scroll, 0f) && !IsPointerOverUI(screenPos))
            {
                cameraService.Zoom(-scroll * mouseWheelZoomUnits);
            }

            // Bỏ qua kéo camera nếu click xuất phát từ UI
            if (wasDragStartedOverUI)
            {
                return;
            }

            var delta = mouse.delta.ReadValue();
            var altHeld = keyboard != null && (keyboard.leftAltKey.isPressed || keyboard.rightAltKey.isPressed);
            if (altHeld && leftPressed)
            {
                cameraService.Orbit(delta.x * orbitDegreesPerPixel, -delta.y * orbitDegreesPerPixel);
            }

            if (middlePressed)
            {
                cameraService.Pan(delta, GetPanUnitsPerPixel());
            }
        }

        private void HandleTouch()
        {
            var touchscreen = Touchscreen.current;
            if (touchscreen == null)
            {
                return;
            }

            var first = touchscreen.touches[0];
            var second = touchscreen.touches[1];
            
            var firstPressed = first.press.isPressed;
            var secondPressed = second.press.isPressed;

            // 1. Quản lý trạng thái chặn cảm ứng bắt đầu từ UI (IMGUI hoặc uGUI)
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

            // Bỏ qua toàn bộ cử chỉ vuốt nếu chạm xuất phát từ vùng UI
            if (wasTouchStartedOverUI)
            {
                return;
            }

            if (firstPressed && !secondPressed)
            {
                // CỬ CHỈ 1 NGÓN: Xoay camera (Orbit) xung quanh Pivot
                // Bỏ qua delta khung hình đầu tiên để tránh giật camera khi vừa chạm
                if (!first.press.wasPressedThisFrame)
                {
                    var delta = first.delta.ReadValue();
                    cameraService.Orbit(delta.x * orbitDegreesPerPixel, -delta.y * orbitDegreesPerPixel);
                }
            }
            else if (firstPressed && secondPressed)
            {
                // CỬ CHỈ 2 NGÓN: Pinch Zoom kết hợp Pan di chuyển hòn đảo
                Vector2 firstPosition = first.position.ReadValue();
                Vector2 secondPosition = second.position.ReadValue();
                Vector2 firstDelta = first.delta.ReadValue();
                Vector2 secondDelta = second.delta.ReadValue();

                // 1. Pan: Dịch chuyển trung điểm 2 ngón tay
                Vector2 panDelta = (firstDelta + secondDelta) * 0.5f;
                cameraService.Pan(panDelta, GetPanUnitsPerPixel());

                // 2. Pinch Zoom: Tính tỉ lệ thay đổi khoảng cách
                Vector2 previousFirst = firstPosition - firstDelta;
                Vector2 previousSecond = secondPosition - secondDelta;
                float currentDistance = Vector2.Distance(firstPosition, secondPosition);
                float previousDistance = Vector2.Distance(previousFirst, previousSecond);

                // Độ giãn nở tỉ lệ theo khoảng cách Zoom thực tế
                float zoomDelta = (currentDistance - previousDistance) * touchPinchZoomUnits;
                cameraService.Zoom(-zoomDelta);
            }
        }

        private bool IsPointerOverUI(Vector2 screenPosition)
        {
            // 1. Kiểm tra EventSystem của Unity (uGUI / UI Toolkit Canvas)
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return true;
            }

            // 2. Kiểm tra bộ chặn động đã đăng ký (IMGUI panels)
            if (inputBlockers != null)
            {
                int count = inputBlockers.Count;
                for (int i = 0; i < count; i++)
                {
                    if (inputBlockers[i].IsPointerOverUI(screenPosition))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private float GetPanUnitsPerPixel()
        {
            return cameraService.Distance * panUnitsPerPixelAtDistance;
        }
    }
}
