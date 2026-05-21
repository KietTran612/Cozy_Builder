# Graph Report - App  (2026-05-21)

## Corpus Check
- 11 files · ~25,120 words
- Verdict: corpus is large enough that graph structure adds value.

## Summary
- 41 nodes · 31 edges · 10 communities (3 shown, 7 thin omitted)
- Extraction: 100% EXTRACTED · 0% INFERRED · 0% AMBIGUOUS
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `124772e3`
- Run `git rev-parse HEAD` and compare to check if the graph is stale.
- Run `graphify update .` after code changes (no API cost).

## Community Hubs (Navigation)
- [[_COMMUNITY_Community 0|Community 0]]
- [[_COMMUNITY_Community 1|Community 1]]
- [[_COMMUNITY_Community 2|Community 2]]
- [[_COMMUNITY_Community 3|Community 3]]
- [[_COMMUNITY_Community 4|Community 4]]
- [[_COMMUNITY_Community 5|Community 5]]
- [[_COMMUNITY_Community 6|Community 6]]
- [[_COMMUNITY_Community 7|Community 7]]
- [[_COMMUNITY_Community 8|Community 8]]
- [[_COMMUNITY_Community 9|Community 9]]

## God Nodes (most connected - your core abstractions)
1. `PlacementService` - 6 edges
2. `GameLifetimeScope` - 3 edges
3. `TownData` - 2 edges
4. `int` - 2 edges
5. `TownDataStore` - 2 edges
6. `TownVisualRebuilder` - 2 edges
7. `RuleEvaluator` - 2 edges
8. `CozyBuilder.Bootstrap` - 1 edges
9. `CozyBuilder.Camera` - 1 edges
10. `CameraService` - 1 edges

## Surprising Connections (you probably didn't know these)
- `TownData` --references--> `int`  [EXTRACTED]
  Town/Data/TownData.cs → Town/Data/TownDataStore.cs

## Communities (10 total, 7 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.25
Nodes (5): CozyBuilder.Town.Placement, PlacementService, RuleEvaluator, TownDataStore, TownVisualRebuilder

### Community 1 - "Community 1"
Cohesion: 0.29
Nodes (5): CozyBuilder.Town.Data, TownData, CozyBuilder.Town.Data, TownDataStore, int

### Community 2 - "Community 2"
Cohesion: 0.4
Nodes (3): CozyBuilder.Bootstrap, GameLifetimeScope, LifetimeScope

## Knowledge Gaps
- **15 isolated node(s):** `CozyBuilder.Bootstrap`, `CozyBuilder.Camera`, `CameraService`, `CozyBuilder.Town.Data`, `CozyBuilder.Town.Data` (+10 more)
  These have ≤1 connection - possible missing edges or undocumented components.
- **7 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **What connects `CozyBuilder.Bootstrap`, `CozyBuilder.Camera`, `CameraService` to the rest of the system?**
  _15 weakly-connected nodes found - possible documentation gaps or missing edges._