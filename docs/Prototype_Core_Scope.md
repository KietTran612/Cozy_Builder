# Prototype Core Scope

## Mục Tiêu

Prototype Core dùng để kiểm chứng cảm giác xây dựng cốt lõi trước khi mở rộng content. Prototype không phải bản đẹp hoàn chỉnh, không phải MVP, và không cần có mọi chức năng mong muốn.

Prototype phải trả lời 5 câu hỏi:

- Đặt/xóa block có dễ chịu không?
- Grid có dễ hiểu và dễ thao tác không?
- Procedural rule có tạo ra cảm giác thị trấn không?
- Camera có đủ tốt để xây trên mobile không?
- Data model có đủ sạch để mở rộng sang save/load, undo/redo và chunk rebuild không?

## Thời Lượng Đề Xuất

3-5 tuần sau khi hoàn tất pre-production.

## Chức Năng Có Trong Prototype

### 1. Grid Cơ Bản

- Organic island grid bản đầu.
- Mỗi cell có coordinate/id rõ.
- Có debug hiển thị cell id/neighbor.
- Có thể raycast/tap vào cell.

### 2. Placement

- Tap/click để đặt block.
- Tap/click chế độ xóa để xóa block.
- Height tăng khi đặt chồng.
- Data cập nhật trước, visual rebuild sau.

### 3. Color

- Palette cơ bản 3-6 màu.
- Màu lưu bằng `ColorId`, không lưu trực tiếp material instance trong cell.

### 4. Procedural Rule Tối Thiểu

Rule cần có:

- Cell đơn tạo nhà nhỏ.
- Stack nhiều tầng tạo nhà cao/tháp đơn giản.
- Cell cạnh nhau tạo dãy nhà.
- Roof cơ bản theo height/neighbor.
- Foundation/water edge đơn giản nếu có nước.

Không cần trong prototype:

- Cầu phức tạp.
- Cầu thang đẹp.
- Sân trong hoàn chỉnh.
- Cư dân.
- Street view.
- Nhiều terrain.

### 5. Camera

- Orbit.
- Pan.
- Zoom.
- Reset camera.
- Pivot quanh vùng người chơi thao tác nếu làm kịp.

### 6. Debug Tooling

- Toggle cell grid.
- Toggle neighbor info.
- Toggle dirty cell/chunk.
- Hiển thị rule result khi hover/click.
- FPS/debug stats cơ bản.

### 7. Architecture Foundation

- Unity + URP.
- VContainer `GameLifetimeScope`.
- `TownDataStore`.
- `PlacementService`.
- `RuleEvaluator`.
- `ChunkRebuilder` hoặc `TownVisualRebuilder` bản đầu.
- Struct data: `GridCoord`, `CellData`, `RuleResult`.
- Không singleton tĩnh cho service gameplay.

## Chức Năng Không Làm Trong Prototype

- Copy/paste.
- Brush xây hàng loạt.
- Online gallery.
- Daily challenge.
- Purchase/full unlock.
- Cư dân.
- Thuyền.
- Street view.
- Nhiều grid.
- Nhiều theme.
- Export 3D.
- Photo mode hoàn chỉnh.

## Data Model Đầu Tiên

Đề xuất:

```csharp
public sealed class TownData
{
    public int Version;
    public CellData[] Cells;
}
```

```csharp
public readonly struct GridCoord
{
    public readonly int X;
    public readonly int Y;
}
```

```csharp
public struct CellData
{
    public ushort Height;
    public ushort ColorId;
    public ushort MaterialId;
    public byte TerrainId;
    public CellFlags Flags;
}
```

```csharp
[Flags]
public enum CellFlags : byte
{
    None = 0,
    Occupied = 1 << 0,
    Dirty = 1 << 1,
    HasWaterfront = 1 << 2
}
```

## Services Đầu Tiên

```text
GameLifetimeScope
    TownDataStore
    PlacementService
    RuleEvaluator
    TownVisualRebuilder
    CameraService
```

## Success Criteria

Prototype đạt nếu:

- Người chơi có thể tạo cụm 10-30 nhà nhỏ trong vài phút.
- Nhìn từ camera xa, công trình có cảm giác thị trấn.
- Đặt/xóa block phản hồi ngay, không giật rõ.
- Debug view giúp hiểu vì sao rule được chọn.
- Code không phụ thuộc vào scene object làm source of truth.
- Có thể mở rộng sang undo/redo mà không viết lại data model.

## Failure Criteria

Prototype chưa đạt nếu:

- Công trình chỉ giống cube stack.
- Camera làm người chơi khó chịu.
- Mỗi lần đặt block phải rebuild toàn bộ town mà không có hướng chuyển sang chunk.
- Data nằm rải rác trong MonoBehaviour.
- Asset không thể tách module để procedural dùng.
- Visual quá giống Townscaper hoặc quá xấu để phát triển tiếp.

## Quyết Định Sau Prototype

Nếu prototype đạt:

- Chuyển sang Vertical Slice.
- Thêm undo/redo.
- Thêm save/load local.
- Thêm roof/bridge/waterfront rule tốt hơn.
- Bắt đầu art pass.

Nếu prototype chưa đạt:

- Không mở rộng content.
- Quay lại sửa camera, placement feel, grid hoặc asset foundation.

