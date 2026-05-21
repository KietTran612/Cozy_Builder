# Unity URP Performance Code Rules

## Mục Đích

Tài liệu này đặt ra các rule code bắt buộc để tối ưu performance ngay từ đầu khi làm game bằng Unity + URP. Với game cozy procedural town builder, performance không chỉ là FPS. Nó ảnh hưởng trực tiếp đến cảm giác thư giãn: đặt block phải mượt, camera phải nhẹ, town lớn không được giật, save/load không được làm người chơi mất niềm tin.

Các rule này cần được áp dụng từ prototype, không đợi đến cuối dự án.

Tài liệu liên quan:

- `Architecture_And_Code_Rules.md`: chốt rule dùng VContainer, DIP, struct cho data và UniTask.

## Target Kỹ Thuật Ban Đầu

Mục tiêu đề xuất:

- 60 FPS trên thiết bị tầm trung nếu có thể.
- Tối thiểu 30 FPS ổn định trên thiết bị yếu trong target.
- Không spike frame rõ rệt khi đặt/xóa một block.
- Không mất save.
- Không làm nóng máy quá nhanh trong phiên chơi 10-20 phút.
- Town vừa/lớn vẫn xoay camera được mượt.

Các con số cụ thể sẽ được chốt lại sau khi chọn target device.

## Rule 1: Data Trước, Visual Sau

Town phải được lưu bằng dữ liệu logic, không phụ thuộc trực tiếp vào GameObject trong scene.

Nên có:

- TownData.
- CellData.
- GridCoordinate.
- Height/layer.
- Color/material id.
- Terrain id.
- Rule result/cache nếu cần.

Không nên:

- Dùng scene hierarchy làm source of truth.
- Lưu trạng thái quan trọng trong MonoBehaviour rải rác.
- Để procedural system đọc ngược visual object để hiểu town.

Lý do:

- Dễ save/load.
- Dễ undo/redo.
- Dễ rebuild chunk.
- Dễ test procedural rule.

## Rule 2: Không Rebuild Toàn Bộ Town Khi Chỉ Đổi Một Cell

Khi người chơi đặt/xóa một block, chỉ các vùng liên quan được cập nhật.

Nên làm:

- Chia town thành chunk.
- Khi cell thay đổi, mark dirty cell và neighbor cần tính lại.
- Rebuild mesh/props cho chunk bị ảnh hưởng.
- Có dirty queue để xử lý theo thứ tự.

Không nên:

- Clear toàn bộ town rồi sinh lại.
- Recalculate mọi roof/wall/prop mỗi lần tap.
- Rebuild mesh toàn scene nếu chỉ một cell thay đổi.

Lý do:

- Town càng lớn, rebuild toàn bộ sẽ tạo spike.
- Game cần cảm giác đặt block tức thì.

## Rule 3: Pooling Cho Object Sinh/Xóa Thường Xuyên

Không Instantiate/Destroy liên tục trong gameplay path.

Dùng pooling cho:

- Preview block.
- Highlight cell.
- Small props.
- Ambient characters.
- Birds/boats nếu có.
- VFX đặt block.
- UI markers.

Không cần pooling cho:

- Object tạo một lần khi load scene.
- Asset không xuất hiện/xóa thường xuyên.

Rule:

- Không gọi Instantiate/Destroy trong mỗi tap nếu có thể tránh.
- Nếu cần tạo object runtime nhiều lần, phải có pool.

## Rule 4: Hạn Chế GameObject, Ưu Tiên Mesh/Batch Theo Chunk

Không biến mỗi mảng tường, mái, cửa, đá, cỏ thành một GameObject riêng nếu số lượng lớn.

Nên làm:

- Mesh combine theo chunk.
- GPU instancing cho props lặp lại.
- Static batching cho object tĩnh nếu phù hợp.
- Dùng shared mesh/shared material.

Không nên:

- Hàng nghìn GameObject active cho từng chi tiết nhỏ.
- Mỗi cửa sổ là một object riêng nếu có thể bake/combine.
- Mỗi block có nhiều child object không cần thiết.

## Rule 5: Material Phải Được Kiểm Soát Chặt

Material count là một rủi ro lớn trên mobile.

Rule:

- Dùng shared material.
- Không gọi renderer.material trong runtime nếu không thật sự cần, vì Unity có thể tạo material instance.
- Dùng MaterialPropertyBlock cho thay đổi màu theo instance nếu phù hợp.
- Gom palette vào một hệ thống thống nhất.
- Tránh shader custom phức tạp trong MVP.

Nên ưu tiên:

- Ít material.
- Shader URP đơn giản.
- Flat/stylized color.
- Texture atlas nếu asset pack hỗ trợ.

## Rule 6: Allocation-Free Cho Update Path Quan Trọng

Các hàm chạy thường xuyên không được tạo garbage không cần thiết.

Áp dụng cho:

- Camera update.
- Input handling.
- Cell hover/selection.
- Preview placement.
- Dirty queue processing.
- Ambient movement.

Tránh:

- LINQ trong Update.
- foreach trên collection có thể allocate trong hot path.
- Tạo List/Dictionary mới mỗi frame.
- String concat/log liên tục.
- GetComponent lặp lại mỗi frame.

