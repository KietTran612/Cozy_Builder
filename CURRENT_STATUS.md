# CURRENT STATUS

This is the short startup context for agents. Read this first, then use `HANDOVER.md` only when deeper history is needed.

## Product Direction

- Unity mobile game in the cozy procedural town builder space.
- Core fantasy: place blocks slowly and watch a small peaceful town take shape.
- Not a resource-management city builder.
- Inspired by Townscaper research, but must not become a direct clone.
- Differentiators planned over time: clearer terrain strategy, controlled grid/seed, ambient life, possible street/walk view, mobile-first performance.

## Current Phase

- Phase 0 / early prototype foundation.
- Unity project exists at `Cozy_Builder`.
- Unity version: `6000.3.11f1`.
- Render pipeline: URP.
- DI: VContainer.
- Async package: UniTask.
- First imported/test asset pack: KayKit Medieval Builder Pack 1.0.

## Current Commit Baseline

- Latest committed code foundation commit: `124772e Add Unity code foundation and KayKit test scene`.
- Check `git log -1 --oneline` and `git status --short` for the latest committed/uncommitted state.
- Some `docs/*.md` files may appear modified from line-ending noise; do not stage them unless their content was intentionally changed.

## What Exists

- Project runtime root: `Cozy_Builder/Assets/CozyBuilder/Runtime`.
- Runtime assembly: `Cozy_Builder/Assets/CozyBuilder/Runtime/CozyBuilder.Runtime.asmdef`.
- Composition root: `Cozy_Builder/Assets/CozyBuilder/Runtime/Bootstrap/GameLifetimeScope.cs`.
- Initial data/services:
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
- KayKit FBX test scene: `Cozy_Builder/Assets/CozyBuilder/Scenes/KayKitFbxAssetTest.unity`.
- Graphify output exists at `graphify-out/` and is maintained with `graphify update .`.

## Current Intent

- Code is foundation only, not gameplay implementation.
- `GameLifetimeScope` should register system-level prototype services only.
- Data must remain separate from scene GameObjects.
- No static singleton gameplay services.
- KayKit test scene is for scale/material/URP/modularity inspection before prototype placement work.

## Next Work

1. Open `KayKitFbxAssetTest.unity` in Unity.
2. Verify Unity import/compile and Console status.
3. Inspect KayKit FBX scale, material, URP compatibility, modularity, and procedural suitability.
4. Only after the asset test scene is stable, begin Prototype Core:
   - organic island grid
   - tap/click placement
   - delete mode
   - basic palette
   - minimal procedural wall/roof/tower rules
   - camera orbit/pan/zoom
   - debug cell/neighbor/rule view

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
