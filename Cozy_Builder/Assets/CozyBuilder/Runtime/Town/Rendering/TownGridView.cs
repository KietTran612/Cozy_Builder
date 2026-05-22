using System.Collections.Generic;
using CozyBuilder.Town.Data;
using CozyBuilder.Town.Rules;
using UnityEngine;
using VContainer;

namespace CozyBuilder.Town.Rendering
{
    [System.Serializable]
    public struct PrefabOffsetConfig
    {
        public GameObject prefab;
        public Vector3 positionOffset;
        public Vector3 rotationOffset;
        public Vector3 scaleMultiplier;
    }

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
        [SerializeField] private float firstBlockHeightOffset = 0.35f;
        [SerializeField] private float blockHeightStep = 2.0f;
        [SerializeField] private Vector3 blockScale = Vector3.one;
        [SerializeField] private int maxDirtyCellsPerFrame = 16;
        [SerializeField] private bool rebuildOnStart = true;

        [Header("Prefab Offset & Tuning")]
        [SerializeField] private List<PrefabOffsetConfig> prefabOffsets = new List<PrefabOffsetConfig>();

        [Header("Color & Material Config")]
        [SerializeField] private Color[] cozyPalette = new Color[]
        {
            new Color(0.878f, 0.353f, 0.278f), // 0: Đỏ gạch ấm (#e05a47)
            new Color(0.902f, 0.663f, 0.333f), // 1: Vàng mù tạt (#e6a955)
            new Color(0.220f, 0.478f, 0.482f), // 2: Xanh cổ vịt/teal (#387a7b)
            new Color(0.455f, 0.639f, 0.702f), // 3: Xanh trời nhạt (#74a3b3)
            new Color(0.900f, 0.851f, 0.761f), // 4: Kem đá/stone (#e5d9c2)
            new Color(0.439f, 0.545f, 0.459f)  // 5: Xanh lá xám (#708b75)
        };

        private MaterialPropertyBlock propertyBlock;
        private static readonly List<Renderer> tempRendererList = new List<Renderer>(8);
        private static readonly int baseColorPropId = Shader.PropertyToID("_BaseColor");
        private static readonly int smoothnessPropId = Shader.PropertyToID("_Smoothness");
        private static readonly int metallicPropId = Shader.PropertyToID("_Metallic");

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

        public float CellSpacing => cellSpacing;
        public float FirstBlockHeightOffset => firstBlockHeightOffset;
        public float BlockHeightStep => blockHeightStep;

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
            EnsureCollider(terrainView);
            terrainView.transform.localRotation = Quaternion.identity;

            var state = new CellVisualState
            {
                TerrainView = terrainView
            };

