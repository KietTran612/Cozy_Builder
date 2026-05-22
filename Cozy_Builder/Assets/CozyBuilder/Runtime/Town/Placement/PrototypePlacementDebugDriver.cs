using CozyBuilder.Town.Data;
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

        [Inject]
        public void Construct(PlacementService placementService)
        {
            this.placementService = placementService;
        }

        public bool PlaceDebugBlock()
        {
            if (placementService == null)
            {
                Debug.LogWarning("PrototypePlacementDebugDriver requires PlacementService injection.", this);
                return false;
            }

            return placementService.TryPlaceBlock(DebugCoord, colorId, materialId);
        }

        public bool DeleteDebugBlock()
        {
            if (placementService == null)
            {
                Debug.LogWarning("PrototypePlacementDebugDriver requires PlacementService injection.", this);
                return false;
            }

            return placementService.TryDeleteBlock(DebugCoord);
        }

        private GridCoord DebugCoord => new GridCoord(debugX, debugY);
    }
}
