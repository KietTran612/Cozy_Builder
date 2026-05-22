using System.Collections.Generic;
using CozyBuilder.Town.Data;
using UnityEngine;
using VContainer;

namespace CozyBuilder.Town.Rendering
{
    public sealed class TownGridView : MonoBehaviour
    {
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private GameObject blockPrefab;
        [SerializeField] private GameObject smallHousePrefab;
        [SerializeField] private GameObject houseRoofPrefab;
        [SerializeField] private GameObject towerTopPrefab;
        [SerializeField] private GameObject stiltsPrefab;
        [SerializeField] private GameObject houseWallPrefab;
        [SerializeField] private GameObject towerWallPrefab;
        [SerializeField] private Transform generatedRoot;
        [SerializeField] private float cellSpacing = 2.1f;
        [SerializeField] private float blockHeightStep = 0.35f;
        [SerializeField] private Vector3 blockScale = new Vector3(0.65f, 0.35f, 0.65f);
        [SerializeField] private int maxDirtyCellsPerFrame = 16;
        [SerializeField] private bool rebuildOnStart = true;

        private readonly Dictionary<GridCoord, GameObject> cellsByCoord = new Dictionary<GridCoord, GameObject>();
        private readonly Dictionary<GridCoord, CellVisualState> visualStatesByCoord = new Dictionary<GridCoord, CellVisualState>();
        private readonly Dictionary<ushort, Queue<GameObject>> pools = new Dictionary<ushort, Queue<GameObject>>();
        private TownDataStore townDataStore;
        private TownVisualRebuilder townVisualRebuilder;
        private RuleEvaluator ruleEvaluator;
        private Transform terrainRoot;
        private Transform blockRoot;
        private bool hasBuiltInitialGrid;

        private struct BlockViewData
        {
            public GameObject GameObject;
            public ushort VisualId;
        }

        private sealed class CellVisualState
        {
            public GameObject TerrainView;
            public readonly List<BlockViewData> BlockViews = new List<BlockViewData>();
        }

        [Inject]
        public void Construct(
            TownDataStore townDataStore,
            TownVisualRebuilder townVisualRebuilder,
            RuleEvaluator ruleEvaluator)
        {
            this.townDataStore = townDataStore;
            this.townVisualRebuilder = townVisualRebuilder;
            this.ruleEvaluator = ruleEvaluator;
        }

        private void Start()
        {
            if (rebuildOnStart)
            {
                RebuildInitialGrid();
            }
        }

        private void LateUpdate()
        {
            ProcessDirtyCells(maxDirtyCellsPerFrame);
        }

        public void RebuildInitialGrid()
        {
            if (townDataStore == null || townVisualRebuilder == null || cellPrefab == null)
            {
                Debug.LogWarning("TownGridView requires injected services and a cell prefab.", this);
                return;
            }

            EnsureRoot();
            ClearGeneratedCells();
            EnsureVisualRoots();

            var townData = townDataStore.Current;
            for (var i = 0; i < townData.CellCount; i++)
            {
                var coord = townData.Coordinates[i];
                var cell = townData.Cells[i];
                CreateCellView(coord, in cell);
            }

            hasBuiltInitialGrid = true;
        }

        public int ProcessDirtyCells(int maxCells)
        {
            if (!hasBuiltInitialGrid || townDataStore == null || townVisualRebuilder == null)
            {
                return 0;
            }

            var processed = 0;
            var limit = Mathf.Max(0, maxCells);

            while (processed < limit && townVisualRebuilder.TryDequeueDirty(out var coord))
            {
                RefreshCell(coord);
                processed++;
            }

            return processed;
        }

        public bool TryGetCellView(GridCoord coord, out GameObject cellView)
        {
            return cellsByCoord.TryGetValue(coord, out cellView);
        }

