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

        public bool TryPlaceBlock(GridCoord coord, ushort colorId = 0, ushort materialId = 0)
        {
            if (!townDataStore.Current.TryGetCell(coord, out var cell))
            {
                return false;
            }

            if (cell.Height == ushort.MaxValue)
            {
                return false;
            }

            cell.Height++;
            cell.ColorId = colorId;
            cell.MaterialId = materialId;
            cell.Flags |= CellFlags.Occupied | CellFlags.Dirty;

            townDataStore.Current.TrySetCell(coord, cell);
            MarkDirtyWithNeighbors(coord);
            return true;
        }

        public bool TryDeleteBlock(GridCoord coord)
        {
            if (!townDataStore.Current.TryGetCell(coord, out var cell) || cell.Height == 0)
            {
                return false;
            }

            cell.Height--;
            if (cell.Height == 0)
            {
                cell.Flags &= ~CellFlags.Occupied;
            }

            cell.Flags |= CellFlags.Dirty;
            townDataStore.Current.TrySetCell(coord, cell);
            MarkDirtyWithNeighbors(coord);
            return true;
        }

        public void MarkDirty(GridCoord coord)
        {
            townVisualRebuilder.MarkDirty(coord);
        }

        private void MarkDirtyWithNeighbors(GridCoord coord)
        {
            MarkDirty(coord);

            foreach (var offset in GridNeighborhood.CardinalOffsets)
            {
                var neighbor = new GridCoord(coord.X + offset.X, coord.Y + offset.Y);
                if (townDataStore.Current.Contains(neighbor))
                {
                    MarkDirty(neighbor);
                }
            }
        }
    }
}
