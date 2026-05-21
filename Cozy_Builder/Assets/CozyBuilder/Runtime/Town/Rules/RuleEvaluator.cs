using CozyBuilder.Town.Data;

namespace CozyBuilder.Town.Rules
{
    public sealed class RuleEvaluator
    {
        public RuleResult Evaluate(in CellData cell)
        {
            return new RuleResult(cell.Height, 0);
        }
    }
}
