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

            // When a press is first detected, check if it's over the UI
            if (mouse.leftButton.wasPressedThisFrame || mouse.middleButton.wasPressedThisFrame)
            {
                wasDragStartedOverUI = IsPointerOverUI(screenPos);
            }

            // If we are not pressing anything, reset the state
            if (!leftPressed && !middlePressed)
            {
                wasDragStartedOverUI = false;
            }

            // Block zoom if pointer is currently over UI
            var scroll = mouse.scroll.ReadValue().y;
            if (!Mathf.Approximately(scroll, 0f) && !IsPointerOverUI(screenPos))
            {
                cameraService.Zoom(-scroll * mouseWheelZoomUnits);
            }

            // Block dragging if it was started over the UI
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
            
            // Check if both touches are pressed
            var firstPressed = first.press.isPressed;
            var secondPressed = second.press.isPressed;

            if (!firstPressed || !secondPressed)
            {
                wasTouchStartedOverUI = false;
                return;
            }

            var firstPosition = first.position.ReadValue();
            var secondPosition = second.position.ReadValue();

            // When touch first starts, check if it's over the UI
            if (first.press.wasPressedThisFrame || second.press.wasPressedThisFrame)
            {
                wasTouchStartedOverUI = IsPointerOverUI(firstPosition) || IsPointerOverUI(secondPosition);
            }

            if (wasTouchStartedOverUI)
            {
                return;
            }

            var firstDelta = first.delta.ReadValue();
            var secondDelta = second.delta.ReadValue();
            var panDelta = (firstDelta + secondDelta) * 0.5f;
            cameraService.Pan(panDelta, GetPanUnitsPerPixel());

            var previousFirst = firstPosition - firstDelta;
            var previousSecond = secondPosition - secondDelta;
            var currentDistance = Vector2.Distance(firstPosition, secondPosition);
            var previousDistance = Vector2.Distance(previousFirst, previousSecond);
            cameraService.Zoom(-(currentDistance - previousDistance) * touchPinchZoomUnits);
        }

        private bool IsPointerOverUI(Vector2 screenPosition)
        {
            // 1. Check EventSystem for uGUI / UI Toolkit / Canvas elements
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return true;
            }

            // 2. Check injected blockers (like IMGUI panels)
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
