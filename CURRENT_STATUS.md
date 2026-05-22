# CURRENT STATUS

This is the short startup context for agents. Read this first, then use `HANDOVER.md` only when deeper current project context is needed. Use `docs/Development_Session_Log.md` only for old session history.

## Product Direction

- Unity mobile game in the cozy procedural town builder space.
- Core fantasy: place blocks slowly and watch a small peaceful town take shape.
- Not a resource-management city builder.
- Inspired by Townscaper research, but must not become a direct clone.
- Differentiators planned over time: clearer terrain strategy, controlled grid/seed, ambient life, possible street/walk view, mobile-first performance.

## Current Phase

- Phase 0 / early Prototype Core foundation.
- Unity project exists at `Cozy_Builder`.
- Unity version: `6000.3.11f1`.
- Render pipeline: URP.
- DI: VContainer.
- Async package: UniTask.
- First imported/test asset pack: KayKit Medieval Builder Pack 1.0.
- KayKit validation is complete enough to use it as prototype terrain/grid placeholder content.
- Prototype Core placement/visual/input/debug foundation has started.

## Current Commit Baseline

- Latest committed baseline after completing UI Pointer Blocking: `df2297a feat: implement UI pointer blocking and input routing for camera and placement drivers`.

- Check `git log -1 --oneline` and `git status --short` for the latest committed/uncommitted state.
- Some `docs/*.md` files may appear modified from line-ending noise; do not stage them unless their content was intentionally changed.
- Current uncommitted work includes updating status/handover documentation files.
- Local-only/untracked MCP package files may appear under `Cozy_Builder/Packages/io.realvirtual.mcp/`; do not commit them unless project policy changes.
- Do not commit `Cozy_Builder/Assets/Packages` asset pack contents unless the user explicitly changes that policy.

## What Exists

- Project runtime root: `Cozy_Builder/Assets/CozyBuilder/Runtime`.
- Runtime assembly: `Cozy_Builder/Assets/CozyBuilder/Runtime/CozyBuilder.Runtime.asmdef`.
- Composition root: `Cozy_Builder/Assets/CozyBuilder/Runtime/Bootstrap/GameLifetimeScope.cs`.
- Initial data/services:
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
- First visual adapter:
  - `TownGridView`
  - Reads `TownDataStore.Current`
  - Instantiates KayKit tile placeholder views for initial island cells
  - Keeps runtime generated cell GameObjects under `Generated Town Cells`
  - Processes `TownVisualRebuilder` dirty cells in `LateUpdate`
  - Separates terrain tile views from pooled block views under `Terrain Cells` and `Block Cells`
  - Uses dynamic Dictionary-backed `Queue<GameObject>` object pooling for all visual block types
  - Uses zero-allocation `struct BlockViewData` in cell visual state to prevent GC allocations
  - Provides 6 inspector slots for specialized block prefabs with automated fallback logic
  - Correctly maps and applies 4-cardinal rotations to visual elements based on rule results
- Prototype debug driver:
  - `PrototypePlacementDebugDriver`
  - Calls `PlacementService.TryPlaceBlock` / `TryDeleteBlock` for MCP/manual validation
- Prototype input driver:
  - `PrototypePlacementInputDriver`
  - Converts mouse/touch screen input to a grid coordinate through `TownGridView` and calls `PlacementService`
  - Integrates pointer blocking via `IsPointerOverUI` (maps coordinate space and checks EventSystem plus IMGUI panel boundaries) to prevent placing or deleting blocks when interacting with UI elements
- Prototype placement controls:
  - `PrototypePlacementState`, `PrototypePlacementMode`, and `PrototypePlacementControlsView`
  - Provides minimal IMGUI controls for place/delete mode, `ColorId`, and `MaterialId`
  - Exposes IMGUI screen boundaries via public `PanelRect` property for pointer blocking checks
- Prototype debug views:
  - `PrototypeTownDebugState` and `PrototypeTownDebugView`
  - Shows selected cell id/data, cardinal neighbor info, dirty queue count/preview, and rule result preview
  - Exposes IMGUI screen boundaries via public `PanelRect` property for pointer blocking checks