            cellsByCoord.Add(coord, terrainView);
            visualStatesByCoord.Add(coord, state);
            ApplyCellState(state, coord, in cell);
        }

        private void EnsureCollider(GameObject obj)
        {
            if (obj == null) return;

            // Nếu đối tượng hoặc các đối tượng con của nó đã có bất kỳ Collider nào, bỏ qua
            if (obj.GetComponentInChildren<Collider>(true) != null)
            {
                return;
            }

            // Quét tất cả MeshFilter để bổ sung MeshCollider
            var meshFilters = obj.GetComponentsInChildren<MeshFilter>(true);
            bool added = false;
            foreach (var mf in meshFilters)
            {
                if (mf.gameObject.GetComponent<Collider>() == null)
                {
                    mf.gameObject.AddComponent<MeshCollider>();
                    added = true;
                }
            }

            if (added)
            {
                UnityEngine.Debug.Log($"[TownGridView] Automatically added MeshCollider to FBX model: {obj.name}", obj);
            }
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

                GameObject blockView = null;

                if (i < state.BlockViews.Count)
                {
                    // Existing block layer - check if visual type matches
                    var existing = state.BlockViews[i];
                    if (existing.VisualId == targetVisualId)
                    {
                        // Same visual type, just update transform & name
                        blockView = existing.GameObject;
                        blockView.name = $"Block {coord.X},{coord.Y} L{layer} V{targetVisualId}";
                        blockView.transform.localPosition = GridToWorld(coord, (ushort)layer);
                        blockView.transform.localRotation = Quaternion.Euler(0f, rule.RotationId * 90f, 0f);
                        blockView.transform.localScale = blockScale;
                    }
                    else
                    {
                        // Mismatching visual type - recycle and replace
                        ReturnToPool(existing.VisualId, existing.GameObject);
                        blockView = GetPooledBlock(targetVisualId);
                        blockView.name = $"Block {coord.X},{coord.Y} L{layer} V{targetVisualId}";
                        blockView.transform.localPosition = GridToWorld(coord, (ushort)layer);
                        blockView.transform.localRotation = Quaternion.Euler(0f, rule.RotationId * 90f, 0f);
                        blockView.transform.localScale = blockScale;

                        state.BlockViews[i] = new BlockViewData
                        {
                            GameObject = blockView,
                            VisualId = targetVisualId
                        };
                    }
                }
                else
                {
                    // New layer - get pooled block and add to list
                    blockView = GetPooledBlock(targetVisualId);
                    blockView.name = $"Block {coord.X},{coord.Y} L{layer} V{targetVisualId}";
                    blockView.transform.localPosition = GridToWorld(coord, (ushort)layer);
                    blockView.transform.localRotation = Quaternion.Euler(0f, rule.RotationId * 90f, 0f);
                    blockView.transform.localScale = blockScale;

                    state.BlockViews.Add(new BlockViewData
                    {
                        GameObject = blockView,
                        VisualId = targetVisualId
                    });
                }

                // Áp dụng màu sắc và chất liệu động (0 GC Allocations)
                ApplyColorAndMaterial(blockView, cell.ColorId, cell.MaterialId);
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

        private void ApplyColorAndMaterial(GameObject blockView, ushort colorId, ushort materialId)
        {
            if (blockView == null) return;

            if (propertyBlock == null)
            {
                propertyBlock = new MaterialPropertyBlock();
            }

            // 1. Xác định màu sắc
            Color targetColor = Color.white;
            if (cozyPalette != null && cozyPalette.Length > 0)
            {
                int index = colorId % cozyPalette.Length;
                targetColor = cozyPalette[index];
            }

            // 2. Xác định cấu hình chất liệu (Smoothness & Metallic)
            float smoothness = 0.1f;
            float metallic = 0.0f;
            switch (materialId)
            {
                case 1: // Gỗ / Bán bóng
                    smoothness = 0.35f;
                    metallic = 0.0f;
                    break;
                case 2: // Kim loại / Nhẵn bóng
                    smoothness = 0.75f;
                    metallic = 0.8f;
                    break;
                case 3: // Gốm bóng
                    smoothness = 0.85f;
                    metallic = 0.1f;
                    break;
                default: // Đá / Nhám (materialId = 0 hoặc khác)
                    smoothness = 0.1f;
                    metallic = 0.0f;
                    break;
            }

            propertyBlock.Clear();
            propertyBlock.SetColor(baseColorPropId, targetColor);
            propertyBlock.SetFloat(smoothnessPropId, smoothness);
            propertyBlock.SetFloat(metallicPropId, metallic);

            // 3. Tìm renderer qua Adapter hoặc Fallback
            BlockColorAdapter adapter = null;
            if (blockView.TryGetComponent<BlockColorAdapter>(out var rootAdapter))
            {
                adapter = rootAdapter;
            }
            else
            {
                adapter = blockView.GetComponentInChildren<BlockColorAdapter>(true);
            }

            if (adapter != null)
            {
                var renderers = adapter.Renderers;
                if (renderers != null)
                {
                    for (int i = 0; i < renderers.Length; i++)
                    {
                        if (renderers[i] != null)
                        {
                            renderers[i].SetPropertyBlock(propertyBlock);
                        }
                    }
                }
            }
            else
            {
                // Fallback nếu không có Adapter (Zero GC bằng cách truyền list pre-allocated)
                tempRendererList.Clear();
                blockView.GetComponentsInChildren<Renderer>(true, tempRendererList);
                for (int i = 0; i < tempRendererList.Count; i++)
                {
                    if (tempRendererList[i] != null)
                    {
                        tempRendererList[i].SetPropertyBlock(propertyBlock);
                    }
                }
                tempRendererList.Clear();
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
            
            // 1. Tạo GameObject wrapper cha trống
            GameObject wrapper = new GameObject($"{prefab.name}_Wrapper");
            wrapper.transform.SetParent(blockRoot, false);
            
            // 2. Instantiate FBX làm con của wrapper
            GameObject newObj = Instantiate(prefab, wrapper.transform);
            newObj.name = prefab.name;
            
            // 3. Tra cứu offset config và áp dụng cục bộ lên FBX con
            PrefabOffsetConfig config = GetOffsetConfigForPrefab(prefab);
            newObj.transform.localPosition = config.positionOffset;
            newObj.transform.localRotation = Quaternion.Euler(config.rotationOffset);
            newObj.transform.localScale = config.scaleMultiplier;
            
            // 4. Đảm bảo collider được thêm trực tiếp vào FBX con
            EnsureCollider(newObj);
            
            wrapper.SetActive(true);
            return wrapper;
        }

        private PrefabOffsetConfig GetOffsetConfigForPrefab(GameObject prefab)
        {
            if (prefabOffsets != null)
            {
                for (int i = 0; i < prefabOffsets.Count; i++)
                {
                    if (prefabOffsets[i].prefab == prefab)
                    {
                        return prefabOffsets[i];
                    }
                }
            }

            return new PrefabOffsetConfig
            {
                prefab = prefab,
                positionOffset = Vector3.zero,
                rotationOffset = Vector3.zero,
                scaleMultiplier = Vector3.one
            };
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
            float y = height == 0 ? 0f : firstBlockHeightOffset + (height - 1) * blockHeightStep;
            return new Vector3(coord.X * cellSpacing, y, coord.Y * cellSpacing);
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

#if UNITY_EDITOR
        private void OnValidate()
        {
            AutoWirePrefabs();
        }

        [ContextMenu("Auto Wire Prefabs")]
        public void AutoWirePrefabs()
        {
            string packPath = "Assets/Packages/kaykit_medieval_builder_pack_1.0";
            string modelsPath = $"{packPath}/Models/objects/fbx";
            if (!System.IO.Directory.Exists(packPath)) return;

            bool changed = false;

            if (smallHousePrefab == null)
            {
                smallHousePrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>($"{modelsPath}/house.fbx");
                if (smallHousePrefab != null) changed = true;
            }
            if (houseRoofPrefab == null)
            {
                houseRoofPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>($"{modelsPath}/house.fbx");
                if (houseRoofPrefab != null) changed = true;
            }
            if (towerTopPrefab == null)
            {
                towerTopPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>($"{modelsPath}/watchtower.fbx");
                if (towerTopPrefab != null) changed = true;
            }
            if (stiltsPrefab == null)
            {
                stiltsPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>($"{modelsPath}/wall_straight.fbx");
                if (stiltsPrefab != null) changed = true;
            }
            if (houseWallPrefab == null)
            {
                houseWallPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>($"{modelsPath}/wall_straight.fbx");
                if (houseWallPrefab != null) changed = true;
            }
            if (towerWallPrefab == null)
            {
                towerWallPrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>($"{modelsPath}/wall_straight.fbx");
                if (towerWallPrefab != null) changed = true;
            }

            if (changed)
            {
                UnityEditor.EditorUtility.SetDirty(this);
                UnityEngine.Debug.Log("[TownGridView] Auto-wired prefabs successfully and marked dirty for save.");
            }

            // Tự động cấu hình danh sách offsets mặc định để người dùng có sẵn cài đặt tối ưu
            if (prefabOffsets == null || prefabOffsets.Count == 0)
            {
                prefabOffsets = new List<PrefabOffsetConfig>();

                var houseModel = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>($"{modelsPath}/house.fbx");
                if (houseModel != null)
                {
                    prefabOffsets.Add(new PrefabOffsetConfig
                    {
                        prefab = houseModel,
                        positionOffset = Vector3.zero,
                        rotationOffset = Vector3.zero,
                        scaleMultiplier = new Vector3(0.85f, 0.85f, 0.85f)
                    });
                }

                var watchtowerModel = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>($"{modelsPath}/watchtower.fbx");
                if (watchtowerModel != null)
                {
                    prefabOffsets.Add(new PrefabOffsetConfig
                    {
                        prefab = watchtowerModel,
                        positionOffset = Vector3.zero,
                        rotationOffset = Vector3.zero,
                        scaleMultiplier = new Vector3(0.9f, 0.9f, 0.9f)
                    });
                }

                var wallModel = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>($"{modelsPath}/wall_straight.fbx");
                if (wallModel != null)
                {
                    prefabOffsets.Add(new PrefabOffsetConfig
                    {
                        prefab = wallModel,
                        positionOffset = Vector3.zero,
                        rotationOffset = Vector3.zero,
                        scaleMultiplier = Vector3.one
                    });
                }

                UnityEditor.EditorUtility.SetDirty(this);
                UnityEngine.Debug.Log("[TownGridView] Auto-configured default prefab offsets successfully.");
            }
        }
#endif
    }
}
