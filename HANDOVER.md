# HANDOVER

> [!WARNING]
> **TỐI ƯU HÓA CONTEXT WINDOW (DÀNH CHO AI AGENT):**
> Khi lưu trữ phiên làm việc cũ từ tệp này sang `docs/Development_Session_Log.md`, **nghiêm cấm** sử dụng công cụ đọc toàn bộ tệp log. 
> Chỉ sử dụng công cụ chỉnh sửa trực tiếp (`replace_file_content`) hoặc lệnh `tail` để đối chiếu cuối tệp log.

This file is the compact handover for continuing project work. Read `CURRENT_STATUS.md` first. Read this file when `CURRENT_STATUS.md` is not enough or when a task needs a broader project map.

Older session history has been moved to `docs/Development_Session_Log.md`. Do not read that log by default; use it only when investigating why an old decision was made, checking old validation details, or reconstructing prior work.

When writing a new `Latest Session Update`, first append the previous latest session from this file to `docs/Development_Session_Log.md`. This is a routine archive step: do not read the whole log; use a targeted tail/search only if needed to avoid duplicating an entry.

## Product Summary

- Unity mobile game in the cozy procedural town builder space.
- Core fantasy: place and delete simple blocks slowly, then watch a small peaceful town take shape.
- Not a resource-management city builder.
- Inspired by Townscaper research, but must not become a direct clone.
- Long-term differentiators: clearer terrain strategy, controlled grid/seed, ambient life, possible street/walk view, mobile-first performance.

## Current Project State

- Phase: Phase 0 / early Prototype Core foundation.
- Unity project root: `Cozy_Builder`.
- Unity version: `6000.3.11f1`.
- Render pipeline: URP.
- DI: VContainer.
- Async package: UniTask.
- Input package: Unity Input System.
- First imported/test asset pack: KayKit Medieval Builder Pack 1.0.
- Latest committed baseline: `df2297a feat: implement UI pointer blocking and input routing for camera and placement drivers`.


Check the live state before editing:

- `git log -1 --oneline`
- `git status --short`

Local-only paths that should not be committed by default:

- `Cozy_Builder/.screenshots/`
- `Cozy_Builder/Assets/.screenshots/`
- `Cozy_Builder/Packages/io.realvirtual.mcp/`
- `Cozy_Builder/Assets/.mcp_auth_token`

`Cozy_Builder/ProjectSettings/SceneTemplateSettings.json` may appear as untracked after Unity activity; inspect before deciding whether it belongs in version control.

## Current Runtime Map

- Runtime root: `Cozy_Builder/Assets/CozyBuilder/Runtime`.
- Runtime assembly: `Cozy_Builder/Assets/CozyBuilder/Runtime/CozyBuilder.Runtime.asmdef`.
- Composition root: `Cozy_Builder/Assets/CozyBuilder/Runtime/Bootstrap/GameLifetimeScope.cs`.
- Test scene: `Cozy_Builder/Assets/CozyBuilder/Scenes/KayKitFbxAssetTest.unity`.

Current data/services:

- `GridCoord`
- `CellFlags`
- `CellData`
- `TerrainType`
- `GridNeighborhood`
- `OrganicIslandGridGenerator`
- `RuleResult`
- `TownData`
- `TownDataStore`
- `PlacementService`
- `RuleEvaluator`
- `TownVisualRebuilder`
- `CameraService`

Current Unity adapters:

- `TownGridView`
  - reads `TownDataStore.Current`
  - generates initial island terrain placeholder cells
  - processes dirty cells in `LateUpdate`
  - separates terrain views under `Terrain Cells` from pooled block views under `Block Cells`
- `PrototypePlacementDebugDriver`
  - temporary MCP/manual validation adapter for place/delete
- `PrototypePlacementInputDriver`
  - maps mouse/touch screen positions to `GridCoord`
  - uses Unity Input System (`Mouse.current`, `Touchscreen.current`)
  - calls `PlacementService`
- `PrototypePlacementControlsView`
  - temporary IMGUI prototype controls for mode, `ColorId`, and `MaterialId`
- `PrototypePlacementState`
  - singleton state for Place/Delete mode and current palette ids
- `PrototypeTownDebugState`
  - singleton debug state for the currently selected cell
- `PrototypeTownDebugView`
  - temporary IMGUI debug panel for selected cell, neighbors, dirty queue, and rule preview
- `PrototypeCameraInputDriver`
  - temporary camera adapter for orbit, pan, zoom, and reset input

## Decisions That Must Not Drift