        public bool TryGetCoordFromWorld(Vector3 worldPosition, out GridCoord coord)
        {
            if (townDataStore == null)
            {
                coord = default;
                return false;
            }

            var localPosition = generatedRoot != null
                ? generatedRoot.InverseTransformPoint(worldPosition)
                : transform.InverseTransformPoint(worldPosition);
            var x = Mathf.RoundToInt(localPosition.x / cellSpacing);
            var y = Mathf.RoundToInt(localPosition.z / cellSpacing);
            coord = new GridCoord(x, y);
            return townDataStore.Current.Contains(coord);
        }

        public bool RefreshCell(GridCoord coord)
        {
            if (!townDataStore.Current.TryGetCell(coord, out var cell))
            {
                if (cellsByCoord.TryGetValue(coord, out var staleView))
                {
                    staleView.SetActive(false);
                }

                if (visualStatesByCoord.TryGetValue(coord, out var staleState))
                {
                    for (var i = 0; i < staleState.BlockViews.Count; i++)
                    {
                        ReturnToPool(staleState.BlockViews[i].VisualId, staleState.BlockViews[i].GameObject);
                    }
                    staleState.BlockViews.Clear();
                }

                return false;
            }

            if (!visualStatesByCoord.TryGetValue(coord, out var state))
            {
                CreateCellView(coord, in cell);
                return true;
            }

            ApplyCellState(state, coord, in cell);
            return true;
        }

        private void EnsureRoot()
        {
            if (generatedRoot == null)
            {
                var root = new GameObject("Generated Town Cells");
                root.transform.SetParent(transform, false);
                generatedRoot = root.transform;
            }
        }

        private void EnsureVisualRoots()
        {
            terrainRoot = EnsureChildRoot("Terrain Cells");
            blockRoot = EnsureChildRoot("Block Cells");
        }

        private void ClearGeneratedCells()
        {
            for (var i = generatedRoot.childCount - 1; i >= 0; i--)
            {
                Destroy(generatedRoot.GetChild(i).gameObject);
            }

            cellsByCoord.Clear();
            visualStatesByCoord.Clear();
            pools.Clear();
            terrainRoot = null;
            blockRoot = null;
        }

        private void CreateCellView(GridCoord coord, in CellData cell)
        {
            EnsureRoot();
            EnsureVisualRoots();

            var terrainView = Instantiate(cellPrefab, terrainRoot);
            terrainView.transform.localRotation = Quaternion.identity;

            var state = new CellVisualState
            {
                TerrainView = terrainView
            };

            cellsByCoord.Add(coord, terrainView);
            visualStatesByCoord.Add(coord, state);
            ApplyCellState(state, coord, in cell);
        }

        private void ApplyCellState(GameObject terrainView, GridCoord coord, in CellData cell)
        {
            terrainView.name = $"Cell {coord.X},{coord.Y}";
            terrainView.transform.localPosition = GridToWorld(coord);
            terrainView.transform.localScale = Vector3.one;
            terrainView.SetActive(cell.Terrain != TerrainType.None);
        }

        private void ApplyCellState(CellVisualState state, GridCoord coord, in CellData cell)
        {
            ApplyCellState(state.TerrainView, coord, in cell);
            ApplyBlockState(state, coord, in cell);
        }

