# HANDOVER

## Mục Đích

File này là tài liệu bàn giao tổng quát cho người mới tham gia dự án. Đọc file này trước để hiểu:

- Chúng ta đang làm game gì.
- Vì sao chọn hướng này.
- Những quyết định đã chốt.
- Nên đọc các tài liệu theo thứ tự nào.
- Dự án đang ở giai đoạn nào.
- Việc tiếp theo cần làm là gì.

## Tổng Quan Dự Án

Chúng ta đang thiết kế một game mobile/Unity thuộc nhóm **cozy procedural town builder**.

Ý tưởng cốt lõi:

- Người chơi đặt/xóa block để tạo thị trấn nhỏ.
- Game không có áp lực thắng thua, không tài nguyên phức tạp.
- Hệ thống procedural tự biến block thành nhà, mái, tháp, cầu, vòm, sân hoặc chi tiết phù hợp.
- Trọng tâm là cảm giác thư giãn, thị trấn đẹp, thao tác dễ chịu, camera tốt và visual đủ hấp dẫn để chia sẻ.

Game lấy cảm hứng nghiên cứu từ Townscaper, nhưng không muốn chỉ clone Townscaper. Điểm khác biệt dự kiến:

- Có chiến lược terrain rõ hơn.
- Có grid/seed có kiểm soát hơn.
- Có ambient life như cư dân nhỏ, chim, đèn, thuyền.
- Có định hướng street view/walk view sau này.
- Tối ưu mobile/performance ngay từ đầu.
- Có kế hoạch asset rõ ràng vì hiện chưa có artist riêng.

## Trạng Thái Hiện Tại

Dự án đang ở **Giai Đoạn 0: Pre-Production**.

Đã làm:

- Nghiên cứu Townscaper.
- Viết gameplay direction cho game của chúng ta.
- Viết roadmap phát triển app.
- Viết kế hoạch launch/monetization.
- Chốt Unity + URP là hướng engine/render pipeline.
- Chốt dùng VContainer ngay từ đầu.
- Chốt áp dụng DIP, struct cho data compact, UniTask cho async workflow.
- Viết rule performance/code architecture.
- Viết product vision one-page.
- Viết checklist chọn asset.
- Viết shortlist asset pack đầu tiên.
- Viết scope prototype core.

Chưa làm:

- Chưa setup Unity project.
- Chưa chọn asset pack chính.
- Chưa import/test asset trong Unity URP.
- Chưa viết prototype placement/grid.
- Chưa có code gameplay.

## Thứ Tự Đọc Tài Liệu

### 1. Hiểu Sản Phẩm

Đọc trước:

- [Product_Vision_One_Page.md](D:/soflware/Unity/Source/App/docs/Product_Vision_One_Page.md)

Mục đích:

- Hiểu game là gì.
- Người chơi mục tiêu là ai.
- Core fantasy là gì.
- MVP cần chứng minh điều gì.
- Điều gì không làm trong MVP.

### 2. Hiểu Nguồn Cảm Hứng Và Vấn Đề Người Dùng

Đọc:

- [Townscaper_Gameplay_Research.md](D:/soflware/Unity/Source/App/docs/Townscaper_Gameplay_Research.md)

Mục đích:

- Hiểu gameplay của Townscaper.
- Hiểu chức năng hiện có.
- Hiểu người dùng thích gì, chê gì, mong muốn gì.
- Biết vì sao chúng ta chọn các cải tiến như terrain, grid, cư dân, camera/street view.

### 3. Hiểu Gameplay Của Game Chúng Ta

Đọc:

- [Our_Cozy_Procedural_Town_Builder_Gameplay.md](D:/soflware/Unity/Source/App/docs/Our_Cozy_Procedural_Town_Builder_Gameplay.md)

Mục đích:

- Hiểu "cozy procedural town builder" nghĩa là gì.
- Hiểu gameplay loop.
- Hiểu chức năng nào giống Townscaper.
- Hiểu chức năng nào mới.
- Hiểu điểm mạnh/yếu của từng nhóm chức năng.

### 4. Hiểu Roadmap Làm App

Đọc:

- [App_Development_Roadmap.md](D:/soflware/Unity/Source/App/docs/App_Development_Roadmap.md)

Mục đích:

- Hiểu lộ trình từ pre-production, prototype, vertical slice, MVP, alpha, beta, soft launch, global launch.
- Hiểu gate quyết định sau từng giai đoạn.
- Hiểu việc nào đang làm hiện tại.

### 5. Hiểu Chọn Công Cụ Và Asset

Đọc:

