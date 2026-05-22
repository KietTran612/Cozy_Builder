using System.Text;
using CozyBuilder.Town.Data;
using CozyBuilder.Town.Placement;
using CozyBuilder.Town.Rendering;
using UnityEngine;
using VContainer;
using CozyBuilder.Camera;

namespace CozyBuilder.Town.Debugging
{
    public sealed class PrototypeTownDebugView : MonoBehaviour, ICameraInputBlocker
    {
        [SerializeField] private Rect panelRect = new Rect(16f, 208f, 320f, 350f);
        [SerializeField] private int maxDirtyPreview = 8;

        public Rect PanelRect => panelRect;

        private readonly GridCoord[] dirtyPreviewBuffer = new GridCoord[16];
        private readonly StringBuilder labelBuilder = new StringBuilder(512);
        private PrototypeTownDebugState debugState;
        private TownDataStore townDataStore;
        private TownVisualRebuilder townVisualRebuilder;
        private PlacementService placementService;
        private PrototypeTownDebug3D debug3D;

        [Inject]
        public void Construct(
            PrototypeTownDebugState debugState,
            TownDataStore townDataStore,
            TownVisualRebuilder townVisualRebuilder,
            PlacementService placementService,
            PrototypeTownDebug3D debug3D)
        {
            this.debugState = debugState;
            this.townDataStore = townDataStore;
            this.townVisualRebuilder = townVisualRebuilder;
            this.placementService = placementService;
            this.debug3D = debug3D;
        }

        public bool IsPointerOverUI(Vector2 screenPosition)
        {
            if (!enabled || !gameObject.activeInHierarchy)
            {
                return false;
            }

            Vector2 guiPos = new Vector2(screenPosition.x, Screen.height - screenPosition.y);
            return panelRect.Contains(guiPos);
        }

        private void OnGUI()
        {
            if (debugState == null || townDataStore == null || townVisualRebuilder == null || placementService == null)
            {
                return;
            }

            GUILayout.BeginArea(panelRect, GUI.skin.box);
            GUILayout.Label("Town Debug");
            DrawSelectedCell();
            DrawDirtyQueue();

            if (debug3D != null)
            {
                GUILayout.Space(5);
                GUILayout.Label("3D Debug Tools");
                GUILayout.BeginHorizontal();
                
                bool gridActive = GUILayout.Toggle(debug3D.IsGridActive, "Grid Lines");
                if (gridActive != debug3D.IsGridActive)
                {
                    debug3D.ToggleGrid(gridActive);
                }

                bool focusActive = GUILayout.Toggle(debug3D.IsFocusDebugActive, "Focus Info");
                if (focusActive != debug3D.IsFocusDebugActive)
                {
                    debug3D.ToggleFocusDebug(focusActive);
                }

                bool dirtyActive = GUILayout.Toggle(debug3D.IsDirtyHighlightActive, "Dirty Box");
                if (dirtyActive != debug3D.IsDirtyHighlightActive)
                {
                    debug3D.ToggleDirtyHighlight(dirtyActive);
                }

                GUILayout.EndHorizontal();
            }

            GUILayout.EndArea();
        }

        private void DrawSelectedCell()
        {
            if (!debugState.HasSelectedCoord)
            {
                GUILayout.Label("Selected: none");
                return;
            }

            var coord = debugState.SelectedCoord;
            if (!townDataStore.Current.TryGetCell(coord, out var cell))
            {
                GUILayout.Label($"Selected: {coord.X},{coord.Y} missing");
                return;
            }

            var rule = placementService.Preview(coord, in cell);
            labelBuilder.Length = 0;
            labelBuilder.Append("Selected: ");
            AppendCoord(coord);
            labelBuilder.AppendLine();
            labelBuilder.Append("Height: ");
            labelBuilder.Append(cell.Height);
            labelBuilder.Append("  Terrain: ");
            labelBuilder.Append(cell.Terrain);
            labelBuilder.AppendLine();
            labelBuilder.Append("ColorId: ");
            labelBuilder.Append(cell.ColorId);
            labelBuilder.Append("  MaterialId: ");
            labelBuilder.Append(cell.MaterialId);
            labelBuilder.AppendLine();
            labelBuilder.Append("Flags: ");
            labelBuilder.Append(cell.Flags);
            labelBuilder.AppendLine();
            labelBuilder.Append("Rule: Visual ");
            labelBuilder.Append(rule.VisualId);
            labelBuilder.Append("  Variant ");
            labelBuilder.Append(rule.VariantId);
            GUILayout.Label(labelBuilder.ToString());

            GUILayout.Label("Neighbors");
            for (var i = 0; i < GridNeighborhood.CardinalOffsets.Length; i++)
            {
                var offset = GridNeighborhood.CardinalOffsets[i];
                var neighbor = new GridCoord(coord.X + offset.X, coord.Y + offset.Y);
                DrawNeighbor(neighbor);
            }
        }

        private void DrawNeighbor(GridCoord coord)
        {
            labelBuilder.Length = 0;
            AppendCoord(coord);

            if (!townDataStore.Current.TryGetCell(coord, out var cell))
            {
                labelBuilder.Append(": missing");
                GUILayout.Label(labelBuilder.ToString());
                return;
            }

            labelBuilder.Append(": H");
            labelBuilder.Append(cell.Height);
            labelBuilder.Append(" ");
            labelBuilder.Append(cell.Terrain);
            labelBuilder.Append(" ");
            labelBuilder.Append(cell.Flags);
            GUILayout.Label(labelBuilder.ToString());
        }

        private void DrawDirtyQueue()
        {
            var dirtyCount = townVisualRebuilder.DirtyCount;
            labelBuilder.Length = 0;
            labelBuilder.Append("Dirty queue: ");
            labelBuilder.Append(dirtyCount);
            GUILayout.Label(labelBuilder.ToString());

            var previewLimit = Mathf.Clamp(maxDirtyPreview, 0, dirtyPreviewBuffer.Length);
            var copied = townVisualRebuilder.CopyDirtyCoords(dirtyPreviewBuffer, previewLimit);
            if (copied == 0)
            {
                GUILayout.Label("Dirty preview: empty");
                return;
            }

            labelBuilder.Length = 0;
            labelBuilder.Append("Dirty preview: ");
            for (var i = 0; i < copied; i++)
            {
                if (i > 0)
                {
                    labelBuilder.Append(" ");
                }

                AppendCoord(dirtyPreviewBuffer[i]);
            }

            GUILayout.Label(labelBuilder.ToString());
        }

        private void AppendCoord(GridCoord coord)
        {
            labelBuilder.Append(coord.X);
            labelBuilder.Append(",");
            labelBuilder.Append(coord.Y);
        }
    }
}
