# Development Session Log

> [!WARNING]
> **TỐI ƯU HÓA CONTEXT WINDOW (DÀNH CHO AI AGENT):**
> Khi nối thêm phiên làm việc cũ vào tệp này, **nghiêm cấm** sử dụng công cụ đọc toàn bộ tệp (như `view_file` không giới hạn dòng). 
> Chỉ sử dụng công cụ chỉnh sửa trực tiếp (`replace_file_content`) hoặc lệnh `tail` để đối chiếu cuối tệp nếu thực sự cần thiết.

This is the archive for older session updates. Do not read this file during normal startup. Read `CURRENT_STATUS.md` first, then `HANDOVER.md` if needed. Use this log only when old validation details, historical decisions, or commit context must be reconstructed.

## 2026-05-21 - Unity Project Setup

- Unity project created at `Cozy_Builder`.
- Unity version: `6000.3.11f1`.
- URP installed and configured.
- UniTask installed through Unity Package Manager, resolved version `2.5.11`.
- VContainer installed through Unity Package Manager, resolved version `1.18.0`.
- KayKit Medieval Builder Pack 1.0 imported.
- KayKit license checked: CC0, commercial use allowed.
- KayKit FBX chosen as the source format for prototype and production experiments.
- Current KayKit workflow avoids `dae`, `obj`, `gltf`, and `glb` duplicate source formats.
- `.gitignore` added for Unity generated folders and unused KayKit duplicate formats.

Important historical commit:

- `3b09e7a Setup Unity project packages and handover`

## 2026-05-21 - Code Foundation Started

- Added project runtime folder at `Cozy_Builder/Assets/CozyBuilder/Runtime`.
- Added runtime asmdef `CozyBuilder.Runtime`.
- Added initial `GameLifetimeScope`.
- Added initial data/service shells:
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
- Added `KayKitFbxAssetTest.unity` with camera, light, lifetime scope, and FBX asset samples.
- Intent was foundation only: no static gameplay singletons, data separate from scene objects.

## 2026-05-21 - Graphify Setup

- Added Graphify workflow instructions to `AGENTS.md`.
- Added `.codex/hooks.json`.
- Added `.graphifyignore`.
- Added Graphify output under `graphify-out/`.
- Decision: use `graphify update .` for AST-only no-cost graph refresh.
- Decision: Graphify is for code navigation, not the source of truth for docs.
- Decision: do not use paid semantic extraction unless explicitly requested.

Initial useful commands:

- `graphify update .`
- `graphify explain "PlacementService"`
- `graphify query "How does GameLifetimeScope relate to placement?"`
- `graphify check-update .`

## 2026-05-21 - Current Status Shortcut

- Added `CURRENT_STATUS.md`.
- Purpose: give agents compact startup state without requiring full `HANDOVER.md`.
- Startup rule established:
  - read `CURRENT_STATUS.md` first
  - read `HANDOVER.md` only when deeper current context is needed
  - read task-specific docs selectively

## 2026-05-21 - Unity MCP Local Setup

- Unity MCP installed locally through `io.realvirtual.mcp`.
- Unity Editor side runs a WebSocket bridge on port `18711`.
- Codex connects through the embedded Python MCP bridge over stdio, not directly to the WebSocket port.
- Codex user-level config updated at `C:/Users/Hoang.H/.codex/config.toml`.
- Manual MCP client test succeeded with 76 tools listed after discovery, including 73 Unity tools.
- Local MCP runtime and auth token are not project source by default.

Do not commit by default:

- `Cozy_Builder/Packages/io.realvirtual.mcp/`
- `Cozy_Builder/Assets/.mcp_auth_token`
- embedded `Assets/StreamingAssets/realvirtual-MCP/` runtime files unless policy changes

Important historical commit:

- `ee1392d Add Unity MCP workflow and startup context`

## 2026-05-22 - KayKit Validation And Prototype Core Data Foundation