        private void ApplyBlockState(CellVisualState state, GridCoord coord, in CellData cell)
        {
            ushort height = cell.Height;
            var townData = townDataStore.Current;

            // 1. Process layers up to height
            for (int i = 0; i < height; i++)
            {
                int layer = i + 1;
                var rule = ruleEvaluator.Evaluate(coord, layer, in cell, townData);
                ushort targetVisualId = rule.VisualId;

                if (i < state.BlockViews.Count)
                {
                    // Existing block layer - check if visual type matches
                    var existing = state.BlockViews[i];
                    if (existing.VisualId == targetVisualId)
                    {
                        // Same visual type, just update transform & name
                        var blockView = existing.GameObject;
                        blockView.name = $"Block {coord.X},{coord.Y} L{layer} V{targetVisualId}";
                        blockView.transform.localPosition = GridToWorld(coord, (ushort)layer);
                        blockView.transform.localRotation = Quaternion.Euler(0f, rule.RotationId * 90f, 0f);
                        blockView.transform.localScale = blockScale;
                    }
                    else
                    {
                        // Mismatching visual type - recycle and replace
                        ReturnToPool(existing.VisualId, existing.GameObject);
                        var newObj = GetPooledBlock(targetVisualId);
                        newObj.name = $"Block {coord.X},{coord.Y} L{layer} V{targetVisualId}";
                        newObj.transform.localPosition = GridToWorld(coord, (ushort)layer);
                        newObj.transform.localRotation = Quaternion.Euler(0f, rule.RotationId * 90f, 0f);
                        newObj.transform.localScale = blockScale;

                        state.BlockViews[i] = new BlockViewData
                        {
                            GameObject = newObj,
                            VisualId = targetVisualId
                        };
                    }
                }
                else
                {
                    // New layer - get pooled block and add to list
                    var newObj = GetPooledBlock(targetVisualId);
                    newObj.name = $"Block {coord.X},{coord.Y} L{layer} V{targetVisualId}";
                    newObj.transform.localPosition = GridToWorld(coord, (ushort)layer);
                    newObj.transform.localRotation = Quaternion.Euler(0f, rule.RotationId * 90f, 0f);
                    newObj.transform.localScale = blockScale;

                    state.BlockViews.Add(new BlockViewData
                    {
                        GameObject = newObj,
                        VisualId = targetVisualId
                    });
                }
            }

            // 2. Recycle any excess blocks if height decreased
            if (height < state.BlockViews.Count)
            {
                for (int i = height; i < state.BlockViews.Count; i++)
                {
                    ReturnToPool(state.BlockViews[i].VisualId, state.BlockViews[i].GameObject);
                }
                state.BlockViews.RemoveRange(height, state.BlockViews.Count - height);
            }
        }

        private GameObject GetPooledBlock(ushort visualId)
        {
            if (!pools.TryGetValue(visualId, out var queue))
            {
                queue = new Queue<GameObject>();
                pools.Add(visualId, queue);
            }

            if (queue.Count > 0)
            {
                var obj = queue.Dequeue();
                if (obj != null)
                {
                    obj.SetActive(true);
                    return obj;
                }
            }

            GameObject prefab = GetPrefabForVisualId(visualId);
            var newObj = Instantiate(prefab, blockRoot);
            newObj.SetActive(true);
            return newObj;
        }

        private void ReturnToPool(ushort visualId, GameObject obj)
        {
            if (obj == null) return;

            obj.SetActive(false);
            obj.transform.SetParent(blockRoot, false);

            if (!pools.TryGetValue(visualId, out var queue))
            {
                queue = new Queue<GameObject>();
                pools.Add(visualId, queue);
            }
            queue.Enqueue(obj);
        }

        private GameObject GetPrefabForVisualId(ushort visualId)
        {
            GameObject prefab = null;
            switch (visualId)
            {
                case 1: prefab = smallHousePrefab; break;
                case 2: prefab = houseRoofPrefab; break;
                case 3: prefab = towerTopPrefab; break;
                case 4: prefab = stiltsPrefab; break;
                case 5: prefab = houseWallPrefab; break;
                case 6: prefab = towerWallPrefab; break;
            }

            if (prefab == null)
            {
                prefab = blockPrefab;
            }
            return prefab;
        }

        private Vector3 GridToWorld(GridCoord coord, ushort height)
        {
            return new Vector3(coord.X * cellSpacing, height * blockHeightStep, coord.Y * cellSpacing);
        }

        private Vector3 GridToWorld(GridCoord coord)
        {
            return GridToWorld(coord, 0);
        }

        private Transform EnsureChildRoot(string rootName)
        {
            var child = generatedRoot.Find(rootName);
            if (child != null)
            {
                return child;
            }

            var root = new GameObject(rootName);
            root.transform.SetParent(generatedRoot, false);
            return root.transform;
        }
    }
}