- Data-first: town logic lives in `TownData`/struct data, not scene hierarchy.
- Scene GameObjects are visual output/adapters only, not the source of truth.
- No static singleton gameplay services.
- Use VContainer for system-level services and adapters, not per-cell/per-block data.
- Do not rebuild the whole town for one changed cell; design toward dirty cells/chunks.
- Do not `Instantiate`/`Destroy` continuously in gameplay paths; use pooling where objects are created/deleted frequently.
- Avoid large active GameObject counts; move toward chunk mesh/batching/pooling as prototype complexity grows.
- Do not create runtime material instances casually; use ids/shared materials/MaterialPropertyBlock when color work becomes real.
- Use structs for compact high-count data like `GridCoord`, `CellData`, `RuleResult`.
- Use UniTask for async workflows with cancellation, not placement hot paths.
- Do not add non-prototype features before placement, visual update, debug visibility, and camera feel are proven.
- KayKit is prototype terrain/grid placeholder content, not the final procedural building foundation.

## Latest Session Update - 2026-05-23 - Overlapping Blocks Fix & Runtime GameObject Wrapper Offsets

Baseline before the work:
- KayKit models faced horizontal overlapping (clashing walls and roofs of adjacent house blocks, especially when rotated 90 degrees) because they exceed cell spacing limits.
- Stacking blocks vertically resulted in severe clipping/intersection issues because the vertical block height step (`blockHeightStep`) defaults to a flat 1.0m, while models are actually ~2.1m tall.
- We committed to a zero asset modification constraint (Option 1), meaning we must not manually edit FBX assets in external tools or in Unity Editor assets directly.

Implemented:
- **Runtime GameObject Wrapper Pattern**: Added an automated empty parent GameObject wrapper to serve as the unified grid pivot/scale reference. Instantiated FBX models are attached as children and automatically adjusted with customized position, rotation, and scale multiplier offsets.
- **Dynamic Offset Configuration**: Introduced the `PrefabOffsetConfig` structure and serialized `prefabOffsets` list inside `TownGridView.cs`. Added pre-populated robust default offsets inside `AutoWirePrefabs` (such as house model downscaling to `0.85`) so KayKit models work flawlessly out-of-the-box.
- **Decoupled Vertical Stacking Heights**: Split Y position math into distinct serialized parameters: `firstBlockHeightOffset = 0.35f` (maps from world ground to hexagon terrain surface) and `blockHeightStep = 2.0f` (maps vertical distance between layers). Updated `GridToWorld` formulas accordingly.
- **Recursive Adapter & Collider Attachment**: Enhanced the `ApplyColorAndMaterial` block color search and runtime `EnsureCollider` scanner to check dynamically across the Wrapper parent-child hierarchy recursively, maintaining 100% backward-compatibility and zero GC Allocations.
- **Focus Debug & Input Driver Alignment**: Updated height formulas in `PrototypeTownDebug3D.cs` so hover text and dirty highlights align smoothly on top of stacked wrapper layers, and replaced hardcoded spacing in double-tap camera focus with `CellSpacing`.

Validation:
- C# codebase compiles beautifully with zero errors or warnings.
- Real-time Play Mode execution confirms that walls stack seamlessly with zero vertical clipping and row houses align edge-to-edge without overlapping roofs.
- `graphify update .` completed successfully, updating the AST graph to 1981 nodes, 2128 edges, and 157 communities.


## Next Work

1. **Color & Material Visual Integration (Tích hợp bảng màu & chất liệu trực quan)**:
   - Xây dựng một Palette màu ấm cúng (3-6 màu giống như Townscaper).
   - Tích hợp thay đổi màu sắc trực quan của block trên Scene dựa trên dữ liệu `ColorId` và `MaterialId` hiện có bằng giải pháp tối ưu hiệu năng `MaterialPropertyBlock` (Zero GC Alloc).
2. **Visual Debug Tooling System (Hệ thống công cụ Debug 3D trực quan)**:
   - Triển khai Mesh lưới dòng biên hữu cơ duy nhất (Grid Line Mesh) để vẽ lưới hòn đảo 3D trên Scene với hiệu năng cao (1 Draw Call, Active/Deactive).
   - Tạo UI 3D lơ lửng bám theo ô đang được chọn/hover (Focus-based Debug) để hiển thị trực quan thông tin hàng xóm (Neighbor Index) và các quy tắc RuleResult được áp dụng.
   - Thêm highlight 3D (sử dụng box mờ dạng pooling) cho các ô đang nằm trong dirty queue để dễ dàng kiểm thử quy trình rebuild.