- Unity MCP connected successfully with Unity Editor open.
- `KayKitFbxAssetTest.unity` opened in Unity `6000.3.11f1`.
- Unity compile/import completed without C# compile errors.
- KayKit FBX samples rendered in URP without magenta/missing-shader materials.
- KayKit confirmed useful for prototype terrain/grid placeholders:
  - hex/square tiles
  - forest/rock/sand variants
  - road tiles
  - water tiles
  - water edge/corner/straight pieces
  - wall and bridge placeholders
- KayKit whole building objects are not a final procedural building foundation.
- `KayKitFbxAssetTest.unity` gained separated asset samples and simple procedural compatibility cases:
  - 1-cell house
  - 2-house row
  - 2-level stack
  - 2-wall segment row
  - hex tile sample
- Measurement notes:
  - `wall_straight` aligns cleanly at 2m spacing
  - whole `house` objects at 2m spacing leave visible gaps
  - stacked house geometry can touch around 0.914m but this does not prove it is suitable for procedural stacking

Prototype data foundation changes:

- `TownDataStore.Current` initializes a small organic island grid with radius 4.
- `TownData` owns coordinates, cell data, and coordinate-to-index lookup.
- Added `TerrainType`, `GridNeighborhood`, and `OrganicIslandGridGenerator`.
- `CellData` stores `TerrainType Terrain`.
- `PlacementService` gained data-first place/delete APIs.
- `PlacementService` marks changed cell and cardinal neighbors dirty.
- `TownVisualRebuilder` gained a deduplicated dirty queue.

Important historical commit:

- `c45f758 Start prototype core data foundation`

## 2026-05-22 - First Visual Adapter Validation

- Added `TownGridView` as the first data-to-visual adapter.
- `GameLifetimeScope` registers `TownGridView` from scene hierarchy.
- `KayKitFbxAssetTest.unity` gained `Town Grid View`.
- `TownGridView.cellPrefab` wired to KayKit `hex_forest`.
- Play Mode generated an island grid from `TownDataStore.Current`.
- Runtime generated children appeared under `Town Grid View/Generated Town Cells`.
- Generated runtime cells did not persist after exiting Play Mode.
- Screenshot output was written under `Cozy_Builder/.screenshots/`.
- `graphify update .` succeeded with 81 nodes, 80 edges, 14 communities.

Known issue recorded:

- Unity/editor console showed repeated assertion noise:
  - `Assertion failed on expression: 'IsNormalized(dir, 0.0001f)'`
- No stack trace pointed to project gameplay code.

## 2026-05-22 - Dirty Cell Visual Update Loop

- Updated `TownGridView` to inject `TownVisualRebuilder`.
- `TownGridView` drains dirty cells in `LateUpdate`.
- Added `ProcessDirtyCells(int maxCells)`.
- Added `RefreshCell(GridCoord coord)`.
- Dirty visual updates refresh affected cells only, not the whole island.
- Placeholder height visualization initially moved the terrain tile upward.
- Added `PrototypePlacementDebugDriver` for MCP/manual place-delete validation.
- `GameLifetimeScope` registers `PrototypePlacementDebugDriver`.
- `KayKitFbxAssetTest.unity` gained `PrototypePlacementDebugDriver`.

Validation:

- Unity compile/reload completed without C# compile errors.
- `PrototypePlacementDebugDriver.PlaceDebugBlock` returned `True`.
- `Cell 0,0` became `Cell 0,0 H1`.
- `PrototypePlacementDebugDriver.DeleteDebugBlock` returned `True`.
- `Cell 0,0` returned to `Cell 0,0 H0`.
- Generated runtime cells did not persist after exiting Play Mode.
- `graphify update .` succeeded with 93 nodes, 101 edges, 15 communities.

Important historical commit:

- `4ff5d15 Add prototype town visual update loop`

## 2026-05-22 - Prototype Click/Tap Placement Input

- Added `PrototypePlacementInputDriver`.
- It converted mouse/touch screen position to a ray.
- It intersected the ray with the grid plane.
- It asked `TownGridView` to map world position to an existing `GridCoord`.
- It called `PlacementService.TryPlaceBlock` or `TryDeleteBlock`.
- `TownGridView` gained `TryGetCoordFromWorld`.
- `GameLifetimeScope` registers `PrototypePlacementInputDriver`.
- `KayKitFbxAssetTest.unity` gained `PrototypePlacementInputDriver`.

