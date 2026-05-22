# HANDOVER

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

## Latest Session Update - 2026-05-22 - UI Pointer Blocking & Input Routing

Baseline before the work:
- Procedural rules and object pooling completed. Input conflicts still existed where clicking IMGUI buttons placed/deleted blocks or rotated camera.

Current commit state:
- UI/Input Routing and Pointer Blocking are 100% completed and fully committed.
- Baseline commit: `df2297a feat: implement UI pointer blocking and input routing for camera and placement drivers`

Implemented:
- Added public `PanelRect` property getter to `PrototypePlacementControlsView` and `PrototypeTownDebugView` to expose IMGUI bounds.
- Implemented robust `IsPointerOverUI` method inside `PrototypePlacementInputDriver` and `PrototypeCameraInputDriver`:
  - Checked `EventSystem.current.IsPointerOverGameObject()` to seamlessly support future uGUI/UI Toolkit elements.
  - Checked IMGUI panel rects by mapping coordinates from Input System's bottom-left origin to IMGUI's top-left origin using screen height.
- Blocked block placement and deletion in `PrototypePlacementInputDriver` when the cursor is over any active UI panels.
- Blocked camera orbit/panning dragging and scroll zoom in `PrototypeCameraInputDriver` when pointer interaction starts over active UI panels:
  - Tracked click/touch start state using `wasDragStartedOverUI`/`wasTouchStartedOverUI` on the first frame of interaction.
  - Allowed camera orbit/panning dragging to continue smoothly when the cursor crosses active UI panels, provided the interaction started outside the UI (Drag Continuity).
  - Blocked scroll wheel zoom when the pointer is positioned over active UI panels.

Validation:
- C# project compiles cleanly without any errors or warnings.
- Real-time interaction verified in Play Mode: buttons, placement, deletion, camera panning, camera orbit, zoom, and touch interactions perform cleanly with robust pointer blocking.
- `graphify update .` successfully updated the AST graph to 172 nodes, 235 edges, and 21 communities.

## Next Work

1. Polish camera pivot, zoom responsiveness, and gesture support on mobile targets.
2. Expand procedural rules variations or color/palette/material integrations for block views.
3. Establish uGUI or UI Toolkit production UI layout using the developed EventSystem pointer blocking base.

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