3. **Minimal Mobile UI Canvas (Lớp giao diện cảm ứng tối thiểu)**:
   - Chuyển đổi các nút điều khiển IMGUI tạm thời sang một Canvas di động tối giản (Canvas uGUI đơn giản) để dễ dàng thao tác chạm đổi màu, đổi chế độ và bật/tắt công cụ Debug 3D trực tiếp trên thiết bị di động thay vì dùng giao diện IMGUI thô sơ của Unity Editor.
4. **[FUTURE WORK - PREFAB AUTOMATION] Editor Prefab Generation Automation (Tự động hóa sinh Prefab trong Editor)**:
   - **Mục tiêu**: Chuyển đổi từ **Phương án 1 (Runtime Auto-Add Colliders)** sang **Phương án 3 (Editor Prefab Automation)** trước khi chuyển từ Prototype sang Production.
   - **Lý do**: Tối ưu hóa hiệu năng CPU (loại bỏ chi phí "nấu" MeshCollider ở runtime khi bắt đầu game hoặc sinh block), giảm thiểu dung lượng RAM, và cho phép tùy biến sâu trong Editor (gắn thêm SFX, VFX khói bụi, cấu hình Layer vật lý tĩnh, hệ thống Particle, v.v.).
   - **Hướng thực hiện**:
     1. Viết một `EditorWindow` hoặc `AssetPostprocessor` tùy chỉnh để tự động quét thư mục FBX của KayKit.
     2. Tạo tự động các file `.prefab` tương ứng trong thư mục `Assets/CozyBuilder/Prefabs/`.
     3. Tự động thêm các component `MeshCollider` hoặc `BoxCollider` phù hợp vào Prefab trong Editor.
     4. Cấu hình Static flags, Tag, Layer vật lý tĩnh, và nén mesh.
     5. Cập nhật lại các trường `Inspector` trên component `TownGridView` để tham chiếu đến các Prefab được sinh ra tự động này thay vì các tệp FBX gốc.

For camera and input routing work, read:

- `docs/Architecture_And_Code_Rules.md`
- `docs/Unity_URP_Performance_Code_Rules.md`
- `docs/Prototype_Core_Scope.md`
- `graphify-out/GRAPH_REPORT.md`


Use Graphify for code navigation before broad grep:

- `graphify explain "TownGridView"`
- `graphify explain "RuleEvaluator"`
- `graphify path "PlacementService" "RuleEvaluator"`

## Docs Map

Read selectively by task. Do not open every doc by default.

- Current startup state: `CURRENT_STATUS.md`
- Current handover and project map: `HANDOVER.md`
- Historical session archive: `docs/Development_Session_Log.md`
- Architecture/code foundation: `docs/Architecture_And_Code_Rules.md`
- Unity/URP performance: `docs/Unity_URP_Performance_Code_Rules.md`
- Prototype scope: `docs/Prototype_Core_Scope.md`
- Asset/KayKit strategy:
  - `docs/Tooling_And_Asset_Strategy.md`
  - `docs/Asset_Selection_Checklist.md`
  - `docs/Asset_Pack_Shortlist.md`
- Product/gameplay/roadmap:
  - `docs/Product_Vision_One_Page.md`
  - `docs/Our_Cozy_Procedural_Town_Builder_Gameplay.md`
  - `docs/App_Development_Roadmap.md`
  - `docs/Townscaper_Gameplay_Research.md`
- Launch/monetization:
  - `docs/App_Launch_And_Monetization_Plan.md`

## Graphify

- Graphify output exists at `graphify-out/`.
- Use Graphify for code-symbol navigation, not as the source of truth for Markdown documentation.
- Current maintenance command: `graphify update .`
- After modifying `.cs`, `.asmdef`, or other code-structure files, run `graphify update .` before the final response and mention whether it succeeded.
- Do not rely on Graphify query output alone for Markdown documentation decisions unless semantic extraction has explicitly been run with an API key.

## Unity MCP

- Unity MCP is local editor tooling, not gameplay/runtime logic.
- Configured server name: `unity`.
- Unity Editor must be open and the MCP toolbar must show the server running.
- Codex user config points to the embedded Python bridge:
  - `Cozy_Builder/Assets/StreamingAssets/realvirtual-MCP/python/python.exe`
  - `unity_mcp_server.py --mode stdio --ws-port 18711`
- Do not commit MCP runtime files or auth tokens unless project policy changes.
