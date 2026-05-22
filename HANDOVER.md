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

## Session Update - 2026-05-21 - Unity MCP Local Setup

Unity MCP was installed locally through the `io.realvirtual.mcp` package and the Unity toolbar shows the MCP server running.

Important architecture:

- Unity Editor side runs a WebSocket bridge, currently reported on port `18711`.
- Codex should not connect directly to port `18711`.
- Codex connects to the embedded Python MCP bridge over stdio.
- The Python bridge then connects to Unity over WebSocket.

Codex user-level config was updated:

- File: `C:/Users/Hoang.H/.codex/config.toml`
- Server: `[mcp_servers.unity]`
- Command: `Cozy_Builder/Assets/StreamingAssets/realvirtual-MCP/python/python.exe`
- Script: `Cozy_Builder/Assets/StreamingAssets/realvirtual-MCP/unity_mcp_server.py`
- Mode: `stdio`
- WebSocket port: `18711`
- `PYTHONPATH`: `Cozy_Builder/Assets/StreamingAssets/realvirtual-MCP/Lib`

Operational notes:

- Restart the Codex session/app after config changes so Unity MCP tools are loaded.
- Unity Editor must stay open with MCP server running.
- Manual MCP client test succeeded: 76 tools were listed after discovery, including 73 Unity tools.
- `Assets/.mcp_auth_token` is local/secret and should not be committed.
- `Assets/StreamingAssets/realvirtual-MCP/` contains embedded Python/runtime files and should remain local unless we explicitly decide to version it.
- `.gitignore` and `.graphifyignore` were updated to avoid committing/indexing these local MCP files.

## Session Update - 2026-05-22 - KayKit Validation And Prototype Core Data Foundation

This section is the latest handover status and overrides older "next work" notes that still say the Unity project, KayKit test scene, or code foundation have not been started.

Current baseline:

- Latest committed baseline observed in this session: `ee1392d Add Unity MCP workflow and startup context`.
- Unity Editor was open and Unity MCP connected successfully.
- Unity version in editor: `6000.3.11f1`.
- `KayKitFbxAssetTest.unity` opened successfully from `Assets/CozyBuilder/Scenes/KayKitFbxAssetTest.unity`.
- Unity compile/import completed without C# compile errors.
- `graphify update .` was run after code changes and succeeded.

KayKit validation result:

- KayKit Medieval Builder Pack 1.0 remains the first test asset pack.
- License file confirms CC0; commercial use is allowed and attribution is optional.
- FBX samples render in URP without magenta/missing-shader materials.
- KayKit is suitable for prototype terrain/grid placeholder work:
  - hex/square tiles
  - forest/rock/sand variants
  - road tiles
  - water tiles
  - water edge/corner/straight pieces
  - wall and bridge placeholders
- KayKit is not yet strong enough to treat as the final procedural building foundation:
  - building assets are mostly whole objects
  - wall/roof/floor pieces are not clearly separated for cell-based procedural architecture
  - houses work for scale/mood placeholders, not clean modular wall/roof assembly
- Decision: use KayKit for Prototype Core terrain/grid placeholders and early validation; continue searching/testing a stronger modular building foundation later.

KayKit scene changes:

- `Cozy_Builder/Assets/CozyBuilder/Scenes/KayKitFbxAssetTest.unity` now has separated asset samples instead of overlapping all at origin.
- Added simple procedural compatibility cases:
  - 1-cell house
  - 2-house row
  - 2-level stack
  - 2-wall segment row
  - hex tile sample
- Measurement notes:
  - `wall_straight` aligns cleanly at 2m spacing.
  - whole `house` objects placed at 2m spacing leave visible gap, so they need their own spacing or should remain placeholders.
  - stacked house geometry can touch at roughly 0.914m, but this does not prove it is visually suitable for stacked procedural buildings.
- Screenshot output was written under `Cozy_Builder/.screenshots/`; treat this as local validation output unless intentionally documenting screenshots.

Prototype Core code changes:

- `TownDataStore.Current` now initializes a small organic island grid with radius 4.
- `TownData` now owns:
  - `GridCoord[] Coordinates`
  - `CellData[] Cells`
  - coordinate-to-index lookup
  - `Contains`, `TryGetIndex`, `TryGetCell`, and `TrySetCell`
- Added:
  - `TerrainType`
  - `GridNeighborhood`
  - `OrganicIslandGridGenerator`
