# Kế hoạch Triển khai Định tuyến Nhập liệu và Chặn Pointer (UI/Input Routing & Pointer Blocking)

**Mục tiêu:** Giải quyết xung đột nhập liệu trong Cozy Builder bằng cách chặn tia raycast đặt/xóa khối và thao tác điều khiển camera (orbit/pan/zoom) khi người dùng nhấp chuột hoặc chạm lên giao diện người dùng (cả IMGUI hiện tại và các hệ thống uGUI/UI Toolkit trong tương lai).

## User Review Required

> [!IMPORTANT]
> Cải tiến này tách biệt hoàn toàn thao tác điều khiển camera, đặt khối và tương tác giao diện giúp trải nghiệm chơi thử ổn định hơn mà không làm thay đổi logic lõi của trò chơi.

## Proposed Changes

### [Component: UI & Input Routing]

---

#### [MODIFY] [PrototypePlacementControlsView.cs](file:///d:/soflware/Unity/Source/App/Cozy_Builder/Assets/CozyBuilder/Runtime/Town/Placement/PrototypePlacementControlsView.cs)
- Cung cấp thuộc tính public getter để lộ phạm vi vùng Rect của bảng điều khiển để các driver nhập liệu có thể truy vấn.
```csharp
public Rect PanelRect => panelRect;
```

#### [MODIFY] [PrototypeTownDebugView.cs](file:///d:/soflware/Unity/Source/App/Cozy_Builder/Assets/CozyBuilder/Runtime/Town/Debugging/PrototypeTownDebugView.cs)
- Cung cấp thuộc tính public getter để lộ phạm vi vùng Rect của bảng debug.
```csharp
public Rect PanelRect => panelRect;
```

#### [MODIFY] [PrototypePlacementInputDriver.cs](file:///d:/soflware/Unity/Source/App/Cozy_Builder/Assets/CozyBuilder/Runtime/Town/Placement/PrototypePlacementInputDriver.cs)
- Inject `PrototypePlacementControlsView` và `PrototypeTownDebugView`.
- Thêm phương thức `IsPointerOverUI(Vector2 screenPosition)` để kiểm tra va chạm pointer:
  - Sử dụng `UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()` để hỗ trợ chặn pointer tự động của uGUI / UI Toolkit.
  - Sử dụng tọa độ Rect của 2 bảng IMGUI (chuyển đổi trục Y từ góc dưới bên trái sang góc trên bên trái) để chặn pointer của giao diện IMGUI hiện tại.
- Trong `Update()`, nếu pointer nằm đè lên bất kỳ giao diện UI nào, bỏ qua việc xử lý thao tác đặt hoặc xóa khối.

#### [MODIFY] [PrototypeCameraInputDriver.cs](file:///d:/soflware/Unity/Source/App/Cozy_Builder/Assets/CozyBuilder/Runtime/Camera/PrototypeCameraInputDriver.cs)
- Inject `PrototypePlacementControlsView` và `PrototypeTownDebugView`.
- Triển khai phương thức kiểm tra `IsPointerOverUI(Vector2 screenPosition)` tương tự để kiểm tra va chạm pointer.
- Thêm các biến cờ trạng thái:
  - `private bool wasDragStartedOverUI = false;` để theo dõi thao tác chuột.
  - `private bool wasTouchStartedOverUI = false;` để theo dõi thao tác chạm.
- Chặn thao tác kéo camera (orbit/pan) nếu lượt nhấn đầu tiên (`wasPressedThisFrame`) bắt đầu đè lên UI. Giải phóng trạng thái khi nhấc ngón tay/chuột.
- Chặn thao tác cuộn phóng to/thu nhỏ (`scroll`) nếu con trỏ chuột hiện tại đang nằm đè lên UI.

## Verification Plan

### Manual Verification (Unity Play Mode)
1. **Kiểm tra đặt/xóa khối xuyên qua UI**:
   - Vào Play Mode, bấm chọn các nút trên bảng điều khiển `Town Grid View` (ví dụ nút Mode `Place`, `Delete` hoặc nút Color `0..3`).
   - Xác minh: Chế độ hoặc màu sắc thay đổi chuẩn xác, đồng thời **không** có khối nào bị đặt hoặc bị xóa bên dưới vị trí của nút bấm.
2. **Kiểm tra xoay/pan camera xuyên qua UI**:
   - Nhấp giữ nút bấm hoặc nhấp vào vùng bảng UI điều khiển và thử di chuyển chuột/vuốt màn hình.
   - Xác minh: Camera hoàn toàn đứng yên, **không** bị xoay hoặc dịch chuyển giật cục.
3. **Kiểm tra tính liên tục của thao tác kéo (Drag Continuity)**:
   - Nhấp giữ `Alt + Chuột trái` bắt đầu từ bên ngoài bảng UI để xoay camera, sau đó kéo chuột đi qua vùng bảng UI.
   - Xác minh: Camera vẫn tiếp tục xoay mượt mà (không bị khựng hay dừng lại nửa chừng do chuột đi vào vùng UI).
4. **Kiểm tra phóng to/thu nhỏ bằng con lăn**:
   - Di chuột đè lên bảng UI và cuộn con lăn.
   - Xác minh: Camera **không** bị zoom gần/xa.
