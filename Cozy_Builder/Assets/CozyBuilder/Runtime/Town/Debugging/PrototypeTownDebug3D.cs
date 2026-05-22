using System.Collections.Generic;
using CozyBuilder.Town.Data;
using CozyBuilder.Town.Placement;
using CozyBuilder.Town.Rendering;
using UnityEngine;
using VContainer;

namespace CozyBuilder.Town.Debugging
{
    public sealed class PrototypeTownDebug3D : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Color gridLineColor = new Color(1f, 0.73f, 0.2f, 0.45f); // Vàng ấm mờ
        [SerializeField] private Color dirtyHighlightColor = new Color(1f, 0.2f, 0.2f, 0.35f); // Đỏ mờ
        [SerializeField] private float debugHeightOffset = 1.2f;

        private TownGridView townGridView;
        private PrototypeTownDebugState debugState;
        private TownDataStore townDataStore;
        private TownVisualRebuilder townVisualRebuilder;
        private PlacementService placementService;

        private GameObject gridLineObj;
        private MeshFilter gridLineFilter;
        private MeshRenderer gridLineRenderer;
        private Material lineMaterial;

        private GameObject focusDebugObj;
        private TextMesh focusTextMesh;

        private readonly List<GameObject> dirtyMarkersPool = new List<GameObject>();
        private readonly GridCoord[] dirtyBuffer = new GridCoord[16];

        private bool showGrid = true;
        private bool showFocusDebug = true;
        private bool showDirtyHighlight = true;

        [Inject]
        public void Construct(
            TownGridView townGridView,
            PrototypeTownDebugState debugState,
            TownDataStore townDataStore,
            TownVisualRebuilder townVisualRebuilder,
            PlacementService placementService)
        {
            this.townGridView = townGridView;
            this.debugState = debugState;
            this.townDataStore = townDataStore;
            this.townVisualRebuilder = townVisualRebuilder;
            this.placementService = placementService;
        }

        private void Start()
        {
            InitializeGridLineMesh();
            InitializeFocusDebug();
            InitializeDirtyMarkersPool();
        }

        private void LateUpdate()
        {
            UpdateGridLineVisibility();
            UpdateFocusDebug();
            UpdateDirtyHighlights();
        }

        public void ToggleGrid(bool show) => showGrid = show;
        public void ToggleFocusDebug(bool show) => showFocusDebug = show;
        public void ToggleDirtyHighlight(bool show) => showDirtyHighlight = show;

        public bool IsGridActive => showGrid;
        public bool IsFocusDebugActive => showFocusDebug;
        public bool IsDirtyHighlightActive => showDirtyHighlight;

        private void InitializeGridLineMesh()
        {
            gridLineObj = new GameObject("Debug_GridLines");
            gridLineObj.transform.SetParent(transform, false);

            gridLineFilter = gridLineObj.AddComponent<MeshFilter>();
            gridLineRenderer = gridLineObj.AddComponent<MeshRenderer>();

            // Tạo line material đơn giản sử dụng Sprites/Default (URP/Builtin compatible & alpha-ready)
            var spriteShader = Shader.Find("Sprites/Default");
            lineMaterial = new Material(spriteShader != null ? spriteShader : Shader.Find("Unlit/Color"));
            lineMaterial.color = gridLineColor;
            if (lineMaterial.HasProperty("_BaseColor"))
            {
                lineMaterial.SetColor("_BaseColor", gridLineColor);
            }
            
            gridLineRenderer.sharedMaterial = lineMaterial;
            BuildGridLineMesh();
        }

        private void BuildGridLineMesh()
        {
            if (townGridView == null || townDataStore == null) return;

            var townData = townDataStore.Current;
            int cellCount = townData.CellCount;
            float spacing = townGridView.CellSpacing;
            
            float halfSize = spacing * 0.5f;
            float heightOffset = 0.02f; // Tránh Z-fighting với mặt đất

            List<Vector3> vertices = new List<Vector3>(cellCount * 4);
            List<int> indices = new List<int>(cellCount * 8);

            for (int i = 0; i < cellCount; i++)
            {
                var coord = townData.Coordinates[i];
                // Tính center giống GridToWorld
                Vector3 center = new Vector3(coord.X * spacing, 0f, coord.Y * spacing);
                int baseIdx = vertices.Count;

                vertices.Add(new Vector3(center.x - halfSize, heightOffset, center.z - halfSize));
                vertices.Add(new Vector3(center.x + halfSize, heightOffset, center.z - halfSize));
                vertices.Add(new Vector3(center.x + halfSize, heightOffset, center.z + halfSize));
                vertices.Add(new Vector3(center.x - halfSize, heightOffset, center.z + halfSize));

                indices.Add(baseIdx + 0); indices.Add(baseIdx + 1);
                indices.Add(baseIdx + 1); indices.Add(baseIdx + 2);
                indices.Add(baseIdx + 2); indices.Add(baseIdx + 3);
                indices.Add(baseIdx + 3); indices.Add(baseIdx + 0);
            }

            Mesh mesh = new Mesh();
            mesh.name = "Island Grid Lines";
            mesh.SetVertices(vertices);
            mesh.SetIndices(indices, MeshTopology.Lines, 0);
            mesh.UploadMeshData(false);

            gridLineFilter.sharedMesh = mesh;
        }

