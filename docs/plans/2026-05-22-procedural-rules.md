# Kế hoạch Triển khai Hệ thống Luật Thủ tục (Procedural Rules System)

> **Dành cho Antigravity:** QUY TRÌNH BẮT BUỘC: Sử dụng `.agent/workflows/execute-plan.md` để thực thi kế hoạch này ở chế độ single-flow (tuần tự từng bước).

**Mục tiêu:** Triển khai các quy tắc sinh tự động (procedural rules) hướng dữ liệu (data-first) để biến đổi các khối xây dựng của Cozy Builder thành các ngôi nhà nhỏ, tháp, nhà liền kề, mái nhà và móng ven sông dựa trên chiều cao xếp chồng và các ô liền kề.

**Kiến trúc:** Sử dụng các cấu trúc dữ liệu C# thuần túy để đánh giá các quy tắc bố cục (ví dụ: chiều cao, nước, số lượng ô liền kề) bên trong `RuleEvaluator` và trả về một `RuleResult` có chứa `VisualId` và `RotationId`. Liên kết `TownGridView` để đánh giá các quy tắc này và hoán đổi động các prefab hình ảnh trực quan bằng cách sử dụng hệ thống object pooling dạng hàng đợi (Queue) dựa trên Dictionary nhằm ngăn chặn việc cấp phát rác làm giảm hiệu năng.

**Công nghệ sử dụng:** Unity 2022.x/2023.x, C# (Unity Runtime), VContainer DI.

---

### Nhiệm vụ 1: Tái cấu trúc cấu trúc dữ liệu RuleResult

**Các file cần chỉnh sửa:**
- Sửa đổi: `Cozy_Builder/Assets/CozyBuilder/Runtime/Town/Data/RuleResult.cs`

**Bước 1: Kiểm tra mã nguồn hiện tại của RuleResult.cs**
- Mở `Cozy_Builder/Assets/CozyBuilder/Runtime/Town/Data/RuleResult.cs` để xác định các thuộc tính và constructor hiện có.

**Bước 2: Thêm thuộc tính RotationId**
- Thêm thuộc tính `public byte RotationId { get; set; }` để lưu giữ các giá trị xoay:
  - `0`: 0°
  - `1`: 90°
  - `2`: 180°
  - `3`: 270°
- Cập nhật các constructor để hỗ trợ tương thích ngược (backwards compatibility) bằng cách đặt giá trị mặc định cho `rotationId = 0`.

**Bước 3: Xác minh biên dịch**
- Đảm bảo dự án Unity biên dịch thành công không có lỗi.

**Bước 4: Commit các thay đổi**
```bash
git add Cozy_Builder/Assets/CozyBuilder/Runtime/Town/Data/RuleResult.cs
git commit -m "refactor: add RotationId to RuleResult with backwards compatibility"
```

---

### Nhiệm vụ 2: Triển khai logic nâng cao trong RuleEvaluator.cs

**Các file cần chỉnh sửa:**
- Sửa đổi: `Cozy_Builder/Assets/CozyBuilder/Runtime/Town/Rules/RuleEvaluator.cs`

**Bước 1: Cập nhật chữ ký (signature) của phương thức Evaluate**
- Thay đổi phương thức `Evaluate` để nhận các tham số: `GridCoord coord, int layer, in CellData cell, TownData townData`.

**Bước 2: Triển khai các quy tắc biến đổi khối dựa trên chiều cao xếp chồng và ô lân cận**
- **Waterfront Foundation (Móng ven sông)**: Nếu ô đất có cờ `CellFlags.HasWaterfront` và là tầng chân đế (`layer == 1`), trả về `VisualId = 4` (Cột chống/Stilts).
- **Single Standalone House (Nhà đơn độc lập)**: Nếu là tầng trên cùng (`layer == cell.Height`), chiều cao chỉ có 1 (`cell.Height == 1`), và tất cả 4 ô liền kề hướng cardinal có chiều cao bằng 0, trả về `VisualId = 1` (Nhà nhỏ/Small House).
- **Tower Top (Đỉnh tháp)**: Nếu là tầng trên cùng và chiều cao ô đất lớn hơn tất cả các ô lân cận hướng cardinal, trả về `VisualId = 3` (Đỉnh tháp/Tower Top).
- **Row Houses Roof (Mái nhà liền kề)**: Nếu là tầng trên cùng, căn chỉnh trục mái nhà để kết nối với các ô lân cận có chiều cao tương ứng:
  - Có lân cận hướng Đông/Tây: Mái chạy hướng Đông-Tây (`RotationId = 0`).
  - Có lân cận hướng Bắc/Nam: Mái chạy hướng Bắc-Nam (`RotationId = 1` - xoay 90°).
