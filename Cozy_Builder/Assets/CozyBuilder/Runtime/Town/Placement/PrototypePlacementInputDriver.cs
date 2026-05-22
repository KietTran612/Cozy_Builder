using CozyBuilder.Town.Debugging;
using CozyBuilder.Town.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using VContainer;
using UnityCamera = UnityEngine.Camera;

namespace CozyBuilder.Town.Placement
{
    public sealed class PrototypePlacementInputDriver : MonoBehaviour
    {
        [SerializeField] private UnityCamera inputCamera;

        private PlacementService placementService;
        private PrototypePlacementState placementState;
        private PrototypeTownDebugState debugState;
        private TownGridView townGridView;
        private PrototypePlacementControlsView controlsView;
        private PrototypeTownDebugView debugView;
        private Plane gridPlane;

        [Inject]
        public void Construct(
            PlacementService placementService,
            PrototypePlacementState placementState,
            PrototypeTownDebugState debugState,
            TownGridView townGridView,
            PrototypePlacementControlsView controlsView,
            PrototypeTownDebugView debugView)
        {
            this.placementService = placementService;
            this.placementState = placementState;
            this.debugState = debugState;
            this.townGridView = townGridView;
            this.controlsView = controlsView;
            this.debugView = debugView;
        }

        private void Start()
        {
            if (inputCamera == null)
            {
                inputCamera = UnityCamera.main;
            }

            gridPlane = new Plane(Vector3.up, transform.position);
        }

        private void Update()
        {
            var mouse = Mouse.current;
            if (mouse != null)
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

            var touchscreen = Touchscreen.current;
            if (touchscreen != null)
            {
                var screenPos = touchscreen.primaryTouch.position.ReadValue();
                if (IsPointerOverUI(screenPos))
                {
                    return;
                }

                if (touchscreen.primaryTouch.press.wasPressedThisFrame)
                {
                    TryApplyPointer(screenPos, placementState != null && placementState.IsDeleteMode);
                }
            }
        }

        private bool IsPointerOverUI(Vector2 screenPosition)
        {
            // 1. Check EventSystem for uGUI / UI Toolkit / Canvas elements
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return true;
            }

            // 2. Check IMGUI panels
            Vector2 guiPos = new Vector2(screenPosition.x, Screen.height - screenPosition.y);
            if (controlsView != null && controlsView.enabled && controlsView.gameObject.activeInHierarchy)
            {
                if (controlsView.PanelRect.Contains(guiPos))
                {
                    return true;
                }
            }

            if (debugView != null && debugView.enabled && debugView.gameObject.activeInHierarchy)
            {
                if (debugView.PanelRect.Contains(guiPos))
                {
                    return true;
                }
            }

            return false;
        }

        public bool TryPlaceAtScreenPosition(Vector2 screenPosition)
        {
            return TryApplyPointer(screenPosition, false);
        }

        public bool TryDeleteAtScreenPosition(Vector2 screenPosition)
        {
            return TryApplyPointer(screenPosition, true);
        }

        public bool PlaceScreenCenter()
        {
            if (inputCamera == null)
            {
                return false;
            }

            return TryApplyPointer(new Vector2(inputCamera.pixelWidth * 0.5f, inputCamera.pixelHeight * 0.5f), false);
        }

        public bool DeleteScreenCenter()
        {
            if (inputCamera == null)
            {
                return false;
            }

            return TryApplyPointer(new Vector2(inputCamera.pixelWidth * 0.5f, inputCamera.pixelHeight * 0.5f), true);
        }

        private bool TryApplyPointer(Vector2 screenPosition, bool delete)
        {
            if (placementService == null || placementState == null || townGridView == null || inputCamera == null)
            {
                return false;
            }

            var ray = inputCamera.ScreenPointToRay(screenPosition);
            if (!gridPlane.Raycast(ray, out var distance))
            {
                return false;
            }

            var worldPosition = ray.GetPoint(distance);
            if (!townGridView.TryGetCoordFromWorld(worldPosition, out var coord))
            {
                return false;
            }

            debugState.Select(coord);
            return delete
                ? placementService.TryDeleteBlock(coord)
                : placementService.TryPlaceBlock(coord, placementState.CurrentColorId, placementState.CurrentMaterialId);
        }
    }
}