- `CellData` now stores `TerrainType Terrain` instead of a raw `byte TerrainId`.
- `PlacementService` now includes:
  - `TryPlaceBlock(GridCoord coord, ushort colorId = 0, ushort materialId = 0)`
  - `TryDeleteBlock(GridCoord coord)`
  - dirty marking for changed cell and cardinal neighbors
- `TownVisualRebuilder` now has a deduplicated dirty queue:
  - `DirtyCount`
  - `MarkDirty`
  - `TryDequeueDirty`

Current intent:

- The project has entered early Prototype Core data foundation work.
- Runtime town state must remain data-first.
- Scene objects are visual output only, not the source of truth.
- Do not build input, UI, or camera features before the data-to-visual path is proven.
- Do not turn KayKit whole building objects into long-term procedural architecture assumptions.

Next recommended work:

1. Build the first visual adapter:
   - read `TownDataStore.Current`
   - display KayKit tile placeholders for initial island cells
   - keep GameObjects as rebuild output only
2. Add a minimal MonoBehaviour runtime driver only where needed to bridge Unity lifecycle to services.
3. Add click/tap placement and delete mode against `PlacementService`.
4. Add a basic palette using `ColorId`/`MaterialId`; avoid runtime material instances.
5. Add debug views for:
   - cell coordinates
   - neighbor state
   - dirty queue
   - rule result preview
6. Add camera orbit/pan/zoom after placement/visual loop is visible.

Dirty/uncommitted state notes:

- Expected modified files include runtime data/service code, `KayKitFbxAssetTest.unity`, and `graphify-out/`.
- Expected new code files include:
  - `GridNeighborhood.cs`
  - `OrganicIslandGridGenerator.cs`
  - `TerrainType.cs`
- Unity generated `.meta` files for new C# files should be included if committing those files.
- `Cozy_Builder/Packages/io.realvirtual.mcp/` is local MCP tooling and should not be committed unless project policy changes.
- `Cozy_Builder/.screenshots/` is local validation output unless explicitly chosen for docs.
- `Cozy_Builder/ProjectSettings/SceneTemplateSettings.json` appeared as untracked after Unity activity; inspect before deciding whether it belongs in version control.

## Session Update - 2026-05-22 - First Visual Adapter Validation

This section records the next Prototype Core step after commit `c45f758 Start prototype core data foundation`.

Implemented but not yet committed:

- Added first data-to-visual adapter:
  - `Cozy_Builder/Assets/CozyBuilder/Runtime/Town/Rendering/TownGridView.cs`
  - `TownGridView.cs.meta`
- Updated composition root:
  - `Cozy_Builder/Assets/CozyBuilder/Runtime/Bootstrap/GameLifetimeScope.cs`
  - It now registers `TownGridView` from the scene hierarchy with VContainer.
- Updated test scene:
  - `Cozy_Builder/Assets/CozyBuilder/Scenes/KayKitFbxAssetTest.unity`
  - Added root object `Town Grid View`
  - Wired `cellPrefab` to KayKit `hex_forest`
  - Used `cellSpacing` of `2.1`
  - Enabled `rebuildOnStart`
- Refreshed Graphify output with `graphify update .`.

Validation:

- Unity Editor was open and MCP connected.
- Unity compile completed without C# compile errors.
- Entered Play Mode in `KayKitFbxAssetTest`.
- `TownGridView` generated an island grid from `TownDataStore.Current`.
- Runtime generated children appeared under `Town Grid View/Generated Town Cells`.
- After exiting Play Mode, generated runtime cells did not persist in the scene.
- Screenshot output:
  - `Cozy_Builder/.screenshots/scene_20260522_112322.png`
- `graphify update .` succeeded after the code changes:
  - 81 nodes
  - 80 edges
  - 14 communities

Known open issue:

- Unity/editor console still prints repeated assertion errors:
  - `Assertion failed on expression: 'IsNormalized(dir, 0.0001f)'`
- The assertion currently has no stack trace pointing to project gameplay code.
- Treat it as unresolved editor/scene-view noise unless later evidence links it to a project script or asset.

Current uncommitted state expected after this update:

- Modified:
  - `CURRENT_STATUS.md`
  - `HANDOVER.md`
  - `Cozy_Builder/Assets/CozyBuilder/Runtime/Bootstrap/GameLifetimeScope.cs`
  - `Cozy_Builder/Assets/CozyBuilder/Scenes/KayKitFbxAssetTest.unity`
  - `graphify-out/GRAPH_REPORT.md`
  - `graphify-out/graph.html`
  - `graphify-out/graph.json`
- New:
  - `Cozy_Builder/Assets/CozyBuilder/Runtime/Town/Rendering/TownGridView.cs`
  - `Cozy_Builder/Assets/CozyBuilder/Runtime/Town/Rendering/TownGridView.cs.meta`
