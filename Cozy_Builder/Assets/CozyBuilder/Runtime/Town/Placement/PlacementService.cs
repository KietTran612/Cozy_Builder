using CozyBuilder.Town.Data;
using CozyBuilder.Town.Rendering;
using CozyBuilder.Town.Rules;

namespace CozyBuilder.Town.Placement
{
    public sealed class PlacementService
    {
        private readonly TownDataStore townDataStore;
        private readonly RuleEvaluator ruleEvaluator;
        private readonly TownVisualRebuilder townVisualRebuilder;

        public PlacementService(
            TownDataStore townDataStore,
            RuleEvaluator ruleEvaluator,
            TownVisualRebuilder townVisualRebuilder)
        {
            this.townDataStore = townDataStore;
            this.ruleEvaluator = ruleEvaluator;
            this.townVisualRebuilder = townVisualRebuilder;
        }

        public TownData TownData => townDataStore.Current;

        public RuleResult Preview(in CellData cell)
        {
            return ruleEvaluator.Evaluate(cell);
        }

        public void MarkDirty(GridCoord coord)
        {
            townVisualRebuilder.MarkDirty(coord);
        }
    }
}
