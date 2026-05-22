using CozyBuilder.Camera;
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

        [Header("Tap Constraints")]
        [SerializeField] private float tapDurationThreshold = 0.25f;
        [SerializeField] private float tapMoveThreshold = 15f;
        [SerializeField] private float doubleTapInterval = 0.25f;

        private PlacementService placementService;
        private PrototypePlacementState placementState;
        private PrototypeTownDebugState debugState;
        private TownGridView townGridView;
        private PrototypePlacementControlsView controlsView;
        private PrototypeTownDebugView debugView;
        private CameraService cameraService;
        
        private Plane gridPlane;

        // Trạng thái cảm ứng
        private Vector2 touchStartPos;
        private float touchStartTime;
        private bool isPossibleTap;
        
        // Quản lý trạng thái trễ để phân biệt Tap và Double-Tap
        private float lastTapReleaseTime = -999f;
        private Vector2 lastTapReleasePos;
        private bool isPendingSingleTap = false;
        private float pendingSingleTapExecuteTime = 0f;

        [Inject]
        public void Construct(
            PlacementService placementService,
            PrototypePlacementState placementState,
            PrototypeTownDebugState debugState,
            TownGridView townGridView,
            PrototypePlacementControlsView controlsView,
            PrototypeTownDebugView debugView,
            CameraService cameraService)
        {
            this.placementService = placementService;
            this.placementState = placementState;
            this.debugState = debugState;
            this.townGridView = townGridView;
            this.controlsView = controlsView;
            this.debugView = debugView;
            this.cameraService = cameraService;
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
                HandleMouseInput(mouse);
            }

            var touchscreen = Touchscreen.current;
            if (touchscreen != null)
            {
                HandleTouchInput(touchscreen);
            }

            // Thực thi lệnh đặt block (Single-Tap) trễ nếu hết thời gian chờ Double-Tap
            if (isPendingSingleTap && Time.time >= pendingSingleTapExecuteTime)
            {
                ExecuteSingleTap(lastTapReleasePos);
            }
        }

        private void HandleTouchInput(Touchscreen touchscreen)
        {
            var primaryTouch = touchscreen.primaryTouch;
            var screenPos = primaryTouch.position.ReadValue();

            // 1. Khi ngón tay bắt đầu chạm xuống màn hình
            if (primaryTouch.press.wasPressedThisFrame)
            {
                // Chỉ nhận diện là cú chạm hợp lệ nếu không bấm lên UI
                isPossibleTap = !IsPointerOverUI(screenPos);
                touchStartPos = screenPos;
                touchStartTime = Time.time;
            }

            // 2. Liên tục kiểm tra dịch chuyển ngón tay trong lúc nhấn đè
            if (primaryTouch.press.isPressed && isPossibleTap)
            {
                if (Vector2.Distance(screenPos, touchStartPos) > tapMoveThreshold)
                {
                    isPossibleTap = false; // Đã vuốt kéo di chuyển (đang xoay camera), hủy lệnh Tap
                }
            }

            // 3. Khi ngón tay thả ra khỏi màn hình
            if (primaryTouch.press.wasReleasedThisFrame)
            {
                // Đảm bảo không kết thúc đè lên nút UI
                if (isPossibleTap && (Time.time - touchStartTime) <= tapDurationThreshold && !IsPointerOverUI(screenPos))
                {
                    float timeSinceLastTap = Time.time - lastTapReleaseTime;
                    if (timeSinceLastTap <= doubleTapInterval)
                    {
                        // Nhấp đúp (Double-Tap): Hủy cú chạm đơn đang chờ và kích hoạt Lấy nét camera
                        isPendingSingleTap = false;
                        ExecuteDoubleTap(screenPos);
                    }
                    else
                    {
                        // Đưa cú chạm đơn vào trạng thái chờ trễ
                        lastTapReleaseTime = Time.time;
                        lastTapReleasePos = screenPos;
                        isPendingSingleTap = true;
                        pendingSingleTapExecuteTime = Time.time + 0.15f; // Chờ 0.15s xem có nhấp đúp hay không
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
            if (cameraService == null || inputCamera == null || townGridView == null)
            {
                return;
            }

            var ray = inputCamera.ScreenPointToRay(screenPosition);
            if (gridPlane.Raycast(ray, out var distance))
            {
                var worldPosition = ray.GetPoint(distance);
                if (townGridView.TryGetCoordFromWorld(worldPosition, out var coord))
                {
                    // Lấy nét camera vào trung tâm ô đất được nhấp đúp
                    // KayKit spacing là 2m, nên nhân hệ số 2
                    Vector3 cellWorldPos = townGridView.transform.position + new Vector3(coord.X * 2.0f, 0f, coord.Y * 2.0f);
                    cameraService.FocusOn(cellWorldPos);
                    
                    if (debugState != null)
                    {
                        debugState.Select(coord);
                    }
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

        private bool IsPointerOverUI(Vector2 screenPosition)
        {
            // 1. Kiểm tra EventSystem (uGUI / UI Toolkit / Canvas)
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return true;
            }

            // 2. Kiểm tra các bảng điều khiển IMGUI
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

            if (debugState != null)
            {
                debugState.Select(coord);
            }

            return delete
                ? placementService.TryDeleteBlock(coord)
                : placementService.TryPlaceBlock(coord, placementState.CurrentColorId, placementState.CurrentMaterialId);
        }
    }
}
