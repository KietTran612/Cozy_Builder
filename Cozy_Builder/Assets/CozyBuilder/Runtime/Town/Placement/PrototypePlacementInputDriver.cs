using CozyBuilder.Town.Rendering;
using UnityEngine;
using VContainer;
using UnityCamera = UnityEngine.Camera;

namespace CozyBuilder.Town.Placement
{
    public sealed class PrototypePlacementInputDriver : MonoBehaviour
    {
        [SerializeField] private UnityCamera inputCamera;
        [SerializeField] private bool deleteMode;
        [SerializeField] private ushort colorId;
        [SerializeField] private ushort materialId;

        private PlacementService placementService;
        private TownGridView townGridView;
        private Plane gridPlane;

        [Inject]
        public void Construct(PlacementService placementService, TownGridView townGridView)
        {
            this.placementService = placementService;
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
            if (Input.GetMouseButtonDown(0))
            {
                TryApplyPointer(Input.mousePosition, deleteMode);
            }

            if (Input.GetMouseButtonDown(1))
            {
                TryApplyPointer(Input.mousePosition, true);
            }

            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    TryApplyPointer(touch.position, deleteMode);
                }
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
            if (placementService == null || townGridView == null || inputCamera == null)
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
                : placementService.TryPlaceBlock(coord, colorId, materialId);
        }
    }
}
