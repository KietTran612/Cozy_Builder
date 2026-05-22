using CozyBuilder.Town.Rendering;
using UnityEngine;
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
        private TownGridView townGridView;
        private Plane gridPlane;

        [Inject]
        public void Construct(
            PlacementService placementService,
            PrototypePlacementState placementState,
            TownGridView townGridView)
        {
            this.placementService = placementService;
            this.placementState = placementState;
            this.townGridView = townGridView;
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
            if (mouse != null && mouse.leftButton.wasPressedThisFrame)
            {
                TryApplyPointer(mouse.position.ReadValue(), placementState != null && placementState.IsDeleteMode);
            }

            if (mouse != null && mouse.rightButton.wasPressedThisFrame)
            {
                TryApplyPointer(mouse.position.ReadValue(), true);
            }

            var touchscreen = Touchscreen.current;
            if (touchscreen != null && touchscreen.primaryTouch.press.wasPressedThisFrame)
            {
                TryApplyPointer(touchscreen.primaryTouch.position.ReadValue(), placementState != null && placementState.IsDeleteMode);
            }
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

            return delete
                ? placementService.TryDeleteBlock(coord)
                : placementService.TryPlaceBlock(coord, placementState.CurrentColorId, placementState.CurrentMaterialId);
        }
    }
}
