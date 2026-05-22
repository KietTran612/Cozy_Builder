using System.Collections.Generic;

namespace CozyBuilder.Town.Data
{
    public static class OrganicIslandGridGenerator
    {
        public static TownData Generate(int radius)
        {
            var coordinates = new List<GridCoord>((radius * 2 + 1) * (radius * 2 + 1));
            var cells = new List<CellData>(coordinates.Capacity);

            for (var y = -radius; y <= radius; y++)
            {
                for (var x = -radius; x <= radius; x++)
                {
                    var coord = new GridCoord(x, y);
                    if (!IsInsideIsland(coord, radius))
                    {
                        continue;
                    }

                    coordinates.Add(coord);
                    cells.Add(new CellData
                    {
                        Terrain = TerrainType.Grass,
                        Flags = CellFlags.Dirty
                    });
                }
            }

            MarkWaterfrontCells(coordinates, cells);
            return new TownData(coordinates.ToArray(), cells.ToArray());
        }

        private static bool IsInsideIsland(GridCoord coord, int radius)
        {
            var distance = coord.X * coord.X + coord.Y * coord.Y;
            var edgeNoise = Hash(coord.X, coord.Y) % 3 - 1;
            var adjustedRadius = radius + edgeNoise;
            return distance <= adjustedRadius * adjustedRadius;
        }

        private static void MarkWaterfrontCells(List<GridCoord> coordinates, List<CellData> cells)
        {
            var lookup = new HashSet<GridCoord>(coordinates);

            for (var i = 0; i < coordinates.Count; i++)
            {
                var coord = coordinates[i];
                var isWaterfront = false;

                foreach (var offset in GridNeighborhood.CardinalOffsets)
                {
                    if (!lookup.Contains(new GridCoord(coord.X + offset.X, coord.Y + offset.Y)))
                    {
                        isWaterfront = true;
                        break;
                    }
                }

                if (!isWaterfront)
                {
                    continue;
                }

                var cell = cells[i];
                cell.Flags |= CellFlags.HasWaterfront;
                cells[i] = cell;
            }
        }

        private static int Hash(int x, int y)
        {
            unchecked
            {
                var hash = x * 73856093 ^ y * 19349663;
                return hash < 0 ? -hash : hash;
            }
        }
    }
}
