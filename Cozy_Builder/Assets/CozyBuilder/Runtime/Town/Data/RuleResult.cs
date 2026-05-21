namespace CozyBuilder.Town.Data
{
    public readonly struct RuleResult
    {
        public readonly ushort VisualId;
        public readonly byte VariantId;

        public RuleResult(ushort visualId, byte variantId)
        {
            VisualId = visualId;
            VariantId = variantId;
        }
    }
}
