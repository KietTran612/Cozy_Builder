# Kế hoạch Triển khai Module Hóa và Tách Biệt Hệ Thống Camera (Camera Decoupling Implementation Plan)

> **For Antigravity:** REQUIRED WORKFLOW: Use `.agent/workflows/execute-plan.md` to execute this plan in single-flow mode.

**Goal:** Di chuyển hệ thống Camera sang một Assembly riêng biệt và khử bỏ liên kết chặt chẽ (tight coupling) với các thành phần UI thông qua Interface và Dependency Injection.

**Architecture:** Tạo Assembly Definition `CozyBuilder.Camera` độc lập cho camera. Sử dụng DIP qua interface `ICameraInputBlocker` và VContainer để tiêm danh sách blockers động vào `PrototypeCameraInputDriver`, giúp cô lập hoàn toàn mã nguồn camera.

**Tech Stack:** Unity 6000.3.11f1, VContainer, Unity Input System.

---

### Task 1: Tạo Interface ICameraInputBlocker

**Files:**
- Create: `Cozy_Builder/Assets/CozyBuilder/Runtime/Camera/ICameraInputBlocker.cs`

**Step 1: Viết mã nguồn Interface**
Tạo tệp `ICameraInputBlocker.cs` trong không gian tên `CozyBuilder.Camera`:

```csharp
using UnityEngine;

namespace CozyBuilder.Camera
{
    public interface ICameraInputBlocker
    {
        bool IsPointerOverUI(Vector2 screenPosition);
    }
}
```

**Step 2: Commit**
```bash
git add Cozy_Builder/Assets/CozyBuilder/Runtime/Camera/ICameraInputBlocker.cs
git commit -m "feat: add ICameraInputBlocker interface for modular camera interaction blocking"
```

---

### Task 2: Định nghĩa Assembly Definition CozyBuilder.Camera

**Files:**
- Create: `Cozy_Builder/Assets/CozyBuilder/Runtime/Camera/CozyBuilder.Camera.asmdef`

**Step 1: Viết mã nguồn asmdef**
Tạo định nghĩa Assembly `CozyBuilder.Camera.asmdef` để tham chiếu tới VContainer và InputSystem:

```json
{
    "name": "CozyBuilder.Camera",
    "rootNamespace": "CozyBuilder",
    "references": [
        "VContainer",
        "Unity.InputSystem"
    ],
    "includePlatforms": [],
    "excludePlatforms": [],
    "allowUnsafeCode": false,
    "overrideReferences": false,
    "precompiledReferences": [],
    "autoReferenced": true,
    "defineConstraints": [],
    "versionDefines": [],
    "noEngineReferences": false
}
```

**Step 2: Commit**
```bash
git add Cozy_Builder/Assets/CozyBuilder/Runtime/Camera/CozyBuilder.Camera.asmdef
git commit -m "feat: create CozyBuilder.Camera assembly definition"
```

---

### Task 3: Cập nhật CozyBuilder.Runtime.asmdef

**Files:**
- Modify: `Cozy_Builder/Assets/CozyBuilder/Runtime/CozyBuilder.Runtime.asmdef`

**Step 1: Thêm reference CozyBuilder.Camera**
Sửa đổi tệp `CozyBuilder.Runtime.asmdef` để bao gồm tham chiếu tới Assembly Camera mới:

```json
{
    "name": "CozyBuilder.Runtime",
    "rootNamespace": "CozyBuilder",
    "references": [
        "VContainer",
        "UniTask",
        "Unity.InputSystem",
        "CozyBuilder.Camera"
    ],
    "includePlatforms": [],
    "excludePlatforms": [],
    "allowUnsafeCode": false,
    "overrideReferences": false,
    "precompiledReferences": [],
    "autoReferenced": true,
    "defineConstraints": [],
    "versionDefines": [],
    "noEngineReferences": false
}
```

**Step 2: Commit**
```bash
git add Cozy_Builder/Assets/CozyBuilder/Runtime/CozyBuilder.Runtime.asmdef
git commit -m "refactor: update runtime asmdef to reference camera assembly"
```

---

### Task 4: Tách biệt PrototypeCameraInputDriver khỏi các UI View cụ thể

**Files:**
- Modify: `Cozy_Builder/Assets/CozyBuilder/Runtime/Camera/PrototypeCameraInputDriver.cs`

**Step 1: Loại bỏ using và biến View chặt chẽ, inject ICameraInputBlocker**
Chỉnh sửa mã nguồn của `PrototypeCameraInputDriver.cs` để nhận danh sách `IReadOnlyList<ICameraInputBlocker>` động và loại bỏ các using liên quan đến Town và Debug.

