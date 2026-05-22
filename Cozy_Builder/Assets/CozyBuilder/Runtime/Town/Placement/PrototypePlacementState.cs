namespace CozyBuilder.Town.Placement
{
    public sealed class PrototypePlacementState
    {
        public PrototypePlacementMode Mode { get; private set; } = PrototypePlacementMode.Place;
        public ushort CurrentColorId { get; private set; }
        public ushort CurrentMaterialId { get; private set; }
        public bool IsDeleteMode => Mode == PrototypePlacementMode.Delete;

        public void SetMode(PrototypePlacementMode mode)
        {
            Mode = mode;
        }

        public void SetColorId(ushort colorId)
        {
            CurrentColorId = colorId;
        }

        public void SetMaterialId(ushort materialId)
        {
            CurrentMaterialId = materialId;
        }
    }
}