        private void InitializeFocusDebug()
        {
            focusDebugObj = new GameObject("Debug_FocusPanel");
            focusDebugObj.transform.SetParent(transform, false);

            focusTextMesh = focusDebugObj.AddComponent<TextMesh>();
            focusTextMesh.anchor = TextAnchor.LowerCenter;
            focusTextMesh.alignment = TextAlignment.Center;
            focusTextMesh.fontSize = 24;
            focusTextMesh.characterSize = 0.1f;
            focusTextMesh.color = Color.white;

            // Xoay nhẹ Text hướng về camera nghiêng 30 độ
            focusDebugObj.transform.localRotation = Quaternion.Euler(30f, 0f, 0f);
            focusDebugObj.SetActive(false);
        }

        private void InitializeDirtyMarkersPool()
        {
            // Khởi tạo trước 10 Box Marker đỏ mờ
            var spriteShader = Shader.Find("Sprites/Default");
            var markerMat = new Material(spriteShader != null ? spriteShader : Shader.Find("Unlit/Color"));
            markerMat.color = dirtyHighlightColor;
            if (markerMat.HasProperty("_BaseColor"))
            {
                markerMat.SetColor("_BaseColor", dirtyHighlightColor);
            }

            for (int i = 0; i < 10; i++)
            {
                var marker = GameObject.CreatePrimitive(PrimitiveType.Cube);
                marker.name = $"DirtyMarker_{i}";
                marker.transform.SetParent(transform, false);
                
                // Sử dụng kích thước tỉ lệ với spacing
                float spacing = townGridView != null ? townGridView.CellSpacing : 2.1f;
                marker.transform.localScale = new Vector3(spacing, 0.1f, spacing); // Tấm phẳng bao quanh ô
                
                // Gỡ bỏ Collider để tránh cản trở raycast đặt block
                if (marker.TryGetComponent<Collider>(out var col))
                {
                    Destroy(col);
                }

                if (marker.TryGetComponent<MeshRenderer>(out var mr))
                {
                    mr.sharedMaterial = markerMat;
                }

                marker.SetActive(false);
                dirtyMarkersPool.Add(marker);
            }
        }

        private void UpdateGridLineVisibility()
        {
            if (gridLineObj != null)
            {
                gridLineObj.SetActive(showGrid);
            }
        }

        private void UpdateFocusDebug()
        {
            if (!showFocusDebug || debugState == null || !debugState.HasSelectedCoord || townDataStore == null || townGridView == null)
            {
                if (focusDebugObj != null) focusDebugObj.SetActive(false);
                return;
            }

            var coord = debugState.SelectedCoord;
            if (!townDataStore.Current.TryGetCell(coord, out var cell))
            {
                if (focusDebugObj != null) focusDebugObj.SetActive(false);
                return;
            }

            var rule = placementService.Preview(coord, in cell);
            float spacing = townGridView.CellSpacing;
            float heightStep = townGridView.BlockHeightStep;

            // Đặt text lơ lửng phía trên block cao nhất của ô đó
            float height = cell.Height == 0 ? 0f : townGridView.FirstBlockHeightOffset + cell.Height * heightStep;
            Vector3 targetPos = new Vector3(coord.X * spacing, height + debugHeightOffset, coord.Y * spacing);
            focusDebugObj.transform.localPosition = targetPos;
            focusDebugObj.SetActive(true);

            focusTextMesh.text = $"ID: {coord.X},{coord.Y}\nHeight: {cell.Height}\nRule: V{rule.VisualId} R{rule.RotationId}";
        }

        private void UpdateDirtyHighlights()
        {
            // Tắt toàn bộ marker cũ
            for (int i = 0; i < dirtyMarkersPool.Count; i++)
            {
                dirtyMarkersPool[i].SetActive(false);
            }

            if (!showDirtyHighlight || townVisualRebuilder == null || townGridView == null) return;

            int dirtyCount = townVisualRebuilder.DirtyCount;
            if (dirtyCount == 0) return;

            int previewLimit = Mathf.Min(dirtyCount, dirtyMarkersPool.Count);
            int copied = townVisualRebuilder.CopyDirtyCoords(dirtyBuffer, previewLimit);
            float spacing = townGridView.CellSpacing;
            float heightStep = townGridView.BlockHeightStep;

            for (int i = 0; i < copied; i++)
            {
                var coord = dirtyBuffer[i];
                var marker = dirtyMarkersPool[i];
                
                // Lấy chiều cao thực tế của ô để highlight tấm phẳng ngay trên mặt móng/block
                float height = 0.05f;
                if (townDataStore.Current.TryGetCell(coord, out var cell))
                {
                    height = cell.Height == 0 ? 0.05f : townGridView.FirstBlockHeightOffset + cell.Height * heightStep + 0.05f;
                }

                marker.transform.localPosition = new Vector3(coord.X * spacing, height, coord.Y * spacing);
                marker.SetActive(true);
            }
        }

        private void OnDestroy()
        {
            // Giải phóng Material tránh rò rỉ bộ nhớ
            if (lineMaterial != null) Destroy(lineMaterial);
            if (dirtyMarkersPool.Count > 0 && dirtyMarkersPool[0] != null)
            {
                var renderer = dirtyMarkersPool[0].GetComponent<MeshRenderer>();
                if (renderer != null && renderer.sharedMaterial != null)
                {
                    Destroy(renderer.sharedMaterial);
                }
            }
        }
    }
}
