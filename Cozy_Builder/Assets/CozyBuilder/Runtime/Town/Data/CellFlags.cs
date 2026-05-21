using System;

namespace CozyBuilder.Town.Data
{
    [Flags]
    public enum CellFlags : byte
    {
        None = 0,
        Occupied = 1 << 0,
        Dirty = 1 << 1,
        HasWaterfront = 1 << 2
    }
}