Mã thay thế cho phần constructor và các trường biến:
```csharp
// Xóa:
// using CozyBuilder.Town.Placement;
// using CozyBuilder.Town.Debugging;

// Thay bằng:
using System.Collections.Generic;

// ...
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
```

Mã thay thế cho phương thức `IsPointerOverUI`:
```csharp
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
```

**Step 2: Commit**
```bash
git add Cozy_Builder/Assets/CozyBuilder/Runtime/Camera/PrototypeCameraInputDriver.cs
git commit -m "refactor: decouple PrototypeCameraInputDriver from runtime UI panels using ICameraInputBlocker"
```

---

### Task 5: Thực hiện ICameraInputBlocker trong các View giao diện

**Files:**
- Modify: `Cozy_Builder/Assets/CozyBuilder/Runtime/Town/Placement/PrototypePlacementControlsView.cs`
- Modify: `Cozy_Builder/Assets/CozyBuilder/Runtime/Town/Debugging/PrototypeTownDebugView.cs`

**Step 1: Kế thừa và triển khai interface trong PrototypePlacementControlsView.cs**
Cập nhật lớp kế thừa thêm `ICameraInputBlocker`:
```csharp
using CozyBuilder.Camera;

namespace CozyBuilder.Town.Placement
{
    public sealed class PrototypePlacementControlsView : MonoBehaviour, ICameraInputBlocker
    {
        // ...
        public bool IsPointerOverUI(Vector2 screenPosition)
        {
            if (!enabled || !gameObject.activeInHierarchy)
            {
                return false;
            }

            Vector2 guiPos = new Vector2(screenPosition.x, Screen.height - screenPosition.y);
            return panelRect.Contains(guiPos);
        }
    }
}
```

**Step 2: Kế thừa và triển khai interface trong PrototypeTownDebugView.cs**
Cập nhật lớp kế thừa thêm `ICameraInputBlocker`:
```csharp
using CozyBuilder.Camera;

namespace CozyBuilder.Town.Debugging
{
    public sealed class PrototypeTownDebugView : MonoBehaviour, ICameraInputBlocker
    {
        // ...
        public bool IsPointerOverUI(Vector2 screenPosition)
        {
            if (!enabled || !gameObject.activeInHierarchy)
            {
                return false;
            }

            Vector2 guiPos = new Vector2(screenPosition.x, Screen.height - screenPosition.y);
            return panelRect.Contains(guiPos);
        }
    }
}
```

**Step 3: Commit**
```bash
git add Cozy_Builder/Assets/CozyBuilder/Runtime/Town/Placement/PrototypePlacementControlsView.cs Cozy_Builder/Assets/CozyBuilder/Runtime/Town/Debugging/PrototypeTownDebugView.cs
git commit -m "feat: implement ICameraInputBlocker on prototype IMGUI views"
```

---

### Task 6: Cấu hình Dependency Injection trong GameLifetimeScope

**Files:**
- Modify: `Cozy_Builder/Assets/CozyBuilder/Runtime/Bootstrap/GameLifetimeScope.cs`

**Step 1: Thay đổi cách đăng ký View trên VContainer**
Cấu hình VContainer để đăng ký các View dưới dạng cả chính nó và `ICameraInputBlocker` để tiêm tự động:

```csharp
            // Thay đổi đăng ký:
            builder.RegisterComponentInHierarchy<PrototypePlacementControlsView>()
                .AsSelf()
                .As<ICameraInputBlocker>();
                
            builder.RegisterComponentInHierarchy<PrototypeTownDebugView>()
                .AsSelf()
                .As<ICameraInputBlocker>();
```

**Step 2: Commit**
```bash
git add Cozy_Builder/Assets/CozyBuilder/Runtime/Bootstrap/GameLifetimeScope.cs
git commit -m "feat: update VContainer registrations for UI views to act as ICameraInputBlocker"
```

---

### Task 7: Đồng bộ hóa Đồ thị Graphify và Kiểm thử biên dịch

**Step 1: Cập nhật đồ thị và kiểm tra lỗi biên dịch**
Run command để kiểm tra biên dịch mã nguồn và đồng bộ đồ thị AST Graphify:
```bash
graphify update .
```

---

### Task 8: Kiểm thử Play Mode thủ công

**Step 1: Xác minh hành vi**
1. Bật Unity Editor và vào Play Mode.
2. Kiểm tra thao tác xoay/zoom camera ngoài UI (hoạt động bình thường).
3. Kiểm tra rê chuột/chạm và thao tác cuộn trên IMGUI panels (đảm bảo hoàn toàn bị chặn).
