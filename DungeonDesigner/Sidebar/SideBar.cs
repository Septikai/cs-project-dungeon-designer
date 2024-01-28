using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DungeonDesigner.Properties;

namespace DungeonDesigner.Sidebar
{
    public class SideBar : Panel
    {
        public static readonly SideBar Instance = new SideBar();
        private List<SideBarItem> _items = new List<SideBarItem>()
        {
            new SideBarItem(Resources.room_four, "ROOM_FOUR", ItemType.Room),
            new SideBarItem(Resources.room_three, "ROOM_THREE", ItemType.Room),
            new SideBarItem(Resources.room_two_corner, "ROOM_TWO_CORNER", ItemType.Room),
            new SideBarItem(Resources.room_two_line, "ROOM_TWO_LINE", ItemType.Room),
            new SideBarItem(Resources.room_one, "ROOM_ONE", ItemType.Room),
            new SideBarItem(Resources.stairs_marker, "STAIRS_MARKER", ItemType.Marker),
        };
        public bool itemSelected;
        public SideBarItem SelectedItem;

        private SideBar()
        {
            this.BorderStyle = BorderStyle.FixedSingle;
            this.InitialiseComponents();
        }

        public void SelectItem(SideBarItem toSelect)
        {
            this.itemSelected = toSelect != null;
            this.SelectedItem = toSelect;
            foreach (var item in _items)
            {
                item.Selected = toSelect != null && item.Name == toSelect.Name;
                item.Invalidate();
            }
        }

        private void InitialiseComponents()
        {
            foreach (var item in _items)
            {
                this.Controls.Add(item);
            }
            this.Controls.Add(CopyButton.Instance);
            this.Controls.Add(ExportButton.Instance);
            this.Controls.Add(ClearButton.Instance);
            this.Controls.Add(QuitButton.Instance);
            SetComponents();
        }

        private void SetComponents()
        {
            for (var i = 0; i < _items.Count; i++)
            {
                _items[i].Size = new Size(this.Width / 4, this.Width / 4);
                if (i % 2 == 0)
                {
                    _items[i].Location = new Point(
                        this.Width / 4 - _items[i].Width / 2,
                        this.Height / (_items.Count + 1) * (i + 1));
                }
                else
                {
                    _items[i].Location = new Point(
                        (this.Width / 4) * 3 - _items[i].Width / 2,
                        this.Height / (_items.Count + 1) * (i + 1 - 1));
                }
            }

            CopyButton.Instance.Size = new Size(this.Width / 3, this.Width / 3 / 3);
            CopyButton.Instance.Location = new Point(this.Width / 9, this.Width / 3 / 3 / 2);
            ExportButton.Instance.Size = new Size(this.Width / 3, this.Width / 3 / 3);
            ExportButton.Instance.Location = new Point(this.Width * 5 / 9, this.Width / 3 / 3 / 2);
            ClearButton.Instance.Size = new Size(this.Width / 3, this.Width / 3 / 3);
            ClearButton.Instance.Location = new Point(this.Width / 9, this.Height - ClearButton.Instance.Height - this.Width / 3 / 3 / 2);
            QuitButton.Instance.Size = new Size(this.Width / 3, this.Width / 3 / 3);
            QuitButton.Instance.Location = new Point(this.Width * 5 / 9, this.Height - ClearButton.Instance.Height - this.Width / 3 / 3 / 2);
        }
        
        public void ResizeComponent()
        {
            SetComponents();
        }

        public static Image GetImageFromType(SideBarItem item)
        {
            switch (item.Name)
            {
                case @"ROOM_FOUR":
                    return Resources.room_four;
                case @"ROOM_THREE":
                    return Resources.room_three;
                case @"ROOM_TWO_CORNER":
                    return Resources.room_two_corner;
                case @"ROOM_TWO_LINE":
                    return Resources.room_two_line;
                case @"ROOM_ONE":
                    return Resources.room_one;
                case @"STAIRS_MARKER":
                    return Resources.stairs_marker;
                default:
                    return null;
            }
        }
    }
}