Nên:

- Cache component.
- Reuse collections.
- Dùng profiler để kiểm tra GC Alloc.
- Tắt debug log trong build production.

## Rule 7: Camera Và Input Phải Nhẹ

Camera là phần người chơi dùng liên tục.

Rule:

- Không raycast quá nhiều lần mỗi frame nếu không cần.
- Raycast cell selection nên gom vào một service rõ ràng.
- Camera smoothing không tạo allocation.
- Camera pivot cần ổn định quanh điểm thao tác.
- Input system phải tách khỏi gameplay command để dễ undo/redo.

Nên có:

- BuildCommand.
- DeleteCommand.
- UndoStack.
- CameraController riêng.
- PlacementController riêng.

## Rule 8: Procedural Rule Phải Test Được

Procedural generation không nên chỉ là logic ẩn trong MonoBehaviour.

Nên:

- Tách rule thành class/service thuần C# nếu có thể.
- Input là CellData/neighbors.
- Output là RuleResult.
- Có test scene để xem từng rule.
- Có debug overlay hiển thị rule nào được chọn.

Lý do:

- Dễ debug.
- Dễ tối ưu.
- Dễ tránh bug dây chuyền khi thêm rule mới.

## Rule 9: Ambient Life Phải Rẻ

Cư dân, chim, thuyền làm town sống hơn nhưng không được biến thành simulation nặng.

Rule:

- Cư dân chỉ là ambient, không phải AI phức tạp.
- Dùng path đơn giản.
- Giới hạn số lượng active.
- Dùng LOD hoặc disable animation khi xa camera.
- Không pathfind lại liên tục.
- Spawn/despawn qua pool.

Không nên:

- Mỗi cư dân có behavior tree phức tạp.
- Mỗi frame tính route mới.
- Physics collider phức tạp cho từng cư dân.

## Rule 10: Save/Load Và Undo Phải Tối Ưu Từ Đầu

Undo/redo là chức năng cốt lõi vì giúp người chơi không sợ sai.

Rule:

- Undo lưu command/delta, không copy toàn bộ town nếu town lớn.
- Save format có version.
- Save dữ liệu logic, không save scene object.
- Có autosave an toàn.
- Save/load không block frame quá lâu nếu dữ liệu lớn.

Nên có:

- BuildCommand: cell, previous state, new state.
- DeleteCommand.
- Batch command chỉ dùng sau này nếu có brush/copy.
- Save migration theo version.

## Rule 11: URP Settings Cho Mobile Phải Được Khóa Sớm

Không để mỗi scene/asset tự quyết định chất lượng render.

Cần chốt:

- URP asset cho mobile.
- Shadow distance.
- Shadow resolution.
- MSAA.
- Render scale.
- Post-processing nào được phép.
- Light count.
- Reflection usage.

Khuyến nghị MVP:

- Ít hoặc không dùng realtime shadow nặng.
- Post-processing rất nhẹ.
- Dùng baked/ambient/simple lighting nếu phù hợp.
- Nước stylized nhẹ, không shader phức tạp.

## Rule 12: Profiler Là Một Phần Của Workflow

Không tối ưu bằng cảm giác đoán.

Phải dùng:

- Unity Profiler.
- Frame Debugger.
- Memory Profiler nếu cần.
- Build thật trên device, không chỉ test Editor.

Mỗi milestone cần kiểm tra:

- FPS.
- GC Alloc.
- Draw calls.
- Batches.
- Memory.
- Frame spike khi đặt/xóa block.
- Load/save time.

## Rule 13: Asset Import Phải Có Chuẩn

Asset từ store/free source phải được kiểm tra trước khi dùng production.

Rule:

- Scale chuẩn.
- Pivot chuẩn.
- Mesh readable chỉ bật khi thật sự cần.
- Texture size giới hạn.
- Material remap về shader/material chuẩn của project.
- Prefab naming rõ.
- Không để asset demo scene kéo theo resource thừa vào build.

## Rule 14: Không Thêm Feature Nếu Chưa Có Budget

Mỗi feature mới phải có budget:

- CPU.
- GPU.
- Memory.
- UI complexity.
- Save data.
- QA cost.

Nếu không biết cost, feature đó phải được prototype riêng trước khi nhập vào main.

## Checklist Code Review

Trước khi merge code gameplay/procedural/rendering, kiểm tra:

- Có tạo garbage trong Update không?
- Có Instantiate/Destroy trong gameplay path không?
- Có renderer.material runtime không?
- Có rebuild quá nhiều chunk không?
- Có tách data khỏi visual không?
- Có thể undo/redo thao tác này không?
- Có thể save/load dữ liệu này không?
- Có debug/profiler path để kiểm tra không?
- Có chạy được trên device thật không?

## Kết Luận

Performance không phải là bước cuối. Với game này, performance là một phần của cảm giác cozy. Đặt block mà giật, camera khó xoay, town lớn bị lag thì trải nghiệm thư giãn sẽ mất.

Vì vậy, ngay sau khi chọn Unity + URP, dự án phải áp dụng code rules này như tiêu chuẩn kỹ thuật nền tảng.
