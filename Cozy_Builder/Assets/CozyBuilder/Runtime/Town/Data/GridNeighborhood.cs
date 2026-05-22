namespace CozyBuilder.Town.Data
{
    public static class GridNeighborhood
    {
        public static readonly GridCoord[] CardinalOffsets =
        {
            new GridCoord(1, 0),
            new GridCoord(-1, 0),
            new GridCoord(0, 1),
            new GridCoord(0, -1)
        };
    }
}