- Prototype camera controls:
  - `CameraService` and `PrototypeCameraInputDriver`
  - Provides temporary orbit/pan/zoom/reset camera controls through Unity Input System
  - Integrates robust pointer blocking via `IsPointerOverUI` to block orbit/pan dragging startup and scroll zoom over UI elements, while maintaining drag continuity if dragging started outside UI bounds

- KayKit FBX test scene: `Cozy_Builder/Assets/CozyBuilder/Scenes/KayKitFbxAssetTest.unity`.
- KayKit test scene now contains separated visual samples plus simple procedural compatibility cases:
  - 1-cell house
  - 2-house row
  - 2-level stack
  - wall segment row
  - hex tile sample
- Graphify output exists at `graphify-out/` and is maintained with `graphify update .`.
- Unity MCP package/server has been installed locally; Codex user config points to the embedded Python bridge.

## Current Intent

- Code is still prototype foundation only, but the first data, placement, visual, input, mode/palette, debug-view, and camera-control paths exist.
- `GameLifetimeScope` should register system-level prototype services only.
- Data must remain separate from scene GameObjects.
- No static singleton gameplay services.
- KayKit should be used as prototype terrain/grid placeholder content, not as the final procedural building foundation.
- Current prototype direction is data-first island grid, dirty visual updates, simple input, temporary controls, debug visibility, and camera feel before deeper procedural rules.

## Next Work

1. Polish camera pivot, zoom responsiveness, and gesture support on mobile targets.
2. Expand procedural rules variations or color/palette/material integrations for block views.
3. Establish uGUI or UI Toolkit production UI layout using the developed EventSystem pointer blocking base.


## Latest Validation Notes

- Unity MCP connected successfully while Unity Editor was open.
- `KayKitFbxAssetTest.unity` opened and compiled in Unity `6000.3.11f1`.
- KayKit FBX samples render in URP without magenta/missing-shader materials.
- KayKit license file confirms CC0 and commercial use.
- KayKit inventory is strong for terrain/grid tests: hex/square tiles, road, water, water corners/straights, forest/rock/sand variations.
- KayKit object inventory includes buildings and walls, but mostly as whole objects; it is not yet suitable as a main wall/roof procedural building foundation.
- Wall segment spacing at 2m aligns cleanly in the scene test.
- Whole-house objects are useful for scale/mood placeholders but do not behave like clean wall/roof modules.
- First `TownGridView` Play Mode validation succeeded:
  - `Town Grid View` exists in `KayKitFbxAssetTest.unity`
  - `cellPrefab` is wired to the KayKit `hex_forest` scene object
  - Play Mode generated an island grid from `TownDataStore.Current`
  - generated runtime cells did not persist after exiting Play Mode
- Dirty-cell visual validation succeeded:
  - `PrototypePlacementDebugDriver.PlaceDebugBlock` returned `True`
  - `Cell 0,0` changed to `Cell 0,0 H1` and moved to local Y `0.35`
  - `PrototypePlacementDebugDriver.DeleteDebugBlock` returned `True`
  - `Cell 0,0` changed back to `Cell 0,0 H0` and local Y `0`
  - After exiting Play Mode, generated runtime cells did not persist in the scene
- Prototype screen input validation succeeded:
  - `PrototypePlacementInputDriver.PlaceScreenCenter` returned `True`
  - screen center mapped to `Cell 0,1`, which changed to `Cell 0,1 H1`
  - `PrototypePlacementInputDriver.DeleteScreenCenter` returned `True`
  - searching for `H1` after delete returned zero objects
  - `Town Grid View` now has `TownGridView`, `PrototypePlacementDebugDriver`, and `PrototypePlacementInputDriver`
- Pooled block visual validation succeeded:
  - `Prototype Block Source` inactive cube exists in the scene and is assigned to `TownGridView.blockPrefab`
  - placing at screen center creates/activates `Block 0,1 L1` under `Generated Town Cells/Block Cells`
  - `Cell 0,1` remains a terrain tile under `Generated Town Cells/Terrain Cells` at local Y `0`
  - deleting at screen center leaves `Block 0,1 L1` pooled but inactive
  - generated runtime terrain/block children do not persist after exiting Play Mode
