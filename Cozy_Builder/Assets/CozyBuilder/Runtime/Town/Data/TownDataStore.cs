namespace CozyBuilder.Town.Data
{
    public sealed class TownDataStore
    {
        private const int InitialIslandRadius = 4;

        public TownData Current { get; } = OrganicIslandGridGenerator.Generate(InitialIslandRadius);
    }
}
