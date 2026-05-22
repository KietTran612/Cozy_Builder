using System.Collections.Generic;
using CozyBuilder.Town.Data;

namespace CozyBuilder.Town.Rendering
{
    public sealed class TownVisualRebuilder
    {
        private readonly Queue<GridCoord> dirtyQueue = new Queue<GridCoord>();
        private readonly HashSet<GridCoord> dirtySet = new HashSet<GridCoord>();

        public int DirtyCount => dirtyQueue.Count;

        public void MarkDirty(GridCoord coord)
        {
            if (!dirtySet.Add(coord))
            {
                return;
            }

            dirtyQueue.Enqueue(coord);
        }

        public bool TryDequeueDirty(out GridCoord coord)
        {
            if (dirtyQueue.Count == 0)
            {
                coord = default;
                return false;
            }

            coord = dirtyQueue.Dequeue();
            dirtySet.Remove(coord);
            return true;
        }

        public int CopyDirtyCoords(GridCoord[] buffer, int maxCount)
        {
            if (buffer == null || maxCount <= 0)
            {
                return 0;
            }

            var copied = 0;
            var limit = maxCount < buffer.Length ? maxCount : buffer.Length;
            foreach (var coord in dirtyQueue)
            {
                if (copied >= limit)
                {
                    break;
                }

                buffer[copied] = coord;
                copied++;
            }

            return copied;
        }
    }
}
