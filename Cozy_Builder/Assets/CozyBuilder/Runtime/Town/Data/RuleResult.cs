namespace CozyBuilder.Town.Data
{
    public readonly struct RuleResult
    {
        public readonly ushort VisualId;
        public readonly byte VariantId;
        public readonly byte RotationId;

        public RuleResult(ushort visualId, byte variantId, byte rotationId = 0)
        {
            VisualId = visualId;
            VariantId = variantId;
            RotationId = rotationId;
        }
    }
}
