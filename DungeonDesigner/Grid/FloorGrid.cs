using System;
using System.Drawing;
using System.Windows.Forms;
using DungeonDesigner.Sidebar;

namespace DungeonDesigner.Grid
{
    public class FloorGrid : TableLayoutPanel
    {
        public static readonly FloorGrid Instance = new FloorGrid();
        
        
        private FloorGrid()
        {
            this.InitialiseComponent();
            this.SetComponentSizes();
            this.MouseDown += GridClick;
        }

        private void InitialiseComponent()
        {
            this.BackColor = SystemColors.Control;
            this.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            this.RowCount = 9;
            this.ColumnCount = 9;
        }

        private void GridClick(object sender, EventArgs e)
        {
            var me = (MouseEventArgs) e;
            var location = GetRowColIndex(this.PointToClient(Cursor.Position));
            if (me.Button.Equals(MouseButtons.Left))
            {
                if (SideBar.Instance.itemSelected)
                {
                    var item = new SideBarItem(SideBar.GetImageFromType(SideBar.Instance.SelectedItem),
                        SideBar.Instance.SelectedItem.Name, SideBar.Instance.SelectedItem.ItemType);
                    if (item.ItemType == ItemType.Room)
                    {
                        item.Size = new Size(this.GetColumnWidths()[0], this.GetRowHeights()[0]);
                        this.Controls.Add(item, location.X, location.Y);
                    }
                    else if (item.ItemType == ItemType.Marker &&
                             GetControlFromPosition(location.X,location.Y) != null)
                    {
                        item.Size = new Size(
                            FloorGrid.Instance.GetColumnWidths()[0] / 3,
                            FloorGrid.Instance.GetRowHeights()[0] / 3);
                        item.Location = new Point(
                            this.Width / 2 - item.Width / 2,
                            this.Height / 2 - item.Height / 2);
                        var room = (SideBarItem) GetControlFromPosition(location.X, location.Y);
                        room.Staircase = item;
                        room.Staircase.StaircaseDirection = Direction.Down;
                        room.Controls.Add(item);
                        item.BringToFront();
                    }
                }
                else
                {
                    this.Controls.Remove(this.GetControlFromPosition(location.X, location.Y));
                }
            }
            else if (me.Button.Equals(MouseButtons.Right))
            {
                Image img;
                var item = (SideBarItem) GetControlFromPosition(location.X, location.Y);
                if (item == null) return;
                if (SideBar.Instance.SelectedItem.ItemType == ItemType.Marker && item.Staircase != null)
                {
                    img = item.Staircase.Image;
                    item.Staircase.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    item.Staircase.Image = img;
                    item.Staircase.StaircaseDirection = item.StaircaseDirection == Direction.Up ? Direction.Down : Direction.Up;
                }
                else 
                {
                    img = item.Image;
                    img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    item.Image = img;
                    item.RotateDoorDirections();
                }
            }
        }
        
        private Point GetRowColIndex(Point point)
        {
            if (point.X > this.Width || point.Y > this.Height)
                return new Point(-1, -1);

            int w = this.Width;
            int h = this.Height;
            int[] widths = this.GetColumnWidths();

            int i;
            for (i = widths.Length - 1; i >= 0 && point.X < w; i--)
                w -= widths[i];
            int col = i + 1;

            int[] heights = this.GetRowHeights();
            for (i = heights.Length - 1; i >= 0 && point.Y < h; i--)
                h -= heights[i];

            int row = i + 1;

            return new Point(col, row);
        }
        
        private void SetComponentSizes()
        {
            for (var i = 0; i < 9; i++)
            {
                this.RowStyles.Add(new RowStyle(SizeType.Percent, this.Height));
                this.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, this.Width));
            }
            this.Margin = new Padding(1);
            this.Padding = new Padding(1);
        }

        public void ResizeComponent()
        {
            SetComponentSizes();
        }

        public string Export()
        {
            var exportString = "return new List<List<RoomData>>()\n" +
                               "{\n";
            for (var col = 0; col < this.ColumnCount; col++)
            {
                exportString += "    // col " + col + "\n" +
                                "    new List<RoomData>()\n" +
                                "    {\n";
                for (var cell = 0; cell < this.RowCount; cell++)
                {
                    exportString += "        " + ExportCell(col, cell);
                    if (cell != this.RowCount - 1) exportString += ",\n";
                    else exportString += "\n";
                }

                if (col != this.ColumnCount - 1) exportString += "    },\n";
                else exportString += "    }\n";
            }
            exportString += "};\n";
            return exportString;
        }

        private string ExportCell(int col, int row)
        {
            var item = (SideBarItem) this.GetControlFromPosition(col, row);
            if (item != null) return item.Export();
            else return "new RoomData(null)";
        }
    }
}