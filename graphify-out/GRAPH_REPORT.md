# Graph Report - App  (2026-05-22)

## Corpus Check
- 23 files · ~29,649 words
- Verdict: corpus is large enough that graph structure adds value.

## Summary
- 165 nodes · 217 edges · 20 communities (10 shown, 10 thin omitted)
- Extraction: 100% EXTRACTED · 0% INFERRED · 0% AMBIGUOUS
- Token cost: 0 input · 0 output

## Graph Freshness
- Built from commit: `2a7824d4`
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
2. `PrototypePlacementInputDriver` - 18 edges
3. `PrototypeTownDebugView` - 16 edges
4. `PrototypeCameraInputDriver` - 13 edges
5. `PlacementService` - 9 edges
6. `PrototypePlacementControlsView` - 9 edges
7. `PrototypePlacementDebugDriver` - 9 edges
8. `CameraService` - 8 edges
9. `TownData` - 7 edges
10. `int` - 6 edges

## Surprising Connections (you probably didn't know these)
- `CameraService` --references--> `Vector3`  [EXTRACTED]
  Camera/CameraService.cs → Town/Rendering/TownGridView.cs
- `PrototypeCameraInputDriver` --references--> `float`  [EXTRACTED]
  Camera/PrototypeCameraInputDriver.cs → Town/Rendering/TownGridView.cs
- `CameraService` --references--> `float`  [EXTRACTED]
  Camera/CameraService.cs → Town/Rendering/TownGridView.cs
- `PrototypeCameraInputDriver` --references--> `Vector3`  [EXTRACTED]
  Camera/PrototypeCameraInputDriver.cs → Town/Rendering/TownGridView.cs
- `PrototypeCameraInputDriver` --references--> `UnityCamera`  [EXTRACTED]
  Camera/PrototypeCameraInputDriver.cs → Town/Placement/PrototypePlacementInputDriver.cs

## Communities (20 total, 10 thin omitted)

### Community 0 - "Community 0"
Cohesion: 0.14
Nodes (7): bool, GameObject, List, CellVisualState, CozyBuilder.Town.Rendering, TownGridView, Transform

### Community 1 - "Community 1"
Cohesion: 0.14
Nodes (8): MonoBehaviour, CozyBuilder.Town.Placement, PrototypePlacementDebugDriver, PrototypePlacementInputDriver, Plane, PrototypeTownDebugState, TownGridView, ushort

### Community 2 - "Community 2"
Cohesion: 0.17
Nodes (8): CozyBuilder.Town.Data, GridNeighborhood, CozyBuilder.Town.Debugging, PrototypeTownDebugView, GridCoord, PlacementService, StringBuilder, TownDataStore

### Community 3 - "Community 3"
Cohesion: 0.2
Nodes (6): CozyBuilder.Camera, PrototypeCameraInputDriver, CameraService, CozyBuilder.Town.Placement, UnityCamera, Vector3

### Community 4 - "Community 4"
Cohesion: 0.17
Nodes (6): CozyBuilder.Town.Data, TownData, CozyBuilder.Town.Data, TownDataStore, Dictionary, int

### Community 5 - "Community 5"
Cohesion: 0.27
Nodes (4): CozyBuilder.Town.Placement, PlacementService, RuleEvaluator, TownVisualRebuilder

### Community 6 - "Community 6"
Cohesion: 0.22
Nodes (3): CameraService, CozyBuilder.Camera, float

### Community 7 - "Community 7"
Cohesion: 0.28
Nodes (4): CozyBuilder.Town.Placement, PrototypePlacementControlsView, PrototypePlacementState, Rect

### Community 8 - "Community 8"
Cohesion: 0.25
Nodes (4): HashSet, Queue, CozyBuilder.Town.Rendering, TownVisualRebuilder

### Community 11 - "Community 11"
Cohesion: 0.4
Nodes (3): CozyBuilder.Bootstrap, GameLifetimeScope, LifetimeScope

## Knowledge Gaps
- **32 isolated node(s):** `CozyBuilder.Bootstrap`, `CozyBuilder.Camera`, `CozyBuilder.Camera`, `CameraService`, `CozyBuilder.Town.Data` (+27 more)
  These have ≤1 connection - possible missing edges or undocumented components.
- **10 thin communities (<3 nodes) omitted from report** — run `graphify query` to explore isolated nodes.

## Suggested Questions
_Questions this graph is uniquely positioned to answer:_

- **Why does `TownGridView` connect `Community 0` to `Community 1`, `Community 2`, `Community 3`, `Community 4`, `Community 5`, `Community 6`?**
  _High betweenness centrality (0.253) - this node is a cross-community bridge._
- **Why does `PrototypeTownDebugView` connect `Community 2` to `Community 1`, `Community 4`, `Community 5`, `Community 7`?**
  _High betweenness centrality (0.139) - this node is a cross-community bridge._
- **Why does `PrototypePlacementInputDriver` connect `Community 1` to `Community 0`, `Community 2`, `Community 3`, `Community 7`?**
  _High betweenness centrality (0.108) - this node is a cross-community bridge._
- **What connects `CozyBuilder.Bootstrap`, `CozyBuilder.Camera`, `CozyBuilder.Camera` to the rest of the system?**
  _32 weakly-connected nodes found - possible documentation gaps or missing edges._
- **Should `Community 0` be split into smaller, more focused modules?**
  _Cohesion score 0.14 - nodes in this community are weakly interconnected._
- **Should `Community 1` be split into smaller, more focused modules?**
  _Cohesion score 0.14 - nodes in this community are weakly interconnected._