Validation:

- Unity compile/reload completed without C# compile errors after fixing `UnityEngine.Camera` aliasing.
- `PrototypePlacementInputDriver.PlaceScreenCenter` returned `True`.
- Screen center mapped to `Cell 0,1`.
- `PrototypePlacementInputDriver.DeleteScreenCenter` returned `True`.
- Searching for `H1` after delete returned zero objects.
- `graphify update .` succeeded with 108 nodes, 125 edges, 16 communities.

Important historical commit:

- `1efc7c3 Add prototype placement input`

## 2026-05-22 - Pooled Prototype Block Visuals

- Updated `TownGridView` with optional `blockPrefab`.
- Added generated child roots:
  - `Terrain Cells`
  - `Block Cells`
- Terrain tile views stay at ground level.
- Block views are separate from terrain tiles.
- Block views are pooled per cell by keeping deleted blocks inactive.
- `KayKitFbxAssetTest.unity` gained inactive `Prototype Block Source`.
- `TownGridView.blockPrefab` assigned to `Prototype Block Source`.
- `blockHeightStep` set to `0.38`.
- `blockScale` set to `(0.75, 0.35, 0.75)`.

Validation:

- `PrototypePlacementInputDriver.PlaceScreenCenter` returned `True`.
- Created/activated `Block 0,1 L1` under `Generated Town Cells/Block Cells`.
- Terrain `Cell 0,1` stayed under `Generated Town Cells/Terrain Cells` at local Y `0`.
- `PrototypePlacementInputDriver.DeleteScreenCenter` returned `True`.
- `Block 0,1 L1` remained present but inactive.
- Runtime generated terrain/block children did not persist after exiting Play Mode.
- `graphify update .` succeeded with 115 nodes, 140 edges, 17 communities.

Important historical commit:

- `37f43e3 Add pooled prototype block visuals`

## 2026-05-22 - Prototype Mode And Palette Controls

This is the latest detailed session as of the handover rewrite. It remains summarized here and is detailed in `HANDOVER.md`.

- Added `PrototypePlacementMode`.
- Added `PrototypePlacementState`.
- Added `PrototypePlacementControlsView`.
- Changed `PrototypePlacementInputDriver` to read placement state.
- Switched input driver to Unity Input System.
- Added `Unity.InputSystem` to `CozyBuilder.Runtime.asmdef`.
- `GameLifetimeScope` registers placement state and controls view.
- `KayKitFbxAssetTest.unity` gained `PrototypePlacementControlsView`.
- Unity compile/reload completed without C# compile errors.
- `PlaceScreenCenter` and `DeleteScreenCenter` returned `True` after the Input System fix.
- `git diff --check` passed.
- `graphify update .` succeeded with 132 nodes, 159 edges, 20 communities.

Important commit:

- `461c52e Add prototype placement controls`

## 2026-05-22 - Handover History Restructure

- Moved older detailed session updates out of `HANDOVER.md` into this archive log.
- Kept `HANDOVER.md` focused on compact project context plus the latest detailed session.
- Clarified startup flow:
  - read `CURRENT_STATUS.md` first
  - read `HANDOVER.md` only when deeper current context is needed
  - read this log only for old session history
- Preserved the prototype controls session as the latest detailed handover at the time of the rewrite.

Important commit:

- `9b14199 Restructure handover history`

## 2026-05-22 - Minimal Procedural Debug Views

- Added `PrototypeTownDebugState` for selected-cell debug state.
- Added `PrototypeTownDebugView` as a temporary IMGUI debug panel.
- Debug panel displays selected cell data, cardinal neighbors, dirty queue preview, and `PlacementService.Preview` rule result.
- Added `TownVisualRebuilder.CopyDirtyCoords` for bounded dirty queue debug snapshots.
- Updated `PrototypePlacementInputDriver` and `PrototypePlacementDebugDriver` to select the targeted coord in debug state.
- Updated `GameLifetimeScope` and `KayKitFbxAssetTest.unity` to wire the debug view.
- Unity compile/reload completed without C# compile errors.
- Play Mode validation returned `True` for input place/delete and debug-driver place/delete.
- `graphify update .` succeeded with 149 nodes, 189 edges, and 20 communities.

