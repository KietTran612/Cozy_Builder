# Kế hoạch thiết kế Hệ thống công cụ Debug 3D trực quan (Visual Debug Tooling System)

Tài liệu này mô tả chi tiết giải pháp kỹ thuật xây dựng công cụ Debug 3D trực quan trên Scene trong Game View phục vụ việc kiểm thử luật xây dựng và cấu trúc lưới hữu cơ trên thiết bị cảm ứng di động.

## User Review Required

> [!IMPORTANT]
> **Giải pháp vẽ lưới Grid Line Mesh (1 Draw Call):**
> Để vẽ đường lưới bao quanh hàng trăm ô lục giác/vuông trên đảo mà không làm giảm hiệu năng di động, chúng ta tự động sinh một **Mesh Line duy nhất** (`Grid Line Mesh`) chứa tất cả các đoạn thẳng biên giới lúc bắt đầu game và hiển thị qua đúng **1 MeshRenderer**. Việc bật/tắt lưới chỉ tốn **0 GC Allocations** bằng cách gọi `gameObject.SetActive(true/false)`.
> 
> **Focus-based UI 3D lơ lửng:**
> Để hiển thị chỉ số Neighbor 3D và Rule áp dụng, chúng ta xây dựng một cụm UI 3D gọn nhẹ lơ lửng bám theo ô đang được chọn/hover (Focus-based), thay vì hiển thị tràn lan trên tất cả các ô gây nhiễu loạn thị giác.

## Proposed Changes

### CozyBuilder.Runtime

#### [NEW] [PrototypeTownDebug3D.cs](file:///c:/1.SOURCE/Unity/Source/Cozy_Builder/Cozy_Builder/Assets/CozyBuilder/Runtime/Town/Debugging/PrototypeTownDebug3D.cs)
Tạo component Debug 3D mới phụ trách:
- Vẽ lưới hòn đảo Mesh Line dựa trên thông tin tọa độ từ `TownGridView`.
- Quản lý cụm chữ 3D (TextMesh) lơ lửng bám theo ô đang chọn để hiển thị thông tin hàng xóm và luật visual.
- Quản lý Pool các Marker Cube đỏ mờ để highlight các ô đang nằm trong dirty queue.
- Cung cấp các nút Toggle bật/tắt (Toggle Grid, Toggle Neighbor, Toggle Dirty Highlight) thông qua kết nối với `PrototypeTownDebugView` và các driver.

```csharp
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

            // Tạo line material đơn giản sử dụng URP Unlit
            var unlitShader = Shader.Find("Universal Render Pipeline/Unlit");
            lineMaterial = new Material(unlitShader != null ? unlitShader : Shader.Find("Unlit/Color"));
            lineMaterial.color = gridLineColor;
            
            // Hỗ trợ vẽ mờ trong suốt (Alpha blending)
            lineMaterial.SetFloat("_Surface", 1); // 1 = Transparent in URP
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            lineMaterial.SetInt("_ZWrite", 0);
            lineMaterial.DisableKeyword("_ALPHATEST_ON");
            lineMaterial.EnableKeyword("_ALPHABLEND_ON");
            lineMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            lineMaterial.renderQueue = 3000; // Queue Transparent

            gridLineRenderer.sharedMaterial = lineMaterial;
            BuildGridLineMesh();
        }

        private void BuildGridLineMesh()
        {
            if (townGridView == null || townDataStore == null) return;

            var townData = townDataStore.Current;
            int cellCount = townData.CellCount;
            float spacing = 2.1f; // Giá trị cellSpacing mặc định
            
            // Tìm cellSpacing thực tế của TownGridView thông qua reflection hoặc hardcode
            // (Hiện tại là 2.1f bám theo TownGridView.cs)
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
            var markerShader = Shader.Find("Universal Render Pipeline/Unlit");
            var markerMat = new Material(markerShader != null ? markerShader : Shader.Find("Unlit/Color"));
            markerMat.color = dirtyHighlightColor;
            
            // Cấu hình mờ bán trong suốt
            markerMat.SetFloat("_Surface", 1);
            markerMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            markerMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            markerMat.SetInt("_ZWrite", 0);
            markerMat.renderQueue = 3000;

            for (int i = 0; i < 10; i++)
            {
                var marker = GameObject.CreatePrimitive(PrimitiveType.Cube);
                marker.name = $"DirtyMarker_{i}";
                marker.transform.SetParent(transform, false);
                marker.transform.localScale = new Vector3(2.0f, 0.1f, 2.0f); // Tấm phẳng bao quanh ô
                
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
            if (!showFocusDebug || debugState == null || !debugState.HasSelectedCoord || townDataStore == null)
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
            float spacing = 2.1f;
            float heightStep = 0.35f;

            // Đặt text lơ lửng phía trên block cao nhất của ô đó
            Vector3 targetPos = new Vector3(coord.X * spacing, cell.Height * heightStep + debugHeightOffset, coord.Y * spacing);
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

            if (!showDirtyHighlight || townVisualRebuilder == null) return;

            int dirtyCount = townVisualRebuilder.DirtyCount;
            if (dirtyCount == 0) return;

            int previewLimit = Mathf.Min(dirtyCount, dirtyMarkersPool.Count);
            int copied = townVisualRebuilder.CopyDirtyCoords(dirtyBuffer, previewLimit);
            float spacing = 2.1f;

            for (int i = 0; i < copied; i++)
            {
                var coord = dirtyBuffer[i];
                var marker = dirtyMarkersPool[i];
                
                // Lấy chiều cao thực tế của ô để highlight tấm phẳng ngay trên mặt móng/block
                float height = 0.05f;
                if (townDataStore.Current.TryGetCell(coord, out var cell))
                {
                    height = cell.Height * 0.35f + 0.05f;
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
```

#### [MODIFY] [PrototypeTownDebugView.cs](file:///c:/1.SOURCE/Unity/Source/Cozy_Builder/Cozy_Builder/Assets/CozyBuilder/Runtime/Town/Debugging/PrototypeTownDebugView.cs)
Bổ sung các nút bấm Toggle 3D Debug trực tiếp trên bảng IMGUI Debug 2D phẳng hiện có để người dùng có thể click tương tác bật/tắt Grid Lines, Neighbor 3D và Dirty Highlight 3D một cách sinh động.

#### [MODIFY] [GameLifetimeScope.cs](file:///c:/1.SOURCE/Unity/Source/Cozy_Builder/Cozy_Builder/Assets/CozyBuilder/Runtime/Bootstrap/GameLifetimeScope.cs)
Đăng ký `PrototypeTownDebug3D` từ Scene Hierarchy vào VContainer lifetime scope để tự động giải quyết dependency injection.

---

## Verification Plan

### Automated Tests
- Biên dịch dự án C# không có lỗi.
- Chạy lệnh `graphify update .` để làm sạch và cập nhật đồ thị AST.

### Manual Verification
- Mở Play Mode, kiểm tra xem Grid Lines màu vàng ấm đã xuất hiện bao quanh đảo một cách mượt mà hay chưa.
- Chọn một ô bất kỳ: kiểm tra xem chữ Debug 3D có xuất hiện lơ lửng bám theo ô đó và cập nhật thông tin theo thời gian thực hay không.
- Bấm các nút Toggle trên Debug Panel để kiểm tra bật/tắt mượt màng.
- Khi đặt/xóa block nhanh: kiểm tra xem các tấm highlight đỏ mờ có chớp tắt tại các ô bẩn đang chờ rebuild hay không.
- Đo Profiler để xác định GC Alloc = 0 khi bật tắt debug.
