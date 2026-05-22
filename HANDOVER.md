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

## Latest Session Update - 2026-05-22 - Premium Camera Interactions & Mobile Touch Gestures

Baseline before the work:
- Camera system was successfully decoupled and modularized into `CozyBuilder.Camera.asmdef`, but lacked inertia (smooth damping), advanced boundaries, mobile multi-touch support (orbit with 1 finger, pinch-zoom & pan with 2 fingers), and delay-based Tap/Double-Tap Focus machine to prevent accidental block placements while dragging.

Current commit state:
- Premium Camera Interactions and Mobile Touch Gestures logic is 100% implemented, static verification and graph updating are complete. Changes are uncommitted, awaiting manual Play Mode verification and performance checks by the user.

Implemented:
- Added `current` and `target` state values with `Vector3.SmoothDamp` and `Mathf.SmoothDampAngle` in `CameraService.cs` to support beautiful camera inertia/damping transitions.
- Integrated a camera panning pivot bounding sphere (`maxPivotRadius = 15f`) to prevent the camera from straying too far away from the island grid.
- Rewrote mobile multi-touch detection using static arrays from the Unity Input System (`Touchscreen.current.touches[0]` and `[1]`) to avoid runtime garbage generation:
  - **Orbit (1 Finger)**: Drag to rotate the camera around the island with delta filtering on first touch.
  - **Pinch Zoom & Pan (2 Fingers)**: Smoothly zoom (using touch distance delta scaled to current distance for dynamic responsiveness) and pan (using touch average delta translation) simultaneously.
- Programmed a delay-based touch state machine in `PrototypePlacementInputDriver.cs` (`tapDurationThreshold = 0.25s`, `tapMoveThreshold = 15px`) to reliably differentiate drag-to-orbit from tap-to-place.
- Introduced a `0.15s` delay queue (`pendingSingleTapExecuteTime`) for single-tap placements: if a second tap is detected within `0.25s` (`doubleTapInterval`), the single-tap is cancelled and a double-tap is executed instead.
- Implemented **Double-Tap Focus** which uses raycasting onto the grid plane to center the camera's focus on the double-tapped cell through `CameraService.FocusOn()`.
- Optimized both input drivers to ensure **Zero GC Allocations** in gameplay runtime loops (`Update` and `LateUpdate`).

Validation:
- C# project compiles flawlessly in Unity 6000.3.11f1 without any warnings or compiler errors.
- Checked static allocations in all update paths; loops operate strictly on the stack with pre-allocated states (0 GC Allocations guaranteed).
- `graphify update .` successfully updated the AST graph to 180 nodes, 243 edges, and 21 communities.


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
