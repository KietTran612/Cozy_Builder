# CURRENT STATUS

This is the short startup context for agents. Read this first, then use `HANDOVER.md` only when deeper history is needed.

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
- Prototype Core data foundation has started.

## Current Commit Baseline

- Latest committed baseline observed in this session: `ee1392d Add Unity MCP workflow and startup context`.
- Check `git log -1 --oneline` and `git status --short` for the latest committed/uncommitted state.
- Some `docs/*.md` files may appear modified from line-ending noise; do not stage them unless their content was intentionally changed.
- Current uncommitted work includes KayKit test scene changes, Prototype Core data/service changes, Graphify output refresh, and local screenshot output.
- Local-only/untracked MCP package files may appear under `Cozy_Builder/Packages/io.realvirtual.mcp/`; do not commit them unless project policy changes.

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

- Code is still foundation only, but Prototype Core data work has begun.
- `GameLifetimeScope` should register system-level prototype services only.
- Data must remain separate from scene GameObjects.
- No static singleton gameplay services.
- KayKit should be used as prototype terrain/grid placeholder content, not as the final procedural building foundation.
- Current prototype direction is data-first island grid and placement/delete services before user input and visual runtime adapters.

## Next Work

1. Build the first visual adapter for Prototype Core:
   - read `TownDataStore.Current`
   - instantiate or otherwise display KayKit tile placeholders for initial island cells
   - keep scene objects as visual output, not source of truth
2. Add a minimal runtime driver/MonoBehaviour adapter only after the data-to-visual path is clear.
3. Add tap/click placement and delete mode against `PlacementService`.
4. Add a basic palette using `ColorId`/`MaterialId`, not runtime material instances.
5. Add minimal procedural rule/debug views:
   - cell id/neighbor info
   - dirty cell queue
   - rule result preview
6. Then add camera orbit/pan/zoom.

## Latest Validation Notes

- Unity MCP connected successfully while Unity Editor was open.
- `KayKitFbxAssetTest.unity` opened and compiled in Unity `6000.3.11f1`.
- KayKit FBX samples render in URP without magenta/missing-shader materials.
- KayKit license file confirms CC0 and commercial use.
- KayKit inventory is strong for terrain/grid tests: hex/square tiles, road, water, water corners/straights, forest/rock/sand variations.
- KayKit object inventory includes buildings and walls, but mostly as whole objects; it is not yet suitable as a main wall/roof procedural building foundation.
- Wall segment spacing at 2m aligns cleanly in the scene test.
- Whole-house objects are useful for scale/mood placeholders but do not behave like clean wall/roof modules.

## Latest Code Notes

- `TownDataStore.Current` now initializes an organic island grid with radius 4.
- `TownData` now owns coordinates, cell data, and coordinate-to-index lookup.
- `PlacementService` now has data-first `TryPlaceBlock` and `TryDeleteBlock` APIs.
- Placement/delete marks the changed cell and cardinal neighbors dirty.
- `TownVisualRebuilder` now has a deduplicated dirty queue foundation.
- Unity compile completed without C# errors after these code changes.
- `graphify update .` succeeded after code changes and updated `graphify-out/`.

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
- Read `HANDOVER.md` only for deeper history or when current status is unclear.
- Do not read every doc linked from `HANDOVER.md`.
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