- Local/untracked, do not commit by default:
  - `Cozy_Builder/.screenshots/`
  - `Cozy_Builder/Packages/io.realvirtual.mcp/`
- Inspect before deciding:
  - `Cozy_Builder/ProjectSettings/SceneTemplateSettings.json`

Next recommended work:

1. Connect `PlacementService` to `TownGridView` so place/delete updates visual dirty cells.
2. Avoid whole-town rebuilds for gameplay edits; move toward dirty-cell, pooled, or chunk-friendly updates.
3. Add click/tap placement and delete mode after the dirty visual update loop works.
4. Add palette support using `ColorId`/`MaterialId`, not runtime material instances.
5. Add debug visibility for cell coordinates, neighbor state, dirty queue, and rule results.

Commit policy reminder:

- Do not include `Cozy_Builder/Assets/Packages` in new commits unless the user explicitly changes this policy.
- Do not commit local MCP runtime or secrets:
  - `Cozy_Builder/Packages/io.realvirtual.mcp/`
  - `Cozy_Builder/Assets/.mcp_auth_token`

## Session Update - 2026-05-22 - Dirty Cell Visual Update Loop

This section records the follow-up step after the first `TownGridView` validation.

Implemented but not yet committed:

- Updated `TownGridView`:
  - injects `TownVisualRebuilder`
  - drains dirty cells in `LateUpdate`
  - exposes `ProcessDirtyCells(int maxCells)`
  - exposes `RefreshCell(GridCoord coord)`
  - refreshes only affected cell views instead of rebuilding the whole island
  - uses placeholder height visualization by moving a cell view upward by `blockHeightStep` per cell height
- Added temporary validation driver:
  - `Cozy_Builder/Assets/CozyBuilder/Runtime/Town/Placement/PrototypePlacementDebugDriver.cs`
  - calls `PlacementService.TryPlaceBlock` and `PlacementService.TryDeleteBlock`
  - exists only to validate placement/delete through MCP/manual inspector calls before tap/click input is implemented
- Updated `GameLifetimeScope`:
  - registers `TownGridView`
  - registers `PrototypePlacementDebugDriver`
- Updated `KayKitFbxAssetTest.unity`:
  - `Town Grid View` now has `TownGridView` and `PrototypePlacementDebugDriver`

Validation:

- Unity compile/reload completed without C# compile errors.
- Play Mode was entered in `KayKitFbxAssetTest`.
- Invoked `PrototypePlacementDebugDriver.PlaceDebugBlock` through Unity MCP:
  - returned `True`
  - `Cell 0,0` became `Cell 0,0 H1`
  - transform local Y became `0.35`
- Invoked `PrototypePlacementDebugDriver.DeleteDebugBlock` through Unity MCP:
  - returned `True`
  - `Cell 0,0` became `Cell 0,0 H0`
  - transform local Y returned to `0`
- After exiting Play Mode, runtime generated cell children did not persist in the scene.
- `Town Grid View` persists with components:
  - `Transform`
  - `TownGridView`
  - `PrototypePlacementDebugDriver`
- `graphify update .` succeeded:
  - 93 nodes
  - 101 edges
  - 15 communities

Known open issue:

- Unity/editor still logs repeated assertion errors:
  - `Assertion failed on expression: 'IsNormalized(dir, 0.0001f)'`
- No current stack trace links this to project gameplay code.

Next recommended work:

1. Add real tap/click placement and delete input against `PlacementService`.
2. Replace placeholder height-offset tile visuals with a better pooled/chunk-friendly block visual path.
3. Add palette support using `ColorId`/`MaterialId`.
4. Add debug overlay for selected cell, neighbors, dirty queue, and rule result.

## Session Update - 2026-05-22 - Prototype Click/Tap Placement Input

This section records the step after commit `4ff5d15 Add prototype town visual update loop`.

Committed baseline before this step:

- `4ff5d15 Add prototype town visual update loop`

Implemented but not yet committed:

- Added first input adapter:
  - `Cozy_Builder/Assets/CozyBuilder/Runtime/Town/Placement/PrototypePlacementInputDriver.cs`
  - converts mouse/touch screen position to a ray
  - intersects the ray with the grid plane
  - asks `TownGridView` to map world position to an existing `GridCoord`
  - calls `PlacementService.TryPlaceBlock` or `TryDeleteBlock`
- Updated `TownGridView`:
  - added `TryGetCoordFromWorld(Vector3 worldPosition, out GridCoord coord)`