- [Tooling_And_Asset_Strategy.md](D:/soflware/Unity/Source/App/docs/Tooling_And_Asset_Strategy.md)
- [Asset_Selection_Checklist.md](D:/soflware/Unity/Source/App/docs/Asset_Selection_Checklist.md)
- [Asset_Pack_Shortlist.md](D:/soflware/Unity/Source/App/docs/Asset_Pack_Shortlist.md)

Mục đích:

- Hiểu vì sao asset strategy là rủi ro lớn khi chưa có artist.
- Biết tiêu chí chọn asset pack.
- Biết cách đánh giá license, style, modularity, URP compatibility, mobile performance.
- Biết 1-2 asset pack nào đang được đề xuất test import đầu tiên.

### 6. Hiểu Rule Code Và Kiến Trúc

Đọc:

- [Architecture_And_Code_Rules.md](D:/soflware/Unity/Source/App/docs/Architecture_And_Code_Rules.md)
- [Unity_URP_Performance_Code_Rules.md](D:/soflware/Unity/Source/App/docs/Unity_URP_Performance_Code_Rules.md)

Mục đích:

- Hiểu cách dùng VContainer.
- Hiểu cách áp dụng DIP.
- Hiểu khi nào dùng struct cho data.
- Hiểu khi nào dùng UniTask.
- Hiểu performance rules bắt buộc trong Unity + URP.

### 7. Hiểu Scope Prototype Đầu Tiên

Đọc:

- [Prototype_Core_Scope.md](D:/soflware/Unity/Source/App/docs/Prototype_Core_Scope.md)

Mục đích:

- Hiểu prototype đầu tiên cần làm gì.
- Hiểu prototype không làm gì.
- Hiểu data model/service đầu tiên.
- Hiểu success/failure criteria.

### 8. Hiểu Kế Hoạch Phát Hành Và Lợi Nhuận

Đọc sau cùng:

- [App_Launch_And_Monetization_Plan.md](D:/soflware/Unity/Source/App/docs/App_Launch_And_Monetization_Plan.md)

Mục đích:

- Hiểu monetization khuyến nghị.
- Hiểu vì sao không nên dùng ads bắt buộc.
- Hiểu kịch bản doanh thu.
- Hiểu kế hoạch soft launch/global launch.

## Quyết Định Đã Chốt

### Sản Phẩm

- Đây là game thư giãn, không phải city-builder quản lý tài nguyên.
- Không ưu tiên copy/paste hoặc brush xây hàng loạt trong giai đoạn đầu.
- Ưu tiên cảm giác đặt từng block, visual đẹp, camera tốt và performance mượt.
- Undo/redo quan trọng vì giúp người chơi không sợ sai.

### Công Nghệ

- Engine: Unity.
- Render pipeline: URP.
- DI framework: VContainer, dùng ngay từ đầu.
- Async workflow: UniTask.
- Data compact: dùng struct cho data nhỏ/nhiều như `CellData`, `GridCoord`, `RuleResult`.
- Architecture: áp dụng DIP thực dụng.

### Performance

- Data-first: logic town nằm trong data, không phụ thuộc scene object.
- Không rebuild toàn bộ town khi chỉ đổi một cell.
- Dùng chunk/dirty queue.
- Không `Instantiate/Destroy` liên tục trong gameplay path.
- Dùng pooling cho object tạo/xóa thường xuyên.
- Hạn chế GameObject số lượng lớn, ưu tiên mesh/batch theo chunk.
- Không tạo material runtime tùy tiện.
- Profile trên device thật, không chỉ test Editor.

### Asset

- Vì chưa có artist, cần chọn asset pack rất kỹ.
- Style ưu tiên: low-poly stylized, cozy, sáng, dễ đọc.
- Asset phải có license thương mại rõ.
- Asset phải modular để procedural generation dùng được.
- Không dùng asset từ nguồn reupload hoặc license không rõ.
- Không trộn quá nhiều pack trong MVP.

## Việc Tiếp Theo

Việc tiếp theo theo roadmap:

1. Setup Unity project với URP.
2. Import VContainer và UniTask.
3. Test import asset theo [Asset_Pack_Shortlist.md](D:/soflware/Unity/Source/App/docs/Asset_Pack_Shortlist.md):
   - KayKit Medieval Builder Pack 1.0.
   - Pandazole - City Town Lowpoly Pack để test hướng cartoon/cute miễn phí.
   - Simple Town - Cartoon Assets hoặc Simple Buildings - Cartoon City nếu muốn test cute/cartoon rõ hơn.
   - Terrific Modular Fantasy Village nếu ngân sách cho phép.
   - Medieval house modular v2.0 - lite - URP làm backup miễn phí.
4. Tạo scene test asset:
   - scale
   - material
   - URP compatibility
   - mobile FPS
   - modularity
   - procedural compatibility
