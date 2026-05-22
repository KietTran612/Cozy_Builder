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
        [SerializeField] private Transform generatedRoot;
        [SerializeField] private float cellSpacing = 2.1f;
        [SerializeField] private float blockHeightStep = 0.35f;
        [SerializeField] private Vector3 blockScale = new Vector3(0.65f, 0.35f, 0.65f);
        [SerializeField] private int maxDirtyCellsPerFrame = 16;
        [SerializeField] private bool rebuildOnStart = true;

        private readonly Dictionary<GridCoord, GameObject> cellsByCoord = new Dictionary<GridCoord, GameObject>();
        private readonly Dictionary<GridCoord, CellVisualState> visualStatesByCoord = new Dictionary<GridCoord, CellVisualState>();
        private TownDataStore townDataStore;
        private TownVisualRebuilder townVisualRebuilder;
        private Transform terrainRoot;
        private Transform blockRoot;
        private bool hasBuiltInitialGrid;

        private sealed class CellVisualState
        {
            public GameObject TerrainView;
            public readonly List<GameObject> BlockViews = new List<GameObject>();
        }

        [Inject]
        public void Construct(TownDataStore townDataStore, TownVisualRebuilder townVisualRebuilder)
        {
            this.townDataStore = townDataStore;
            this.townVisualRebuilder = townVisualRebuilder;
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
                        staleState.BlockViews[i].SetActive(false);
                    }
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
            ApplyBlockState(state, coord, cell.Height);
        }

        private void ApplyBlockState(CellVisualState state, GridCoord coord, ushort height)
        {
            EnsureBlockCapacity(state, coord, height);

            for (var i = 0; i < state.BlockViews.Count; i++)
            {
                var blockView = state.BlockViews[i];
                var active = i < height;
                blockView.SetActive(active);

                if (!active)
                {
                    continue;
                }

                blockView.name = $"Block {coord.X},{coord.Y} L{i + 1}";
                blockView.transform.localPosition = GridToWorld(coord, (ushort)(i + 1));
                blockView.transform.localRotation = Quaternion.identity;
                blockView.transform.localScale = blockScale;
            }
        }

        private void EnsureBlockCapacity(CellVisualState state, GridCoord coord, ushort height)
        {
            if (blockPrefab == null)
            {
                return;
            }

            while (state.BlockViews.Count < height)
            {
                var blockView = Instantiate(blockPrefab, blockRoot);
                blockView.name = $"Block {coord.X},{coord.Y} Pooled";
                blockView.SetActive(false);
                state.BlockViews.Add(blockView);
            }
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