- **Wall Layers (Các tầng tường phía dưới)**: Nếu tầng hiện tại nhỏ hơn chiều cao ô đất (`layer < cell.Height`):
  - Trả về tường nhà liền kề (`VisualId = 5`) nếu có các ô lân cận kết nối theo hàng.
  - Trả về tường tháp tròn (`VisualId = 6`) nếu không có ô lân cận kết nối.

**Bước 3: Xác minh biên dịch**
- Kiểm tra xem mã nguồn có biên dịch thành công trong Unity Editor không.

**Bước 4: Commit các thay đổi**
```bash
git add Cozy_Builder/Assets/CozyBuilder/Runtime/Town/Rules/RuleEvaluator.cs
git commit -m "feat: implement height stack and neighbor-aware rules in RuleEvaluator"
```

---

### Nhiệm vụ 3: Tái cấu trúc chữ ký gọi hàm trong PlacementService.cs

**Các file cần chỉnh sửa:**
- Sửa đổi: `Cozy_Builder/Assets/CozyBuilder/Runtime/Town/Placement/PlacementService.cs`

**Bước 1: Cập nhật phương thức Preview trong PlacementService**
- Thay đổi chữ ký phương thức thành: `public RuleResult Preview(GridCoord coord, in CellData cell)`.
- Truyền đúng các đối số mới vào lời gọi `ruleEvaluator.Evaluate(coord, targetLayer, in cell, townData)`.

**Bước 2: Xác minh biên dịch**
- Xác nhận dự án biên dịch thành công.

**Bước 3: Commit các thay đổi**
```bash
git add Cozy_Builder/Assets/CozyBuilder/Runtime/Town/Placement/PlacementService.cs
git commit -m "refactor: update Preview signature in PlacementService to pass town data and cell coordinate"
```

---

### Nhiệm vụ 4: Tái cấu trúc lời gọi hàm trong Debug UI

**Các file cần chỉnh sửa:**
- Sửa đổi: `Cozy_Builder/Assets/CozyBuilder/Runtime/Town/Debugging/PrototypeTownDebugView.cs`

**Bước 1: Cập nhật các lời gọi xem trước trong debug view**
- Tìm lời gọi phương thức xem trước (Preview) bên trong `PrototypeTownDebugView.cs` và điều chỉnh thứ tự tham số: `placementService.Preview(coord, in cell)`.

**Bước 2: Xác minh biên dịch**
- Đảm bảo dự án biên dịch hoàn toàn thành công.

**Bước 3: Commit các thay đổi**
```bash
git add Cozy_Builder/Assets/CozyBuilder/Runtime/Town/Debugging/PrototypeTownDebugView.cs
git commit -m "refactor: adjust preview method invocation parameters in PrototypeTownDebugView"
```

---

### Nhiệm vụ 5: Cấu hình Prefab và Object Pooling trong TownGridView.cs

**Các file cần chỉnh sửa:**
- Sửa đổi: `Cozy_Builder/Assets/CozyBuilder/Runtime/Town/Rendering/TownGridView.cs`

**Bước 1: Thêm các trường prefab SerializedField mới có cơ chế fallback an toàn**
- Khai báo SerializedField cho:
  - `smallHousePrefab` (VisualId = 1)
  - `houseRoofPrefab` (VisualId = 2)
  - `towerTopPrefab` (VisualId = 3)
  - `stiltsPrefab` (VisualId = 4)
  - `houseWallPrefab` (VisualId = 5)
  - `towerWallPrefab` (VisualId = 6)