Important commit:

- `2a7824d Add prototype town debug view`

## 2026-05-22 - Prototype Camera Controls

- Expanded `CameraService` from an empty shell into a lightweight camera state service.
- Added `PrototypeCameraInputDriver` as the Unity Input System adapter for prototype camera controls.
- Camera controls:
  - `Alt + left drag`: orbit
  - middle drag: pan
  - mouse wheel: zoom
  - `R`: reset camera
  - two-finger touch drag: pan
  - two-finger pinch: zoom
- Updated `PrototypePlacementInputDriver` so `Alt + left` orbit does not also place a block.
- Updated `GameLifetimeScope` to register `PrototypeCameraInputDriver`.
- Updated `KayKitFbxAssetTest.unity` so `Main Camera` has `PrototypeCameraInputDriver`.

Validation:
- Unity compile/reload completed without C# compile errors.
- Play Mode validation:
  - `PrototypeCameraInputDriver.ResetCamera` returned `void` with status `ok`
  - `Main Camera` moved to `(0, 6.88895035, -11.0246248)` with rotation `(32, 0, 0)` after reset
- Console readback after Play Mode showed no project exceptions.
- `graphify update .` succeeded with 165 nodes, 217 edges, and 20 communities.

## 2026-05-22 - Procedural Rules System Integration

- Refactored `RuleResult.cs` by adding `RotationId` and a backward-compatible constructor.
- Implemented height stack and neighbor-aware procedural morphing rules in `RuleEvaluator.cs`:
  - **Waterfront Foundation (`VisualId = 4`)**: Stilts on waterfront grids at layer 1.
  - **Small House (`VisualId = 1`)**: Single standalone houses at the top layer with height 1.
  - **Tower Top (`VisualId = 3`)**: Pointy roofs if a tower stack is taller than all of its cardinal neighbors.
  - **Row Houses Roof (`VisualId = 2`)**: Connected roofs that automatically rotate (0°/90°) along East-West or North-South axes.
  - **House Wall (`VisualId = 5`)** & **Tower Wall (`VisualId = 6`)**: Under-layer walls depending on whether neighboring blocks share the same layer.
- Refactored `PlacementService.cs`'s `Preview` signature to pass cell coordinate and query neighborhood from `townDataStore.Current`.
- Fixed compiling issues in `PrototypeTownDebugView.cs` by aligning parameters to the new `Preview` signature.
- Redesigned `TownGridView.cs`'s visualization:
  - Replaced continuous GC-allocating instantiation with `Dictionary<ushort, Queue<GameObject>>` object pooling.
  - Utilized a zero-allocation `struct BlockViewData` (instead of classes) for tracking visual block instances.
  - Provided 6 custom SerializedField slots for specialized block prefabs with a safe fallback to `blockPrefab`.
  - Handled clean recycling of active block views when cells are refreshed or cleared.
  - Calculated and applied correct rotations for modular blocks based on `RotationId * 90f`.
- Documented and structured local implementation plan at `docs/plans/2026-05-22-procedural-rules.md` and finalized task progress checklist at `docs/plans/task.md`.

Validation:
- C# project compiles successfully with zero errors.
- Visual block morphing, auto-alignment, stilts base on water, and multi-level stack walls/roofs function exactly as designed.
- Dynamic pooling successfully prevents heap allocations in update cycles.
- `graphify update .` successfully updated the AST graph to 168 nodes, 225 edges, and 20 communities.

