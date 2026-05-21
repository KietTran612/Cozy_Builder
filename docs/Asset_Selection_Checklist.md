# Asset Selection Checklist

## Mục Đích

Checklist này dùng để đánh giá asset pack trước khi đưa vào prototype hoặc production. Vì dự án chưa có artist riêng, asset pack phải được chọn rất kỹ: đẹp thôi chưa đủ, nó phải hợp license, hợp mobile, hợp procedural generation và dễ giữ style nhất quán.

## Quyết Định Nền

- Engine: Unity.
- Render pipeline: URP.
- Style ưu tiên: low-poly stylized, cozy, sáng, dễ đọc.
- Target chính: mobile trước.
- Asset pack chính: tối đa 1-2 pack trong MVP.
- Asset phụ: chỉ dùng nếu chỉnh được về cùng style.

## Thông Tin Cơ Bản Cần Ghi Lại

Với mỗi asset pack, ghi:

- Tên asset.
- Link nguồn.
- Creator/publisher.
- Giá.
- Ngày xem/tải/mua.
- License/EULA.
- Có commercial use không.
- Có modify được không.
- Có cần attribution không.
- Có hỗ trợ Unity URP không.
- Có demo scene không.
- Có mobile performance note không.

## Checklist License

Không dùng asset nếu câu trả lời không rõ.

| Câu hỏi | Đạt/Không | Ghi chú |
| --- | --- | --- |
| Có cho commercial use không? |  |  |
| Có cho mobile game không? |  |  |
| Có cho modify không? |  |  |
| Có cần attribution không? |  |  |
| Có cấm redistribute standalone không? |  |  |
| Nguồn tải có phải nguồn chính thức không? |  |  |
| Có chứng từ mua/tải không? |  |  |
| License có được lưu lại vào project docs không? |  |  |

Rule:

- Không dùng asset từ site reupload.
- Không dùng asset "personal use only".
- Không dùng asset AI không rõ quyền.
- Không dùng asset nếu license không lưu lại được.

## Checklist Visual Style

| Câu hỏi | Đạt/Không | Ghi chú |
| --- | --- | --- |
| Có cảm giác cozy không? |  |  |
| Có quá realistic không? |  |  |
| Có quá dark/medieval nặng không? |  |  |
| Màu sắc có hợp thị trấn thư giãn không? |  |  |
| Silhouette có khác Townscaper không? |  |  |
| Có dễ nhìn ở màn hình điện thoại không? |  |  |
| Có thể dùng làm screenshot store không? |  |  |
| Có thể phối với nước/cỏ/đường đá không? |  |  |

Rule:

- Không chọn asset chỉ vì demo scene đẹp.
- Phải xem từng module riêng khi tách khỏi demo.
- Style càng đơn giản càng dễ mở rộng khi không có artist.

## Checklist Modularity

Asset pack phải phục vụ procedural generation.

| Câu hỏi | Đạt/Không | Ghi chú |
| --- | --- | --- |
| Có tường riêng không? |  |  |
| Có mái riêng không? |  |  |
| Có cửa/cửa sổ riêng hoặc dễ bake không? |  |  |
| Có góc/corner piece không? |  |  |
| Có bridge/arch/stairs không? |  |  |
| Có ground/foundation piece không? |  |  |
| Có waterfront/dock piece không? |  |  |
| Mesh có pivot hợp lý không? |  |  |
| Scale giữa các object có nhất quán không? |  |  |
| Có thể stack tầng không? |  |  |

Rule:

- Nếu asset chỉ là nhà nguyên khối, không hợp làm foundation procedural.
- Asset phải hỗ trợ ít nhất wall/roof/foundation tách rời hoặc dễ chỉnh trong Blender.

## Checklist Technical

| Câu hỏi | Đạt/Không | Ghi chú |
| --- | --- | --- |
| Import vào URP có lỗi material không? |  |  |
| Có quá nhiều material không? |  |  |
| Có quá nhiều texture riêng không? |  |  |
| Poly count có phù hợp mobile không? |  |  |
| Texture size có hợp lý không? |  |  |
| Có shader custom nặng không? |  |  |
| Có animation/rig không cần thiết không? |  |  |
| Có dependency package khác không? |  |  |
| Có demo content thừa dễ loại bỏ không? |  |  |

Rule:

- Ưu tiên shared material.
- Ưu tiên flat/stylized color.
- Tránh shader custom nếu chưa hiểu cost.
- Không đưa demo scene/resource thừa vào build.

## Checklist Procedural Compatibility Test

Trước khi chọn asset pack chính, phải tạo test scene với các case:

- 1 cell đơn.
- 2 cell cạnh nhau.
- Dãy 5 cell.
- Stack 2 tầng.
- Stack 4 tầng.
- Góc 90 độ.
- Góc organic/lệch nếu grid hỗ trợ.
- Vòng kín tạo sân.
- Gần nước tạo waterfront.
- Cầu/vòm giữa hai cụm.
- Mái khi nhà đứng một mình.
- Mái khi nhiều nhà sát nhau.

Đạt nếu:

- Nhìn ra thị trấn chỉ với vài cell.
- Rule procedural có thể chọn mesh hợp lý.
- Không cần sửa tay quá nhiều.
- Camera nhìn gần không bị sai scale quá rõ.

## Checklist Performance Test

Test trên scene mẫu:

- 50 blocks.
- 200 blocks.
- 500 blocks.
- 1000 blocks nếu target cho phép.

Đo:

- FPS.
- Draw calls.
- Batches.
- Memory.
- GC Alloc.
- Frame spike khi đặt/xóa block.
- Material count.
- Mesh count.
- GameObject count.

Rule:

- Nếu 200-500 blocks đã quá nặng, asset không phù hợp mobile hoặc cần pipeline combine/batch mạnh hơn.
- Nếu material count quá cao, cần remap material trước khi production.

## Scoring

Chấm mỗi asset pack theo thang 1-5.

| Nhóm | Điểm | Ghi chú |
| --- | --- | --- |
| License |  |  |
| Visual style |  |  |
| Modularity |  |  |
| URP compatibility |  |  |
| Mobile performance |  |  |
| Procedural compatibility |  |  |
| Ease of editing in Blender |  |  |
| Difference from Townscaper |  |  |

Quyết định:

- Tổng dưới 25: không dùng làm foundation.
- 25-32: chỉ dùng prototype hoặc asset phụ.
- 33-40: có thể dùng làm foundation nếu license rõ.

## Asset Candidate Template

```text
Asset name:
Source link:
Publisher:
Price:
License:
Commercial use:
Modify allowed:
Attribution:
URP support:
Style notes:
Modularity notes:
Performance notes:
Procedural compatibility:
Risks:
Decision:
```

## Quyết Định Sau Checklist

Mỗi asset chỉ có một trong các trạng thái:

- `Reject`: không dùng.
- `Prototype Only`: dùng thử, không ship.
- `Secondary Asset`: dùng phụ sau khi chỉnh style.
- `Foundation Candidate`: có thể làm asset pack chính.
- `Approved Foundation`: được phép xây prototype chính quanh asset này.