- Triển khai logic dự phòng an toàn: Nếu bất kỳ trường prefab chuyên dụng nào bị null trong Inspector, hệ thống sẽ tự động sử dụng `blockPrefab` (khối lập phương màu xám mặc định) để đảm bảo không bị lỗi runtime.

**Bước 2: Triển khai hệ thống pooling động dựa trên Dictionary**
- Khai báo `private Dictionary<ushort, Queue<GameObject>> pools = new Dictionary<ushort, Queue<GameObject>>();`
- Tái sử dụng các visual game object đang hoạt động dưới `CellVisualState.BlockViews`.
- Khi chiều cao hoặc hình dáng khối của ô đất thay đổi:
  - **Hủy kích hoạt và thu hồi**: Tắt hiển thị visual cũ bằng `gameObject.SetActive(false)`, gán lại `transform.SetParent(null)` hoặc giữ nguyên tùy cấu trúc pool, rồi đưa khối hình ảnh cũ về hàng đợi pool tương ứng.
  - **Kích hoạt và tái sử dụng**: Lấy khối mới thuộc loại `VisualId` vừa đánh giá từ pool (nếu trống thì instantiate mới), gọi `gameObject.SetActive(true)`, và gán lại cha bằng `transform.SetParent` về dưới nhóm `Generated Town Cells/Block Cells`.

**Bước 3: Reset trạng thái (Reset State) và Áp dụng góc xoay**
- Để tránh lệch vị trí hoặc tỉ lệ khi tái sử dụng các khối visual khác loại:
  - Thiết lập chính xác `transform.localPosition` tương ứng với vị trí ô đất và độ cao tầng (layer).
  - Reset `transform.localScale` về `Vector3.one`.
  - Sử dụng `RuleResult.RotationId` để tính toán góc xoay thích hợp: `Quaternion.Euler(0f, result.RotationId * 90f, 0f)` và áp dụng nó vào `transform.localRotation` của khối visual được kích hoạt.

**Bước 4: Xác minh biên dịch**
- Kiểm tra trạng thái biên dịch.

**Bước 5: Commit các thay đổi**
```bash
git add Cozy_Builder/Assets/CozyBuilder/Runtime/Town/Rendering/TownGridView.cs
git commit -m "feat: implement zero-allocation pooling, rotation mapping, and specialized prefab slots in TownGridView"
```

---

### Nhiệm vụ 6: Xác minh Thủ công và Đồng bộ hóa Đồ thị Kiến thức (Knowledge Graph)

**Bước 1: Kiểm thử trong Play Mode của Unity**
- Khởi động Unity Editor.
- Gán các prefab tương ứng vào các slot mới trên inspector của `TownGridView`.
- Vào Play Mode và kiểm thử đặt khối:
  - Đặt 1 khối đơn lẻ: Xác minh nó hiển thị thành **Small House** độc lập.
  - Đặt các khối liền kề nhau thành hàng: Xác minh chúng kết nối thành **Row House** với mái xếp thẳng hàng theo trục.
  - Xếp chồng các khối theo chiều dọc (độ cao >= 2): Xác minh các tầng dưới hiển thị là **Wall** và tầng trên cùng là **Tower Top / Roof**.
  - Đặt khối trên ô đất ven sông (có cờ `HasWaterfront`): Xác minh tầng chân đế hiển thị là cột chống **Stilts**.
  - Kiểm tra bảng IMGUI `PrototypeTownDebugView` để đối chiếu các thuộc tính ô lân cận và `VisualId` có khớp chính xác không.

**Bước 2: Cập nhật đồ thị code Graphify**
- Chạy lệnh `graphify update .` để đồng bộ lại sơ đồ cây AST-only của dự án.

**Bước 3: Commit hoàn thành**
```bash
git commit --allow-empty -m "docs: finalize procedural rules system integration"
```
