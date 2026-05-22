using CozyBuilder.Camera;
using CozyBuilder.Town.Data;
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
        
        // Quản lý trạng thái vẽ kéo liên tục (Drag-to-Draw)
        private GridCoord lastAppliedCoord;
        private bool hasLastAppliedCoord = false;
        
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

            if (TryGetWorldPositionFromScreen(screenPosition, out var worldPosition))
            {
                if (townGridView.TryGetCoordFromWorld(worldPosition, out var coord))
                {
                    // Lấy nét camera vào trung tâm ô đất được nhấp đúp
                    float spacing = townGridView.CellSpacing;
                    Vector3 cellWorldPos = townGridView.transform.position + new Vector3(coord.X * spacing, 0f, coord.Y * spacing);
                    cameraService.FocusOn(cellWorldPos);
                    
                    if (debugState != null)
                    {
                        debugState.Select(coord);
                    }
                }
            }
        }

        private bool TryGetWorldPositionFromScreen(Vector2 screenPosition, out Vector3 worldPosition)
        {
            if (inputCamera == null)
            {
                worldPosition = Vector3.zero;
                return false;
            }

            var ray = inputCamera.ScreenPointToRay(screenPosition);

            // 1. Ưu tiên dùng Physics Raycast để chạm chính xác vào Collider bề mặt 3D (ô đất lục giác, block nhà)
            if (Physics.Raycast(ray, out RaycastHit hit, 150f))
            {
                worldPosition = hit.point;
                return true;
            }

            // 2. Fallback về Raycast toán học nằm ngang Y = 0 nếu click ra ngoài khoảng không
            if (gridPlane.Raycast(ray, out var distance))
            {
                worldPosition = ray.GetPoint(distance);
                return true;
            }

            worldPosition = Vector3.zero;
            return false;
        }

        private void HandleMouseInput(Mouse mouse)
        {
            var screenPos = mouse.position.ReadValue();
            if (IsPointerOverUI(screenPos))
            {
                hasLastAppliedCoord = false;
                return;
            }

            var keyboard = Keyboard.current;
            var cameraModifierHeld = keyboard != null && (keyboard.leftAltKey.isPressed || keyboard.rightAltKey.isPressed);

            // 1. Nhấn giữ chuột trái để vẽ liên tục (Drag-to-Draw)
            if (mouse.leftButton.isPressed && !cameraModifierHeld)
            {
                if (TryGetWorldPositionFromScreen(screenPos, out var worldPosition))
                {
                    if (townGridView.TryGetCoordFromWorld(worldPosition, out var coord))
                    {
                        if (!hasLastAppliedCoord || coord != lastAppliedCoord)
                        {
                            bool deleteMode = placementState != null && placementState.IsDeleteMode;
                            TryApplyPointer(screenPos, deleteMode);
                            
                            lastAppliedCoord = coord;
                            hasLastAppliedCoord = true;
                        }
                    }
                    else
                    {
                        hasLastAppliedCoord = false;
                    }
                }
                else
                {
                    hasLastAppliedCoord = false;
                }
            }
            // 2. Nhấn giữ chuột phải để xóa liên tục (Drag-to-Delete)
            else if (mouse.rightButton.isPressed)
            {
                if (TryGetWorldPositionFromScreen(screenPos, out var worldPosition))
                {
                    if (townGridView.TryGetCoordFromWorld(worldPosition, out var coord))
                    {
                        if (!hasLastAppliedCoord || coord != lastAppliedCoord)
                        {
                            TryApplyPointer(screenPos, true);
                            
                            lastAppliedCoord = coord;
                            hasLastAppliedCoord = true;
                        }
                    }
                    else
                    {
                        hasLastAppliedCoord = false;
                    }
                }
                else
                {
                    hasLastAppliedCoord = false;
                }
            }
            else
            {
                // Nhả các nút chuột hoặc Alt xoay camera -> Reset vết vẽ liên tục
                hasLastAppliedCoord = false;
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

            if (!TryGetWorldPositionFromScreen(screenPosition, out var worldPosition))
            {
                return false;
            }

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
