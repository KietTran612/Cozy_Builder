using CozyBuilder.Town.Data;

namespace CozyBuilder.Town.Debugging
{
    public sealed class PrototypeTownDebugState
    {
        public bool HasSelectedCoord { get; private set; }
        public GridCoord SelectedCoord { get; private set; }

        public void Select(GridCoord coord)
        {
            HasSelectedCoord = true;
            SelectedCoord = coord;
        }

        public void ClearSelection()
        {
            HasSelectedCoord = false;
            SelectedCoord = default;
        }
    }
}
