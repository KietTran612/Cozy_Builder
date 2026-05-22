# Graph Report - App  (2026-05-22)

## Corpus Check
- 14 files · ~26,008 words
- Verdict: corpus is large enough that graph structure adds value.

## Summary
- 65 nodes · 58 edges · 13 communities (5 shown, 8 thin omitted)
- Extraction: 100% EXTRACTED · 0% INFERRED · 0% AMBIGUOUS
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `ee1392de`
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
- [[_COMMUNITY_Community 10|Community 10]]
- [[_COMMUNITY_Community 11|Community 11]]
- [[_COMMUNITY_Community 12|Community 12]]

## God Nodes (most connected - your core abstractions)
1. `PlacementService` - 9 edges
2. `TownData` - 7 edges
3. `OrganicIslandGridGenerator` - 5 edges
4. `TownVisualRebuilder` - 5 edges
5. `GameLifetimeScope` - 3 edges
6. `GridNeighborhood` - 2 edges
7. `int` - 2 edges
8. `TownDataStore` - 2 edges
9. `RuleEvaluator` - 2 edges
10. `CozyBuilder.Bootstrap` - 1 edges

## Surprising Connections (you probably didn't know these)
- `TownData` --references--> `int`  [EXTRACTED]
  Town/Data/TownData.cs → Town/Data/TownDataStore.cs

## Communities (13 total, 8 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.17
Nodes (6): CozyBuilder.Town.Data, TownData, CozyBuilder.Town.Data, TownDataStore, Dictionary, int

### Community 1 - "Community 1"
Cohesion: 0.24
Nodes (5): CozyBuilder.Town.Placement, PlacementService, RuleEvaluator, TownDataStore, TownVisualRebuilder

### Community 3 - "Community 3"
Cohesion: 0.29
Nodes (4): HashSet, Queue, CozyBuilder.Town.Rendering, TownVisualRebuilder

### Community 4 - "Community 4"
Cohesion: 0.4
Nodes (3): CozyBuilder.Bootstrap, GameLifetimeScope, LifetimeScope

### Community 6 - "Community 6"
Cohesion: 0.5
Nodes (3): CozyBuilder.Town.Data, GridNeighborhood, GridCoord

## Knowledge Gaps
- **22 isolated node(s):** `CozyBuilder.Bootstrap`, `CozyBuilder.Camera`, `CameraService`, `CozyBuilder.Town.Data`, `CozyBuilder.Town.Data` (+17 more)
  These have ≤1 connection - possible missing edges or undocumented components.
- **8 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **What connects `CozyBuilder.Bootstrap`, `CozyBuilder.Camera`, `CameraService` to the rest of the system?**
  _22 weakly-connected nodes found - possible documentation gaps or missing edges._