- Screenshot output for this validation: `Cozy_Builder/.screenshots/scene_20260522_112322.png`.
- Prototype mode/palette controls validation succeeded:
  - Added `PrototypePlacementControlsView` to `Town Grid View`
  - IMGUI controls show current mode, `ColorId`, and `MaterialId`
  - Buttons support Place/Delete plus Color and Material ids `0..3`
  - `PrototypePlacementInputDriver.PlaceScreenCenter` returned `True`
  - `PrototypePlacementInputDriver.DeleteScreenCenter` returned `True`
  - Legacy input exception was fixed by switching the input driver to Unity Input System
  - `EventSystem` is not required for the current IMGUI controls; it will be needed later for uGUI/UI Toolkit pointer routing
  - Editor screenshot output was written under `Cozy_Builder/Assets/.screenshots/`
- Prototype debug view validation succeeded:
  - Added `PrototypeTownDebugView` to `Town Grid View`
  - `PrototypePlacementInputDriver.PlaceScreenCenter` returned `True`
  - `PrototypePlacementInputDriver.DeleteScreenCenter` returned `True`
  - `PrototypePlacementDebugDriver.PlaceDebugBlock` returned `True`
  - `PrototypePlacementDebugDriver.DeleteDebugBlock` returned `True`
  - `Town Grid View` has `PrototypeTownDebugView` with panel rect `(16, 208, 320, 300)`
  - Console readback after Play Mode showed no project exceptions
  - `graphify update .` succeeded after code changes with 149 nodes, 189 edges, and 20 communities
- Prototype camera controls validation succeeded:
  - Added `PrototypeCameraInputDriver` to `Main Camera`
  - `PrototypeCameraInputDriver.ResetCamera` returned status `ok`
  - `Main Camera` reset to position `(0, 6.88895035, -11.0246248)` and rotation `(32, 0, 0)`
  - `PrototypePlacementInputDriver.PlaceScreenCenter` returned `True`
  - `PrototypePlacementInputDriver.DeleteScreenCenter` returned `True`
  - Console readback after Play Mode showed no project exceptions
  - `graphify update .` succeeded after code changes with 165 nodes, 217 edges, and 20 communities
- Unity/editor console still shows recurring assertion noise: `Assertion failed on expression: 'IsNormalized(dir, 0.0001f)'`. No stack trace currently points to project gameplay code.
- Procedural Rules System validation succeeded:
  - Placement of a single block displays a Small House (`VisualId = 1`) correctly.
  - Multi-level stacks generate Tunnels/Walls (`VisualId = 5` or `6`) for lower layers and custom Roof/Tops (`VisualId = 3`) for the peak layer.
  - Adjacent blocks align dynamically, creating continuous Row Houses (`VisualId = 2`) with roofs rotated automatically along East-West (`RotationId = 0`) or North-South (`RotationId = 1`) axes.
  - Placements next to water or on waterfront coordinates display Stilts (`VisualId = 4`) for the foundation layer.
  - Dynamic `Dictionary<ushort, Queue<GameObject>>` recycling has zero runtime heap allocations in dirty update cycles.
  - Clean recycling in `RefreshCell` and `ClearGeneratedCells` successfully avoids visual artifacts and memory leaks.
  - Project compiles cleanly with no warnings or errors, and `graphify update .` successfully updated the codebase to 168 nodes, 225 edges, and 20 communities.
- UI Pointer Blocking & Input Routing validation succeeded:
  - Exposing public `PanelRect` getters on `PrototypePlacementControlsView` and `PrototypeTownDebugView` provides bounds query APIs.
  - Switch coordinates and `IsPointerOverUI` checks in `PrototypePlacementInputDriver` block block placement/deletion cleanly when clicking UI buttons.
  - `PrototypeCameraInputDriver` tracks `wasDragStartedOverUI`/`wasTouchStartedOverUI` to block starting zoom and orbit/pan dragging over UI, while drag continuity allows camera movements to traverse across UI panels smoothly if started outside.
  - Real-time Play Mode execution compiles cleanly with zero warnings/errors.
  - `graphify update .` successfully updated the codebase to 172 nodes, 235 edges, and 21 communities.


