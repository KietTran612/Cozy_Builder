using CozyBuilder.Town.Data;

namespace CozyBuilder.Town.Rules
{
    public sealed class RuleEvaluator
    {
        public RuleResult Evaluate(GridCoord coord, int layer, in CellData cell, TownData townData)
        {
            // Tránh lỗi nếu layer không hợp lệ hoặc vượt quá chiều cao ô đất
            if (layer <= 0 || layer > cell.Height)
            {
                return new RuleResult(0, 0, 0);
            }

            // 1. Waterfront Foundation (Móng cột chống)
            // Nếu ô đất có cờ waterfront và là tầng dưới cùng (layer == 1)
            if ((cell.Flags & CellFlags.HasWaterfront) != CellFlags.None && layer == 1)
            {
                return new RuleResult(4, 0, 0); // VisualId = 4 (Stilts)
            }

            // Lấy thông tin chiều cao lân cận hướng cardinal
            var eastCoord = new GridCoord(coord.X + 1, coord.Y);
            var westCoord = new GridCoord(coord.X - 1, coord.Y);
            var northCoord = new GridCoord(coord.X, coord.Y + 1);
            var southCoord = new GridCoord(coord.X, coord.Y - 1);

            ushort eastHeight = townData.TryGetCell(eastCoord, out var eastCell) ? eastCell.Height : (ushort)0;
            ushort westHeight = townData.TryGetCell(westCoord, out var westCell) ? westCell.Height : (ushort)0;
            ushort northHeight = townData.TryGetCell(northCoord, out var northCell) ? northCell.Height : (ushort)0;
            ushort southHeight = townData.TryGetCell(southCoord, out var southCell) ? southCell.Height : (ushort)0;

            // 2. Tầng trên cùng (layer == cell.Height)
            if (layer == cell.Height)
            {
                // Single Standalone House (Nhà nhỏ độc lập 1 tầng)
                if (cell.Height == 1 &&
                    eastHeight == 0 && westHeight == 0 && northHeight == 0 && southHeight == 0)
                {
                    return new RuleResult(1, 0, 0); // VisualId = 1 (Small House)
                }

                // Tower Top (Mái tháp tròn nhọn)
                // Nếu ô này cao hơn hẳn các ô xung quanh (chiều cao lân cận đều bé hơn layer hiện tại)
                if (layer > eastHeight && layer > westHeight && layer > northHeight && layer > southHeight)
                {
                    return new RuleResult(3, 0, 0); // VisualId = 3 (Tower Top)
                }

                // Row Houses Roof (Mái nhà liền kề)
                // Căn chỉnh trục mái chạy dọc kết nối với hàng xóm lân cận
                byte rotationId = 0; // 0 = Đông-Tây, 1 = Bắc-Nam
                bool connectEastWest = eastHeight >= layer || westHeight >= layer;
                bool connectNorthSouth = northHeight >= layer || southHeight >= layer;

                if (connectNorthSouth && !connectEastWest)
                {
                    rotationId = 1; // Xoay 90° để mái chạy Bắc-Nam (tương ứng 90 độ trong Unity)
                }
                else if (connectEastWest && !connectNorthSouth)
                {
                    rotationId = 0; // Chạy Đông-Tây (tương ứng 0 độ)
                }
                else if (connectEastWest && connectNorthSouth)
                {
                    // Nếu kết nối cả hai hướng (ngã ba/ngã tư), ưu tiên hướng có lân cận cao hơn
                    int maxEastWest = System.Math.Max(eastHeight, westHeight);
                    int maxNorthSouth = System.Math.Max(northHeight, southHeight);
                    rotationId = maxNorthSouth > maxEastWest ? (byte)1 : (byte)0;
                }
                else
                {
                    // Mặc định nếu không có lân cận nào đạt chiều cao tầng
                    rotationId = 0;
                }

                return new RuleResult(2, 0, rotationId); // VisualId = 2 (House Roof)
            }

            // 3. Các tầng tường phía dưới (layer < cell.Height)
            // Nếu có ít nhất một ô lân cận có chiều cao >= tầng hiện tại (tức là nằm trong dãy phố)
            bool hasNeighborAtLayer = eastHeight >= layer || westHeight >= layer || northHeight >= layer || southHeight >= layer;

            if (hasNeighborAtLayer)
            {
                return new RuleResult(5, 0, 0); // VisualId = 5 (House Wall)
            }
            else
            {
                // Tháp đứng cô độc ở tầng này
                return new RuleResult(6, 0, 0); // VisualId = 6 (Tower Wall)
            }
        }
    }
}
