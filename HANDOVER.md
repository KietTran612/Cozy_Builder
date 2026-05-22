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
- Latest committed baseline before current camera work: `2a7824d Add prototype town debug view`.

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

## Latest Session Update - 2026-05-22 - Procedural Rules System

Baseline before the work:
- Camera work is registered in local workspace and committed, starting implementation of Procedural Rules.

Current commit state:
- Procedural Rules System and dynamic zero-allocation pooling are 100% completed and fully committed.
- Baseline commit: `ced9487 docs: finalize procedural rules system task tracker`

Implemented:
- Added `RotationId` byte property (values 0..3 for 0°, 90°, 180°, 270°) to `RuleResult` struct with full backward-compatibility.
- Added height stack and neighbor-aware rules in `RuleEvaluator.cs` supporting:
  - Waterfront Stilts (`VisualId = 4` at base level on water-adjacent grids)
  - Small House (`VisualId = 1` for single-block grid stacks)
  - Tower Top (`VisualId = 3` for stacks exceeding neighbors' heights)
  - Row Houses Roof (`VisualId = 2` with auto-alignment along East-West/North-South axes)
  - Walls (`VisualId = 5` and `6` depending on neighboring blocks' presence)
- Refactored `PlacementService.Preview` and debug views to query neighborhoods based on cell coordinates.
- Redesigned `TownGridView.cs`'s visual block updates to use dynamic `Dictionary<ushort, Queue<GameObject>>` object pooling.
- Used zero-allocation `struct BlockViewData` in the visual state to completely eliminate heap allocation noise in the hot path.
- Configured 6 specialized prefab fields in `TownGridView` with dynamic inspector binding and a safe fallback to `blockPrefab`.
- Handled correct rotation calculations (`result.RotationId * 90f`) and safe clean-up on block deletion or state resets.

Validation:
- C# structural code compiles cleanly without any errors or warnings.
- Real-time cell morphing, connected roofs rotation, stilts foundation placement, and vertical walls stack work precisely as designed.
- Object pooling behaves with zero GC allocation spikes during placement/deletion.
- Graphify AST model successfully rebuilt: 168 nodes, 225 edges, and 20 communities.

## Next Work

1. Iterate on UI/input routing (e.g. uGUI or UI Toolkit) to handle pointer blocking and separate placement gestures from camera orbit.
2. Expand procedural rules variations or color/palette/material integrations for block views.
3. Clean up and polish camera pivot and gesture responsiveness on mobile targets.

For procedural rule work, read:

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
