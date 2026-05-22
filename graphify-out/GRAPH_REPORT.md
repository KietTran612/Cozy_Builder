# Graph Report - App  (2026-05-22)

## Corpus Check
- 20 files · ~30,103 words
- Verdict: corpus is large enough that graph structure adds value.

## Summary
- 132 nodes · 159 edges · 20 communities (10 shown, 10 thin omitted)
- Extraction: 100% EXTRACTED · 0% INFERRED · 0% AMBIGUOUS
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `37f43e3e`
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
- [[_COMMUNITY_Community 13|Community 13]]
- [[_COMMUNITY_Community 14|Community 14]]
- [[_COMMUNITY_Community 15|Community 15]]
- [[_COMMUNITY_Community 16|Community 16]]
- [[_COMMUNITY_Community 17|Community 17]]
- [[_COMMUNITY_Community 18|Community 18]]
- [[_COMMUNITY_Community 19|Community 19]]

## God Nodes (most connected - your core abstractions)
1. `TownGridView` - 28 edges
2. `PrototypePlacementInputDriver` - 17 edges
3. `PlacementService` - 9 edges
4. `PrototypePlacementControlsView` - 9 edges
5. `PrototypePlacementDebugDriver` - 8 edges
6. `TownData` - 7 edges
7. `OrganicIslandGridGenerator` - 5 edges
8. `int` - 5 edges
9. `TownVisualRebuilder` - 5 edges
10. `PrototypePlacementState` - 4 edges

## Surprising Connections (you probably didn't know these)
- `PrototypePlacementInputDriver` --references--> `ushort`  [EXTRACTED]
  Town/Placement/PrototypePlacementInputDriver.cs → Town/Placement/PrototypePlacementDebugDriver.cs
- `TownData` --references--> `Dictionary`  [EXTRACTED]
  Town/Data/TownData.cs → Town/Rendering/TownGridView.cs
- `TownDataStore` --references--> `int`  [EXTRACTED]
  Town/Data/TownDataStore.cs → Town/Rendering/TownGridView.cs
- `PrototypePlacementControlsView` --references--> `int`  [EXTRACTED]
  Town/Placement/PrototypePlacementControlsView.cs → Town/Rendering/TownGridView.cs
- `PrototypePlacementDebugDriver` --references--> `int`  [EXTRACTED]
  Town/Placement/PrototypePlacementDebugDriver.cs → Town/Rendering/TownGridView.cs

## Communities (20 total, 10 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.18
Nodes (4): float, TownGridView, Transform, Vector3

### Community 1 - "Community 1"
Cohesion: 0.19
Nodes (6): bool, CozyBuilder.Town.Placement, PrototypePlacementInputDriver, Plane, TownGridView, UnityCamera

### Community 2 - "Community 2"
Cohesion: 0.17
Nodes (6): CozyBuilder.Town.Data, TownData, CozyBuilder.Town.Data, TownDataStore, Dictionary, int

### Community 3 - "Community 3"
Cohesion: 0.24
Nodes (5): CozyBuilder.Town.Placement, PlacementService, RuleEvaluator, TownDataStore, TownVisualRebuilder

### Community 4 - "Community 4"
Cohesion: 0.28
Nodes (4): CozyBuilder.Town.Placement, PrototypePlacementControlsView, PrototypePlacementState, Rect

### Community 5 - "Community 5"
Cohesion: 0.22
Nodes (5): MonoBehaviour, CozyBuilder.Town.Placement, PrototypePlacementDebugDriver, PlacementService, ushort

### Community 7 - "Community 7"
Cohesion: 0.29
Nodes (4): HashSet, Queue, CozyBuilder.Town.Rendering, TownVisualRebuilder

### Community 9 - "Community 9"
Cohesion: 0.4
Nodes (3): CozyBuilder.Bootstrap, GameLifetimeScope, LifetimeScope

### Community 10 - "Community 10"
Cohesion: 0.4
Nodes (4): GameObject, List, CellVisualState, CozyBuilder.Town.Rendering

### Community 12 - "Community 12"
Cohesion: 0.5
Nodes (3): CozyBuilder.Town.Data, GridNeighborhood, GridCoord

## Knowledge Gaps
- **32 isolated node(s):** `CozyBuilder.Bootstrap`, `CozyBuilder.Camera`, `CameraService`, `CozyBuilder.Town.Data`, `CozyBuilder.Town.Data` (+27 more)
  These have ≤1 connection - possible missing edges or undocumented components.
- **10 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `TownGridView` connect `Community 0` to `Community 1`, `Community 2`, `Community 3`, `Community 5`, `Community 10`?**
  _High betweenness centrality (0.266) - this node is a cross-community bridge._
- **Why does `PrototypePlacementInputDriver` connect `Community 1` to `Community 4`, `Community 5`?**
  _High betweenness centrality (0.121) - this node is a cross-community bridge._
- **Why does `int` connect `Community 2` to `Community 0`, `Community 4`, `Community 5`?**
  _High betweenness centrality (0.105) - this node is a cross-community bridge._
- **What connects `CozyBuilder.Bootstrap`, `CozyBuilder.Camera`, `CameraService` to the rest of the system?**
  _32 weakly-connected nodes found - possible documentation gaps or missing edges._