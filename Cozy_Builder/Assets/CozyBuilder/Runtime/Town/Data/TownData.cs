using System;
using System.Collections.Generic;

namespace CozyBuilder.Town.Data
{
    public sealed class TownData
    {
        public const int CurrentVersion = 1;

        private readonly Dictionary<GridCoord, int> coordToIndex;

        public int Version { get; }
        public GridCoord[] Coordinates { get; }
        public CellData[] Cells { get; }

        public int CellCount => Cells.Length;

        public TownData(GridCoord[] coordinates, CellData[] cells)
        {
            if (coordinates == null)
            {
                throw new ArgumentNullException(nameof(coordinates));
            }

            if (cells == null)
            {
                throw new ArgumentNullException(nameof(cells));
            }

            if (coordinates.Length != cells.Length)
            {
                throw new ArgumentException("Coordinate and cell counts must match.", nameof(cells));
            }

            Version = CurrentVersion;
            Coordinates = coordinates;
            Cells = cells;
            coordToIndex = new Dictionary<GridCoord, int>(coordinates.Length);

            for (var i = 0; i < coordinates.Length; i++)
            {
                coordToIndex[coordinates[i]] = i;
            }
        }

        public bool Contains(GridCoord coord)
        {
            return coordToIndex.ContainsKey(coord);
        }

        public bool TryGetIndex(GridCoord coord, out int index)
        {
            return coordToIndex.TryGetValue(coord, out index);
        }

        public bool TryGetCell(GridCoord coord, out CellData cell)
        {
            if (coordToIndex.TryGetValue(coord, out var index))
            {
                cell = Cells[index];
                return true;
            }

            cell = default;
            return false;
        }

        public bool TrySetCell(GridCoord coord, in CellData cell)
        {
            if (!coordToIndex.TryGetValue(coord, out var index))
            {
                return false;
            }

            Cells[index] = cell;
            return true;
        }
    }
}
