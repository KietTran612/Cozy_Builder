using System.Collections.Generic;
using CozyBuilder.Town.Data;
using UnityEngine;
using VContainer;

namespace CozyBuilder.Town.Rendering
{
    public sealed class TownGridView : MonoBehaviour
    {
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private Transform generatedRoot;
        [SerializeField] private float cellSpacing = 2.1f;
        [SerializeField] private float blockHeightStep = 0.35f;
        [SerializeField] private int maxDirtyCellsPerFrame = 16;
        [SerializeField] private bool rebuildOnStart = true;

        private readonly Dictionary<GridCoord, GameObject> cellsByCoord = new Dictionary<GridCoord, GameObject>();
        private TownDataStore townDataStore;
        private TownVisualRebuilder townVisualRebuilder;
        private bool hasBuiltInitialGrid;

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

        public bool RefreshCell(GridCoord coord)
        {
            if (!townDataStore.Current.TryGetCell(coord, out var cell))
            {
                if (cellsByCoord.TryGetValue(coord, out var staleView))
                {
                    staleView.SetActive(false);
                }

                return false;
            }

            if (!cellsByCoord.TryGetValue(coord, out var cellView))
            {
                CreateCellView(coord, in cell);
                return true;
            }

            ApplyCellState(cellView, coord, in cell);
            return true;
        }

        private void EnsureRoot()
        {
            if (generatedRoot != null)
            {
                return;
            }

            var root = new GameObject("Generated Town Cells");
            root.transform.SetParent(transform, false);
            generatedRoot = root.transform;
        }

        private void ClearGeneratedCells()
        {
            for (var i = generatedRoot.childCount - 1; i >= 0; i--)
            {
                Destroy(generatedRoot.GetChild(i).gameObject);
            }

            cellsByCoord.Clear();
        }

        private void CreateCellView(GridCoord coord, in CellData cell)
        {
            var instance = Instantiate(cellPrefab, generatedRoot);
            instance.transform.localRotation = Quaternion.identity;
            ApplyCellState(instance, coord, in cell);
            cellsByCoord.Add(coord, instance);
        }

        private void ApplyCellState(GameObject instance, GridCoord coord, in CellData cell)
        {
            instance.name = $"Cell {coord.X},{coord.Y} H{cell.Height}";
            instance.transform.localPosition = GridToWorld(coord, cell.Height);
            instance.transform.localScale = Vector3.one;
            instance.SetActive(cell.Terrain != TerrainType.None);
        }

        private Vector3 GridToWorld(GridCoord coord, ushort height)
        {
            return new Vector3(coord.X * cellSpacing, height * blockHeightStep, coord.Y * cellSpacing);
        }

        private Vector3 GridToWorld(GridCoord coord)
        {
            return GridToWorld(coord, 0);
        }
    }
}