## Latest Code Notes

- `TownDataStore.Current` now initializes an organic island grid with radius 4.
- `TownData` now owns coordinates, cell data, and coordinate-to-index lookup.
- `PlacementService` now has data-first `TryPlaceBlock` and `TryDeleteBlock` APIs.
- Placement/delete marks the changed cell and cardinal neighbors dirty.
- `TownVisualRebuilder` now has a deduplicated dirty queue foundation.
- `TownGridView` has been added as the first data-to-visual Unity adapter.
- `TownGridView` now injects `TownVisualRebuilder` and processes dirty cells with `ProcessDirtyCells`.
- Dirty visual updates refresh only affected cell views instead of rebuilding the whole island.
- Placeholder block height currently appears through pooled block views under `Generated Town Cells/Block Cells`.
- `PrototypePlacementDebugDriver` remains as temporary MCP/manual place-delete validation tooling.
- `PrototypePlacementInputDriver` has been added as the first mouse/touch input adapter.
- `PrototypePlacementInputDriver` now reads `PrototypePlacementState` for place/delete mode and palette ids.
- `PrototypePlacementInputDriver` now uses Unity Input System and the runtime asmdef references `Unity.InputSystem`.
- `PrototypePlacementState` centralizes current prototype mode, `ColorId`, and `MaterialId`.
- `PrototypePlacementControlsView` adds minimal IMGUI buttons for mode and palette selection.
- `PrototypeTownDebugState` stores the currently selected grid coordinate for debug UI.
- `PrototypePlacementInputDriver` and `PrototypePlacementDebugDriver` update `PrototypeTownDebugState` when a cell is targeted.
- `PrototypeTownDebugView` displays selected cell data, neighbors, dirty queue preview, and `PlacementService.Preview` rule result.
- `TownVisualRebuilder.CopyDirtyCoords` exposes a bounded dirty queue snapshot for debug display without mutating the queue.
- `TownGridView.TryGetCoordFromWorld` maps world positions back to existing grid coordinates.
- `TownGridView` now keeps terrain visuals and block visuals separate.
- Block visuals are pooled per cell: delete disables existing block instances instead of destroying them.
- Placeholder block source is currently a scene cube named `Prototype Block Source`, not final art.
- `GameLifetimeScope` now registers `TownGridView` from the scene hierarchy with VContainer.
- `GameLifetimeScope` now registers `PrototypePlacementDebugDriver` from the scene hierarchy with VContainer.
- `GameLifetimeScope` now registers `PrototypePlacementInputDriver` from the scene hierarchy with VContainer.
- `GameLifetimeScope` now registers `PrototypePlacementState` as a singleton and `PrototypePlacementControlsView` from the scene hierarchy.
- `GameLifetimeScope` now registers `PrototypeTownDebugState` as a singleton and `PrototypeTownDebugView` from the scene hierarchy.
- `CameraService` stores prototype camera pivot, distance, yaw, pitch, and clamp limits.
- `PrototypeCameraInputDriver` applies camera orbit, pan, zoom, and reset input in `LateUpdate`.
- Current desktop camera controls are `Alt + left drag` orbit, middle drag pan, mouse wheel zoom, and `R` reset.
- Current touch camera controls are two-finger pan and pinch zoom.
- `PrototypePlacementInputDriver` ignores `Alt + left` so orbit does not also place a block.
- Unity compile completed without C# errors after the prototype camera changes.
- `graphify update .` succeeded after the latest code changes and updated `graphify-out/` to 168 nodes, 225 edges, and 20 communities.
- `RuleResult` property `RotationId` added to support 4-cardinal rotations (0°, 90°, 180°, 270°).
- `RuleEvaluator` implemented height-stack and neighbor-aware rule assessment.
- `TownGridView` now uses a dynamic `Dictionary`-backed `Queue<GameObject>` pooling architecture for blocks, zero-allocation `struct BlockViewData` to prevent GC allocations, and maps 4-cardinal rotations correctly.
- Specialized block prefab slots (`smallHousePrefab`, `houseRoofPrefab`, `towerTopPrefab`, `stiltsPrefab`, `houseWallPrefab`, `towerWallPrefab`) added to `TownGridView` with dynamic inspector mapping and safe fallback.
- `PlacementService.Preview` and `PrototypeTownDebugView` updated to support neighbor querying with coordinate parameters.

