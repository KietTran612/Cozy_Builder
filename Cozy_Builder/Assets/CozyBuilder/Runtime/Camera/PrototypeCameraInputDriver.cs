using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using UnityCamera = UnityEngine.Camera;

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

        [Inject]
        public void Construct(CameraService cameraService)
        {
            this.cameraService = cameraService;
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

            var delta = mouse.delta.ReadValue();
            var altHeld = keyboard != null && (keyboard.leftAltKey.isPressed || keyboard.rightAltKey.isPressed);
            if (altHeld && mouse.leftButton.isPressed)
            {
                cameraService.Orbit(delta.x * orbitDegreesPerPixel, -delta.y * orbitDegreesPerPixel);
            }

            if (mouse.middleButton.isPressed)
            {
                cameraService.Pan(delta, GetPanUnitsPerPixel());
            }

            var scroll = mouse.scroll.ReadValue().y;
            if (!Mathf.Approximately(scroll, 0f))
            {
                cameraService.Zoom(-scroll * mouseWheelZoomUnits);
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
            if (!first.press.isPressed || !second.press.isPressed)
            {
                return;
            }

            var firstPosition = first.position.ReadValue();
            var secondPosition = second.position.ReadValue();
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

        private float GetPanUnitsPerPixel()
        {
            return cameraService.Distance * panUnitsPerPixelAtDistance;
        }
    }
}