- Updated `GameLifetimeScope`:
  - registers `PrototypePlacementInputDriver`
- Updated `KayKitFbxAssetTest.unity`:
  - `Town Grid View` now has `TownGridView`, `PrototypePlacementDebugDriver`, and `PrototypePlacementInputDriver`
- Refreshed Graphify output with `graphify update .`.

Validation:

- Unity compile/reload completed without C# compile errors after fixing `UnityEngine.Camera` aliasing.
- Entered Play Mode in `KayKitFbxAssetTest`.
- Invoked `PrototypePlacementInputDriver.PlaceScreenCenter` through Unity MCP:
  - returned `True`
  - screen center mapped to `Cell 0,1`
  - `Cell 0,1` became `Cell 0,1 H1`
- Invoked `PrototypePlacementInputDriver.DeleteScreenCenter` through Unity MCP:
  - returned `True`
  - searching for `H1` after delete returned zero objects
- Exited Play Mode.
- `Town Grid View` persisted with components:
  - `Transform`
  - `TownGridView`
  - `PrototypePlacementDebugDriver`
  - `PrototypePlacementInputDriver`
- `graphify update .` succeeded:
  - 108 nodes
  - 125 edges
  - 16 communities

Known open issue:

- Unity/editor still logs repeated assertion errors:
  - `Assertion failed on expression: 'IsNormalized(dir, 0.0001f)'`
- No current stack trace links this to project gameplay code.

Next recommended work:

1. Replace placeholder height-offset tile visuals with a better pooled/chunk-friendly block visual path.
2. Add a visible delete/place mode and palette control for `ColorId`/`MaterialId`.
3. Add debug overlay for selected cell, neighbors, dirty queue, and rule result.
4. Add camera orbit/pan/zoom.

## Session Update - 2026-05-22 - Pooled Prototype Block Visuals

This section records the step after commit `1efc7c3 Add prototype placement input`.

Committed baseline before this step:

- `1efc7c3 Add prototype placement input`

Implemented but not yet committed:

- Updated `TownGridView`:
  - added optional `blockPrefab`
  - added separate generated child roots:
    - `Terrain Cells`
    - `Block Cells`
  - keeps terrain tile views at ground level
  - creates block views separately from terrain tiles
  - pools block views per cell by keeping deleted blocks inactive
  - no longer visualizes height by moving the terrain tile itself upward
- Updated `KayKitFbxAssetTest.unity`:
  - added inactive scene object `Prototype Block Source`
  - assigned it to `TownGridView.blockPrefab`
  - set `blockHeightStep` to `0.38`
  - set `blockScale` to `(0.75, 0.35, 0.75)`
- Refreshed Graphify output with `graphify update .`.

Validation:

- Unity compile/reload completed without C# compile errors.
- Entered Play Mode in `KayKitFbxAssetTest`.
- Invoked `PrototypePlacementInputDriver.PlaceScreenCenter` through Unity MCP:
  - returned `True`
  - created/activated `Block 0,1 L1`
  - block path was `Town Grid View/Generated Town Cells/Block Cells/Block 0,1 L1`
  - block local position was `(0, 0.38, 2.1)`
  - block local scale was `(0.75, 0.35, 0.75)`
- Verified terrain stayed separate:
  - `Cell 0,1` path was `Town Grid View/Generated Town Cells/Terrain Cells/Cell 0,1`
  - terrain local position stayed `(0, 0, 2.1)`
- Invoked `PrototypePlacementInputDriver.DeleteScreenCenter` through Unity MCP:
  - returned `True`
  - `Block 0,1 L1` remained present but inactive
  - terrain tile stayed active at local Y `0`
- Exited Play Mode.
- Runtime generated terrain/block children did not persist in the saved scene.
- Scene persists only the configured source object:
  - `Prototype Block Source`, inactive
- `graphify update .` succeeded:
  - 115 nodes
  - 140 edges
  - 17 communities

Known open issue:

- Unity/editor still logs repeated assertion errors:
  - `Assertion failed on expression: 'IsNormalized(dir, 0.0001f)'`
- No current stack trace links this to project gameplay code.

Next recommended work:

1. Add visible place/delete mode controls and a basic palette using `ColorId`/`MaterialId`.
2. Add debug overlay for selected cell, neighbors, dirty queue, and rule result.
3. Add camera orbit/pan/zoom.

## Session Update - 2026-05-22 - Prototype Mode And Palette Controls

This section records the step after commit `37f43e3 Add pooled prototype block visuals`.

Committed baseline before this step:

