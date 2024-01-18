using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using DungeonDesigner.Grid;

namespace DungeonDesigner.Sidebar
{
    public class SideBarItem : PictureBox
    {
        public readonly ItemType ItemType;
        public bool Selected;
        public List<Direction> DoorDirections;
        public Direction StaircaseDirection;
        public SideBarItem Staircase;
        
        public SideBarItem(Image image, string name, ItemType itemType)
        {
            this.Name = name;
            this.Image = image;
            this.ItemType = itemType;
            this.SizeMode = PictureBoxSizeMode.Zoom;
            this.MouseDown += OnClick;
            this.Paint += PaintBorder;
            switch (this.Name)
            {
                case @"ROOM_FOUR":
                    this.DoorDirections = new List<Direction>() {Direction.North, Direction.East, Direction.South, Direction.West};
                    this.StaircaseDirection = Direction.NullDirection;
                    break;
                case @"ROOM_THREE":
                    this.DoorDirections = new List<Direction>() {Direction.East, Direction.South, Direction.West};
                    this.StaircaseDirection = Direction.NullDirection;
                    break;
                case @"ROOM_TWO_CORNER":
                    this.DoorDirections = new List<Direction>() {Direction.North, Direction.East};
                    this.StaircaseDirection = Direction.NullDirection;
                    break;
                case @"ROOM_TWO_LINE":
                    this.DoorDirections = new List<Direction>() {Direction.East, Direction.West};
                    this.StaircaseDirection = Direction.NullDirection;
                    break;
                case @"ROOM_ONE":
                    this.DoorDirections = new List<Direction>() {Direction.East};
                    this.StaircaseDirection = Direction.NullDirection;
                    break;
                case @"STAIRS_MARKER":
                    this.DoorDirections = null;
                    this.StaircaseDirection = Direction.Down;
                    break;
                default:
                    this.DoorDirections = null;
                    this.StaircaseDirection = Direction.NullDirection;
                    break;
            }
        }

        private void OnClick(object sender, EventArgs e)
        {
            var me = (MouseEventArgs) e;
            if (me.Button.Equals(MouseButtons.Left))
            {
                if (this.Parent is SideBar)
                {
                    SideBar.Instance.SelectItem(this);
                }
                else
                {
                    if (this.Staircase != null)
                    {
                        this.Controls.Remove(this.Staircase);
                        this.Staircase = null;
                    }
                    else if (this.ItemType == ItemType.Marker)
                    {
                        this.StaircaseDirection = Direction.NullDirection;
                        ((SideBarItem) this.Parent).Staircase = null;
                        this.Parent.Controls.Remove(this);
                    }
                    else if (SideBar.Instance.SelectedItem.ItemType == ItemType.Room)
                    {
                        FloorGrid.Instance.Controls.Remove(this);
                    }
                    else
                    {
                        var item = new SideBarItem(SideBar.GetImageFromType(SideBar.Instance.SelectedItem),
                            SideBar.Instance.SelectedItem.Name, SideBar.Instance.SelectedItem.ItemType);
                        item.Size = new Size(
                            FloorGrid.Instance.GetColumnWidths()[0] / 3,
                            FloorGrid.Instance.GetRowHeights()[0] / 3);
                        item.Location = new Point(
                            this.Width / 2 - item.Width / 2,
                            this.Height / 2 - item.Height / 2);
                        this.Staircase = item;
                        this.Staircase.StaircaseDirection = Direction.Down;
                        this.Controls.Add(item);
                        item.BringToFront();
                    }
                }
            }
            else if (me.Button.Equals(MouseButtons.Right))
            {
                Image img;
                if (this.ItemType == ItemType.Room)
                {
                    img = this.Image;
                    img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    this.Image = img;
                    this.RotateDoorDirections();
                }
                else if (this.ItemType == ItemType.Marker)
                {
                    img = this.Image;
                    this.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    this.Image = img;
                    this.StaircaseDirection = this.StaircaseDirection == Direction.Up ? Direction.Down : Direction.Up;
                }
            }
        }

        public void RotateDoorDirections()
        {
            var newDoorDirections = new List<Direction>();
            if (this.DoorDirections.Contains(Direction.West)) newDoorDirections.Add(Direction.North);
            if (this.DoorDirections.Contains(Direction.North)) newDoorDirections.Add(Direction.East);
            if (this.DoorDirections.Contains(Direction.East)) newDoorDirections.Add(Direction.South);
            if (this.DoorDirections.Contains(Direction.South)) newDoorDirections.Add(Direction.West);
            this.DoorDirections = new List<Direction>(newDoorDirections);
        }

        private void PaintBorder(object sender, PaintEventArgs e)
        {
            if (this.Selected)
            {
                ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, Color.Green, ButtonBorderStyle.Solid);
            }
        }

        public string Export()
        {
            var exportString = "new RoomData(new List<Direction>() {";
            var count = 0;
            if (this.DoorDirections.Contains(Direction.North))
            {
                count++;
                exportString += "Direction.North";
                if (this.DoorDirections.Count > count) exportString += ", ";
            }
            if (this.DoorDirections.Contains(Direction.East))
            {
                count++;
                exportString += "Direction.East";
                if (this.DoorDirections.Count > count) exportString += ", ";
            }
            if (this.DoorDirections.Contains(Direction.South))
            {
                count++;
                exportString += "Direction.South";
                if (this.DoorDirections.Count > count) exportString += ", ";
            }
            if (this.DoorDirections.Contains(Direction.West))
            {
                exportString += "Direction.West";
            }
            exportString += "}";
            if (this.Staircase != null)
            {
                if (this.Staircase.StaircaseDirection == Direction.Up) exportString += ", Direction.Up";
                if (this.Staircase.StaircaseDirection == Direction.Down) exportString += ", Direction.Down";
            }
            exportString += ")";
            return exportString;
        }
    }
}