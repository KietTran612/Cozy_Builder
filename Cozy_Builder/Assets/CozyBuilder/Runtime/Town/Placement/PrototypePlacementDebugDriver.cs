using CozyBuilder.Town.Data;
using CozyBuilder.Town.Debugging;
using UnityEngine;
using VContainer;

namespace CozyBuilder.Town.Placement
{
    public sealed class PrototypePlacementDebugDriver : MonoBehaviour
    {
        [SerializeField] private int debugX;
        [SerializeField] private int debugY;
        [SerializeField] private ushort colorId;
        [SerializeField] private ushort materialId;

        private PlacementService placementService;
        private PrototypeTownDebugState debugState;

        [Inject]
        public void Construct(PlacementService placementService, PrototypeTownDebugState debugState)
        {
            this.placementService = placementService;
            this.debugState = debugState;
        }

        public bool PlaceDebugBlock()
        {
            if (placementService == null)
            {
                Debug.LogWarning("PrototypePlacementDebugDriver requires PlacementService injection.", this);
                return false;
            }

            debugState.Select(DebugCoord);
            return placementService.TryPlaceBlock(DebugCoord, colorId, materialId);
        }

        public bool DeleteDebugBlock()
        {
            if (placementService == null)
            {
                Debug.LogWarning("PrototypePlacementDebugDriver requires PlacementService injection.", this);
                return false;
            }

            debugState.Select(DebugCoord);
            return placementService.TryDeleteBlock(DebugCoord);
        }

        private GridCoord DebugCoord => new GridCoord(debugX, debugY);
    }
}
