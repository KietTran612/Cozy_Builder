namespace CozyBuilder.Town.Data
{
    public sealed class TownData
    {
        public const int CurrentVersion = 1;

        public int Version { get; }
        public CellData[] Cells { get; }

        public TownData(int cellCount)
        {
            Version = CurrentVersion;
            Cells = new CellData[cellCount];
        }
    }
}
