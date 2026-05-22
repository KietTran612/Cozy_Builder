using System.Text;
using CozyBuilder.Town.Data;
using CozyBuilder.Town.Placement;
using CozyBuilder.Town.Rendering;
using UnityEngine;
using VContainer;

namespace CozyBuilder.Town.Debugging
{
    public sealed class PrototypeTownDebugView : MonoBehaviour
    {
        [SerializeField] private Rect panelRect = new Rect(16f, 208f, 320f, 300f);
        [SerializeField] private int maxDirtyPreview = 8;

        private readonly GridCoord[] dirtyPreviewBuffer = new GridCoord[16];
        private readonly StringBuilder labelBuilder = new StringBuilder(512);
        private PrototypeTownDebugState debugState;
        private TownDataStore townDataStore;
        private TownVisualRebuilder townVisualRebuilder;
        private PlacementService placementService;

        [Inject]
        public void Construct(
            PrototypeTownDebugState debugState,
            TownDataStore townDataStore,
            TownVisualRebuilder townVisualRebuilder,
            PlacementService placementService)
        {
            this.debugState = debugState;
            this.townDataStore = townDataStore;
            this.townVisualRebuilder = townVisualRebuilder;
            this.placementService = placementService;
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

            var rule = placementService.Preview(in cell);
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