5. Chọn asset foundation.
6. Bắt đầu Prototype Core theo [Prototype_Core_Scope.md](D:/soflware/Unity/Source/App/docs/Prototype_Core_Scope.md).

## Prototype Core Cần Làm Gì

Prototype đầu tiên chỉ cần:

- Grid organic island bản đầu.
- Tap/click đặt block.
- Xóa block.
- Palette 3-6 màu.
- Procedural wall/roof/tower cơ bản.
- Camera orbit/pan/zoom.
- Debug view cell/neighbor/rule.
- Data model đầu tiên: `TownData`, `CellData`, `GridCoord`.
- VContainer `GameLifetimeScope`.

Prototype không cần:

- Cư dân.
- Street view.
- Copy/paste.
- Brush xây hàng loạt.
- Online.
- Monetization.
- Nhiều theme.
- Gallery.

## Câu Hỏi Cần Trả Lời Ở Bước Kế Tiếp

- Asset pack nào đủ tốt để làm foundation?
- Visual foundation có khác Townscaper đủ rõ không?
- Asset có modular thật không, hay chỉ đẹp trong demo scene?
- Có thể dùng asset đó để tạo nhà procedural từ cell không?
- Unity URP import có ổn không?
- Performance trên mobile có khả thi không?
- Data model ban đầu đã đủ cho undo/redo/save/load chưa?

## Nguyên Tắc Khi Tiếp Tục

- Không thêm feature nếu chưa chứng minh core placement thú vị.
- Không mở rộng content nếu visual/camera/placement chưa đạt.
- Không dùng asset license không rõ.
- Không phá rule performance để làm nhanh.
- Không dùng VContainer, UniTask, struct như "trend"; chỉ dùng đúng phạm vi đã chốt.
- Mọi quyết định mới nên cập nhật lại tài liệu tương ứng.

## Session Update - 2026-05-21

This section is the latest handover status and overrides older "not yet setup" notes above.

Current project setup:

- Unity project has been created at `D:/soflware/Unity/Source/App/Cozy_Builder`.
- Unity version: `6000.3.11f1`.
- Render pipeline: URP is installed and configured.
- KayKit Medieval Builder Pack 1.0 has been imported at `Cozy_Builder/Assets/Packages/kaykit_medieval_builder_pack_1.0`.
- KayKit license checked: CC0, usable for commercial work.
- KayKit source format decision: use `fbx` only for prototype and production experiments.
- KayKit `dae` models show shader/material issues in URP and should be ignored for now.
- KayKit `obj` models render correctly, but are not the preferred Unity workflow for this project.
- UniTask is installed through Unity Package Manager. Resolved version: `2.5.11`.
- VContainer is installed through Unity Package Manager. Resolved version: `1.18.0`.

Asset workflow decision for KayKit:

- Use:
  - `Models/objects/fbx`
  - `Models/tiles/hex/fbx`
  - `Models/tiles/square/fbx`
- Avoid for the current prototype:
  - `Models/**/dae`
  - `Models/**/obj`
  - `Models/**/gltf`
  - `Models/**/glb`

Next recommended work:

1. Push commit `3b09e7a` to `origin/main`.
2. Create initial project code structure.
3. Add `GameLifetimeScope` using VContainer.
4. Create a KayKit FBX test scene for scale, material, URP compatibility, modularity, and procedural suitability.
5. Start prototype core only after the asset test scene is stable.

Git status and workflow notes:

- Current setup commit has been created locally: `3b09e7a Setup Unity project packages and handover`.
- Commit has not been pushed yet.
- `.gitignore` has been added to exclude Unity generated folders and unused KayKit duplicate formats.
- Any Git operation that writes to `.git` (`git add`, `git commit`, `git push`, config changes) should be run by the OS user that owns the repository checkout.
- If Git reports `dubious ownership` or cannot create `.git/index.lock`, verify repository ownership and Git safe-directory settings for the current device.
- After the commit, several existing docs may appear modified in `git status` due to line-ending warnings only. `git diff --stat` showed no real content diff for those docs.

Committed setup contents:

- Unity project files under `Cozy_Builder`.
- URP project settings.
- Package manifest and lockfile with UniTask and VContainer resolved.
- KayKit Medieval Builder Pack 1.0 FBX assets only.
- KayKit duplicate source formats ignored for current workflow: `dae`, `obj`, `gltf`, `glb`.
- Root-level misplaced Unity generated `.cs` files ignored.
- Asset shortlist document added at `docs/Asset_Pack_Shortlist.md`.

## Session Update - 2026-05-21 - Code Foundation Started

