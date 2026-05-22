# Kế hoạch tích hợp Bảng màu & Chất liệu trực quan (Color & Material Visual Integration)

Tài liệu này mô tả chi tiết giải pháp kỹ thuật tích hợp bảng màu sắc và chất liệu động lên các khối Block 3D trên Scene, dựa trên dữ liệu `ColorId` và `MaterialId` đã có trong Data Model, đạt hiệu năng render tối ưu và **Zero GC Allocations** khi chạy trên mobile.

## User Review Required

> [!IMPORTANT]
> **Giải pháp kỹ thuật Zero GC Allocations:**
> Để thay đổi màu sắc và độ bóng/kim loại động của hàng trăm khối nhà trên scene mà không gây sụt giảm FPS và sinh rác bộ nhớ (GC Allocations), kế hoạch này áp dụng cơ chế **`MaterialPropertyBlock`** của Unity. 
> Giải pháp này cho phép chia sẻ chung một Material duy nhất nhưng hiển thị các màu sắc và độ mịn khác nhau trên từng Renderer riêng lẻ.
> 
> **BlockColorAdapter Component:**
> Chúng ta sẽ tạo một MonoBehaviour gọn nhẹ đặt tên là `BlockColorAdapter` để gắn lên các block prefab. Component này lưu trữ sẵn mảng `Renderer[]` để tránh việc gọi `GetComponentsInChildren` lúc runtime (vốn sinh ra mảng rác mới gây GC).

## Open Questions

> [!NOTE]
> Không có câu hỏi mở quan trọng. Mẫu màu sắc ấm cúng kiểu pastel và cấu hình chất liệu cơ bản (đá, gỗ, kim loại, gốm) đã được lựa chọn hài hòa và đồng nhất với thiết kế Cozy Town.

## Proposed Changes

### CozyBuilder.Runtime

#### [NEW] [BlockColorAdapter.cs](file:///c:/1.SOURCE/Unity/Source/Cozy_Builder/Cozy_Builder/Assets/CozyBuilder/Runtime/Town/Rendering/BlockColorAdapter.cs)
Tạo component adapter gắn lên các block prefab để lưu trữ và cache danh sách các Renderer con, tránh GC allocations khi truy vấn lúc runtime.

```csharp
using UnityEngine;

namespace CozyBuilder.Town.Rendering
{
    [DisallowMultipleComponent]
    public sealed class BlockColorAdapter : MonoBehaviour
    {
        [SerializeField] private Renderer[] renderers;

        public Renderer[] Renderers
        {
            get
            {
                // Fallback nếu chưa kéo thả sẵn trong Inspector
                if (renderers == null || renderers.Length == 0)
                {
                    renderers = GetComponentsInChildren<Renderer>(true);
                }
                return renderers;
            }
        }

        private void Reset()
        {
            // Tự động tìm kiếm khi gắn component trong Editor
            renderers = GetComponentsInChildren<Renderer>(true);
        }
    }
}
```

#### [MODIFY] [TownGridView.cs](file:///c:/1.SOURCE/Unity/Source/Cozy_Builder/Cozy_Builder/Assets/CozyBuilder/Runtime/Town/Rendering/TownGridView.cs)
- Cấu hình mảng màu Cozy Palette (3-6 màu pastel đẹp mắt) ngay trong Inspector để Designer dễ chỉnh sửa.
- Định nghĩa các cấu hình chất liệu (Smoothness, Metallic) dựa trên `MaterialId`.
- Khởi tạo một `MaterialPropertyBlock` dùng chung để tái sử dụng.
- Cập nhật logic trong `ApplyBlockState` để áp dụng màu và chất liệu động lên các renderer của block.
- Sử dụng cơ chế fallback thông minh bằng danh sách tĩnh (`List<Renderer>`) nếu block không gắn `BlockColorAdapter`.

```csharp
// Thêm khai báo mảng màu mặc định trong TownGridView.cs:
[Header("Color & Material Config")]
[SerializeField] private Color[] cozyPalette = new Color[]
{
    new Color(0.878f, 0.353f, 0.278f), // 0: Đỏ gạch ấm (#e05a47)
    new Color(0.902f, 0.663f, 0.333f), // 1: Vàng mù tạt (#e6a955)
    new Color(0.220f, 0.478f, 0.482f), // 2: Xanh cổ vịt/teal (#387a7b)
    new Color(0.455f, 0.639f, 0.702f), // 3: Xanh trời nhạt (#74a3b3)
    new Color(0.900f, 0.851f, 0.761f), // 4: Kem đá/stone (#e5d9c2)
    new Color(0.439f, 0.545f, 0.459f)  // 5: Xanh lá xám (#708b75)
};

// Khai báo Property Block và Cache List dùng chung:
private MaterialPropertyBlock propertyBlock;
private static readonly List<Renderer> tempRendererList = new List<Renderer>(8);
private static readonly int baseColorPropId = Shader.PropertyToID("_BaseColor");
private static readonly int smoothnessPropId = Shader.PropertyToID("_Smoothness");
private static readonly int metallicPropId = Shader.PropertyToID("_Metallic");
```

---

## Verification Plan

### Automated Tests
- Biển dịch dự án C# không có lỗi và không có cảnh báo (`0 warnings`).
- Chạy lệnh `graphify update .` để đồng bộ hóa đồ thị mã nguồn mới.

### Manual Verification
- **Kiểm thử trực quan (Visual Verification)**: Mở Play Mode, đặt các block với các `ColorId` và `MaterialId` khác nhau từ IMGUI Panel, kiểm tra xem các khối nhà có thay đổi màu sắc pastel sinh động và có độ phản chiếu ánh sáng (độ bóng/mịn) khác nhau hay không.
- **Kiểm thử hiệu năng (GC Verification)**: Mở Unity Profiler, lọc theo `TownGridView.LateUpdate` và `ProcessDirtyCells`, đảm bảo **không có GC Allocations (0 Bytes)** sinh ra trong quá trình đặt, thay đổi màu sắc hoặc xóa block liên tục.
