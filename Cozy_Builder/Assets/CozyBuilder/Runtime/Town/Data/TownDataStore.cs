namespace CozyBuilder.Town.Data
{
    public sealed class TownDataStore
    {
        private const int InitialCellCount = 0;

        public TownData Current { get; } = new TownData(InitialCellCount);
    }
}