This section records the start of the next roadmap step: initial project/code structure, `GameLifetimeScope`, and a KayKit FBX test scene.

Added:

- Project-owned runtime folder: `Cozy_Builder/Assets/CozyBuilder/Runtime`.
- Runtime assembly definition: `Cozy_Builder/Assets/CozyBuilder/Runtime/CozyBuilder.Runtime.asmdef`.
- Assembly references:
  - `VContainer`
  - `UniTask`
- Initial VContainer composition root:
  - `Cozy_Builder/Assets/CozyBuilder/Runtime/Bootstrap/GameLifetimeScope.cs`
- Initial data/service shells aligned with `Prototype_Core_Scope.md`:
  - `GridCoord`
  - `CellFlags`
  - `CellData`
  - `RuleResult`
  - `TownData`
  - `TownDataStore`
  - `PlacementService`
  - `RuleEvaluator`
  - `TownVisualRebuilder`
  - `CameraService`
- KayKit FBX test scene:
  - `Cozy_Builder/Assets/CozyBuilder/Scenes/KayKitFbxAssetTest.unity`
  - Scene contains `Game Lifetime Scope`, camera, directional light, and KayKit FBX prefab instances for quick visual import checks.

Current intent:

- The code is foundation only, not gameplay implementation.
- No singleton static services were added.
- Data structs remain separate from scene objects.
- `GameLifetimeScope` registers only system-level prototype services.
- The KayKit scene is for scale/material/URP/modularity inspection before prototype placement work.

Verification note:

- Unity executable was not available from the current shell PATH or the default Unity Hub install path, so Unity batchmode compile/import was not run in this session.
- Open `Cozy_Builder/Assets/CozyBuilder/Scenes/KayKitFbxAssetTest.unity` in Unity and check the Console after import before building further gameplay on this foundation.

## Session Update - 2026-05-21 - Graphify Setup

Graphify has been set up for codebase navigation.

Added:

- `AGENTS.md` Graphify instructions for Codex.
- `.codex/hooks.json` with the Graphify pre-tool hook.
- `.graphifyignore` to keep Unity generated folders, imported packages, and large asset formats out of the graph.
- `graphify-out/graph.json`
- `graphify-out/graph.html`
- `graphify-out/GRAPH_REPORT.md`
- `graphify-out/.graphify_root`

Ignored local-only Graphify files:

- `graphify-out/manifest.json`
- `graphify-out/cache/`
- `graphify-out/cost.json`

Current graph state:

- Built with `graphify update .`.
- AST-only, no LLM API key required.
- Current graph summary after filtering Unity/package noise: 41 nodes, 31 edges, 10 communities.

Useful commands:

- Rebuild after code changes: `graphify update .`
- Explain a node: `graphify explain "PlacementService"`
- Query the graph: `graphify query "How does GameLifetimeScope relate to placement?"`
- Check freshness: `graphify check-update .`

Notes:

- `graphify extract . --no-cluster` currently fails without `MOONSHOT_API_KEY` or `ANTHROPIC_API_KEY`; use `graphify update .` for the no-cost AST graph.
- A broad `.graphifyignore` pattern such as `Cozy_Builder/*.cs` caused Graphify to find no code files on Windows, so root generated Unity `.cs` files are ignored by exact filename instead.

Doc workflow decision:

- Do not use paid `MOONSHOT_API_KEY` or `ANTHROPIC_API_KEY` semantic extraction for now.
- Use Graphify for code-symbol navigation and module orientation.
- Use rule-based direct doc reading for project decisions and constraints.
- `AGENTS.md` now maps common task types to the docs that should be read before coding or answering.
- Graphify may track docs in `manifest.json`, but AST-only graph output should not be treated as deep semantic understanding of Markdown docs.
- `HANDOVER.md` should be treated as current status and an index, not as an instruction to read every linked document.
- To control context size, read only the docs relevant to the current task; if unsure, search headings or targeted terms before opening full docs.
- Graphify is not fully automatic; the agent should invoke it when code graph context is useful.
- After changing code files, run `graphify update .` before the final response and report whether it succeeded.

## Session Update - 2026-05-21 - Current Status Shortcut

To reduce context use, a short startup file has been added:

- `CURRENT_STATUS.md`

Purpose:

- Give agents a compact summary of current project state.
- Preserve product direction, prototype scope, architecture rules, next work, and Graphify workflow without requiring the full `HANDOVER.md` on every session.
- Keep `HANDOVER.md` as the deeper history/index document.

Startup rule:

- Read `CURRENT_STATUS.md` first.
- Read `HANDOVER.md` only when deeper history is needed or when `CURRENT_STATUS.md` is unclear.
- Continue to read task-specific docs selectively, not all linked docs.
