using UnityEngine;
using VContainer;

namespace CozyBuilder.Town.Placement
{
    public sealed class PrototypePlacementControlsView : MonoBehaviour
    {
        [SerializeField] private Rect panelRect = new Rect(16f, 16f, 240f, 180f);
        [SerializeField] private int colorCount = 4;
        [SerializeField] private int materialCount = 4;

        private PrototypePlacementState placementState;

        [Inject]
        public void Construct(PrototypePlacementState placementState)
        {
            this.placementState = placementState;
        }

        private void OnGUI()
        {
            if (placementState == null)
            {
                return;
            }

            GUILayout.BeginArea(panelRect, GUI.skin.box);
            GUILayout.Label($"Mode: {placementState.Mode}");
            GUILayout.Label($"ColorId: {placementState.CurrentColorId}  MaterialId: {placementState.CurrentMaterialId}");

            GUILayout.BeginHorizontal();
            DrawModeButton("Place", PrototypePlacementMode.Place);
            DrawModeButton("Delete", PrototypePlacementMode.Delete);
            GUILayout.EndHorizontal();

            GUILayout.Label("Color");
            DrawIdButtons(colorCount, placementState.CurrentColorId, placementState.SetColorId);

            GUILayout.Label("Material");
            DrawIdButtons(materialCount, placementState.CurrentMaterialId, placementState.SetMaterialId);
            GUILayout.EndArea();
        }

        private void DrawModeButton(string label, PrototypePlacementMode mode)
        {
            var wasEnabled = GUI.enabled;
            GUI.enabled = placementState.Mode != mode;
            if (GUILayout.Button(label))
            {
                placementState.SetMode(mode);
            }

            GUI.enabled = wasEnabled;
        }

        private static void DrawIdButtons(int count, ushort currentId, System.Action<ushort> setId)
        {
            GUILayout.BeginHorizontal();
            var limit = Mathf.Max(0, count);
            for (var i = 0; i < limit; i++)
            {
                var wasEnabled = GUI.enabled;
                GUI.enabled = currentId != i;
                if (GUILayout.Button(i.ToString()))
                {
                    setId((ushort)i);
                }

                GUI.enabled = wasEnabled;
            }

            GUILayout.EndHorizontal();
        }
    }
}