## Current Working Tree Notes

- Always check `git status --short` before editing or committing.
- Camera controls work, Procedural Rules System, task trackers, implementation plan, and Graphify refreshes are all fully committed to Git.
- Expected modified files for the current session:
  - `CURRENT_STATUS.md`
  - `HANDOVER.md`
  - `docs/Development_Session_Log.md`
- Expected local/untracked files that should not be committed by default:
  - `Cozy_Builder/.screenshots/`
  - `Cozy_Builder/Assets/.screenshots/`
  - `Cozy_Builder/Packages/io.realvirtual.mcp/`
- `Cozy_Builder/ProjectSettings/SceneTemplateSettings.json` appeared as untracked after Unity activity; inspect before deciding whether it belongs in version control.

## Rules That Must Not Drift

- Data-first: town logic lives in data, not scene hierarchy.
- Do not rebuild the whole town for one changed cell; design toward dirty cells/chunks.
- Do not `Instantiate/Destroy` continuously in gameplay paths.
- Avoid large active GameObject counts; design toward chunk mesh/batching/pooling.
- Do not create runtime material instances casually.
- Use VContainer for system dependencies, not per-cell/per-block data or service locator behavior.
- Use structs for compact high-count data like `GridCoord`, `CellData`, `RuleResult`.
- Use UniTask for async workflows with cancellation, not placement hot paths.
- Do not add non-prototype features before core placement/visual/camera feel is proven.

## Context Reading Rules

- Read this file first for current status.
- Read `HANDOVER.md` only for deeper current project context or when current status is unclear.
- Read `docs/Development_Session_Log.md` only when old session history is needed.
- Do not read every doc linked from `HANDOVER.md`.
- When writing a new `Latest Session Update` in `HANDOVER.md`, first append the previous latest session to `docs/Development_Session_Log.md`.
- For that routine archive step, do not read the whole session log; use a targeted tail/search only if needed to avoid duplicating an entry.
- Read docs by task:
  - architecture/code foundation: `docs/Architecture_And_Code_Rules.md`, `docs/Unity_URP_Performance_Code_Rules.md`, `docs/Prototype_Core_Scope.md`
  - asset/KayKit/URP test: `docs/Tooling_And_Asset_Strategy.md`, `docs/Asset_Selection_Checklist.md`, `docs/Asset_Pack_Shortlist.md`
  - product/gameplay/roadmap: `docs/Product_Vision_One_Page.md`, `docs/Our_Cozy_Procedural_Town_Builder_Gameplay.md`, `docs/App_Development_Roadmap.md`, `docs/Townscaper_Gameplay_Research.md`
  - launch/monetization: `docs/App_Launch_And_Monetization_Plan.md`

## Graphify

- Use Graphify for code navigation, not as the source of truth for docs.
- Useful commands:
  - `graphify explain "PlacementService"`
  - `graphify query "How does GameLifetimeScope relate to placement?"`
  - `graphify path "GameLifetimeScope" "PlacementService"`
- After modifying code files, run `graphify update .` before the final response.

## Unity MCP

- Unity MCP is local editor tooling, not gameplay/runtime logic.
- Codex MCP config is user-level at `C:/Users/Hoang.H/.codex/config.toml`.
- Configured server name: `unity`.
- It launches `Cozy_Builder/Assets/StreamingAssets/realvirtual-MCP/python/python.exe` with `unity_mcp_server.py --mode stdio --ws-port 18711`.
- Manual MCP client test succeeded with 76 tools listed after Unity discovery, including 73 Unity tools.
- Unity Editor must be open and the MCP toolbar must show the server running.
- A new Codex session/restart is needed before Unity MCP tools appear in the tool list.
- Do not commit `Assets/.mcp_auth_token` or the embedded `Assets/StreamingAssets/realvirtual-MCP/` Python runtime.