- `37f43e3 Add pooled prototype block visuals`

Implemented but not yet committed:

- Added prototype placement state:
  - `Cozy_Builder/Assets/CozyBuilder/Runtime/Town/Placement/PrototypePlacementState.cs`
  - `Cozy_Builder/Assets/CozyBuilder/Runtime/Town/Placement/PrototypePlacementMode.cs`
  - Tracks current Place/Delete mode, `CurrentColorId`, and `CurrentMaterialId`.
- Added minimal prototype controls:
  - `Cozy_Builder/Assets/CozyBuilder/Runtime/Town/Placement/PrototypePlacementControlsView.cs`
  - Uses IMGUI/`OnGUI` for a temporary prototype panel.
  - Shows current mode/color/material.
  - Provides Place/Delete buttons and Color/Material id buttons `0..3`.
- Updated `PrototypePlacementInputDriver`:
  - reads `PrototypePlacementState` instead of serialized `deleteMode`, `colorId`, and `materialId`
  - switched from legacy `UnityEngine.Input` to Unity Input System (`Mouse.current` and `Touchscreen.current`)
- Updated runtime assembly:
  - `Cozy_Builder/Assets/CozyBuilder/Runtime/CozyBuilder.Runtime.asmdef`
  - added `Unity.InputSystem` reference.
- Updated `GameLifetimeScope`:
  - registers `PrototypePlacementState` as a singleton
  - registers `PrototypePlacementControlsView` from the scene hierarchy
- Updated `KayKitFbxAssetTest.unity`:
  - `Town Grid View` now has `PrototypePlacementControlsView`
- Refreshed Graphify output with `graphify update .`.

Validation:

- Unity compile/reload completed without C# compile errors.
- Initial Play Mode validation exposed a legacy input exception because active input handling is set to the Input System package.
- After switching `PrototypePlacementInputDriver` to Unity Input System, Play Mode validation succeeded:
  - `PrototypePlacementInputDriver.PlaceScreenCenter` returned `True`
  - `PrototypePlacementInputDriver.DeleteScreenCenter` returned `True`
- `git diff --check` passed.
- `graphify update .` succeeded:
  - 132 nodes
  - 159 edges
  - 20 communities

UI/EventSystem note:

- The current controls use IMGUI, so no `EventSystem` is required for the prototype panel.
- A future uGUI/UI Toolkit control surface should add proper UI event routing and block world placement while the pointer is over UI.

Current uncommitted state expected after this update:

- Modified:
  - `CURRENT_STATUS.md`
  - `HANDOVER.md`
  - `Cozy_Builder/Assets/CozyBuilder/Runtime/Bootstrap/GameLifetimeScope.cs`
  - `Cozy_Builder/Assets/CozyBuilder/Runtime/CozyBuilder.Runtime.asmdef`
  - `Cozy_Builder/Assets/CozyBuilder/Runtime/Town/Placement/PrototypePlacementInputDriver.cs`
  - `Cozy_Builder/Assets/CozyBuilder/Scenes/KayKitFbxAssetTest.unity`
  - `graphify-out/GRAPH_REPORT.md`
  - `graphify-out/graph.html`
  - `graphify-out/graph.json`
- New:
  - `Cozy_Builder/Assets/CozyBuilder/Runtime/Town/Placement/PrototypePlacementControlsView.cs`
  - `Cozy_Builder/Assets/CozyBuilder/Runtime/Town/Placement/PrototypePlacementControlsView.cs.meta`
  - `Cozy_Builder/Assets/CozyBuilder/Runtime/Town/Placement/PrototypePlacementMode.cs`
  - `Cozy_Builder/Assets/CozyBuilder/Runtime/Town/Placement/PrototypePlacementMode.cs.meta`
  - `Cozy_Builder/Assets/CozyBuilder/Runtime/Town/Placement/PrototypePlacementState.cs`
  - `Cozy_Builder/Assets/CozyBuilder/Runtime/Town/Placement/PrototypePlacementState.cs.meta`
- Local/untracked, do not commit by default:
  - `Cozy_Builder/.screenshots/`
  - `Cozy_Builder/Assets/.screenshots/`
  - `Cozy_Builder/Packages/io.realvirtual.mcp/`
- Inspect before deciding:
  - `Cozy_Builder/ProjectSettings/SceneTemplateSettings.json`

Next recommended work:

1. Add minimal procedural rule/debug views:
   - selected cell id and neighbor info
   - dirty cell queue
   - rule result preview
2. Add camera orbit/pan/zoom after debug visibility is in place.