Important commits:
- `ced9487` docs: finalize procedural rules system task tracker
- `ac898fa` chore: update graphify AST graph after implementing procedural rules
- `bfcea17` docs: add procedural rules implementation plan inside the project
- `9c95296` docs: update task progress tracker to complete tasks 2-5
- `d2c7756` feat: implement zero-allocation pooling, rotation mapping, and specialized prefab slots in TownGridView
- `b1c6948` refactor: adjust preview method invocation parameters in PrototypeTownDebugView
- `3fcfea7` feat: implement height stack and neighbor-aware rules in RuleEvaluator
- `28c535d` refactor: update Preview signature in PlacementService to pass town data and cell coordinate

## 2026-05-22 - UI Pointer Blocking & Input Routing

- Added public `PanelRect` property getter to `PrototypePlacementControlsView` and `PrototypeTownDebugView` to expose IMGUI bounds.
- Implemented `IsPointerOverUI(Vector2 screenPosition)` inside `PrototypePlacementInputDriver` and `PrototypeCameraInputDriver`:
  - Checked `EventSystem.current.IsPointerOverGameObject()` for future-proofing against uGUI/UI Toolkit.
  - Checked if the cursor is within the bounds of the IMGUI panels by inverting the screen Y-axis (`guiPos = new Vector2(screenPos.x, Screen.height - screenPos.y)`) to match IMGUI's top-left origin.
- Blocked block placement and deletion inside `PrototypePlacementInputDriver` when the cursor is over any active UI panels.
- Blocked camera orbit, panning, and zoom inside `PrototypeCameraInputDriver` when pointer interaction starts over active UI panels:
  - Tracked click/touch start state using `wasDragStartedOverUI` and `wasTouchStartedOverUI` on the first frame of interaction (`wasPressedThisFrame`).
  - Allowed camera orbit/panning dragging to continue smoothly when the cursor crosses active UI panels, provided the interaction started outside the UI (Drag Continuity).
  - Blocked scroll wheel zoom when the pointer is positioned over active UI panels.
- Updated project plans, approved implementation plans, and unified task lists under `docs/plans/`.

Validation:
- C# project compiles cleanly without any errors or warnings.
- Real-time interaction verified in Play Mode: buttons, placement, deletion, camera panning, camera orbit, zoom, and touch interactions perform cleanly with robust pointer blocking.
- `graphify update .` successfully updated the AST graph to 172 nodes, 235 edges, and 21 communities.

Important commits:
- `df2297a` feat: implement UI pointer blocking and input routing for camera and placement drivers

## 2026-05-22 - Camera Assembly Decoupling & Modularization

- Decoupled and modularized the Camera system by separating all camera logic into an independent, reusable Assembly Definition (`CozyBuilder.Camera.asmdef`) with zero references to the main `CozyBuilder.Runtime` assembly.
- Introduced a modular `ICameraInputBlocker` interface in the `CozyBuilder.Camera` assembly, allowing any runtime UI view to register itself as an interaction blocker without the camera having direct knowledge of it.
- Refactored `PrototypeCameraInputDriver.cs` to inject `IReadOnlyList<ICameraInputBlocker>` dynamically via VContainer and dynamically check all registered blockers.
- Implemented `ICameraInputBlocker` on IMGUI views `PrototypePlacementControlsView` and `PrototypeTownDebugView`.
- Configured Dependency Injection in `GameLifetimeScope.cs` to bind these views as `ICameraInputBlocker`.
- Ensured zero GC allocation in the blocker checking loop by using a standard `for` loop to iterate through the blockers list instead of `foreach` queries.
- Verified that the codebase compiles flawlessly and manual testing in Play Mode confirms that the decoupled camera and placement blocking work seamlessly.
- `graphify update .` successfully updated the AST graph to 180 nodes, 243 edges, and 21 communities.

Important commits:
- `2afd61a` docs: update task list to complete camera decoupling implementation
- `9323507` refactor: update AST code graph and meta files for decoupled camera module
- `3e2455c` feat: update VContainer registrations for UI views to act as ICameraInputBlocker
- `6cc839f` feat: implement ICameraInputBlocker on prototype IMGUI views
- `c8ef8e9` refactor: decouple PrototypeCameraInputDriver from runtime UI panels using ICameraInputBlocker
