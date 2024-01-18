using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using DungeonDesigner.Grid;
using DungeonDesigner.Sidebar;

namespace DungeonDesigner
{
    public partial class DungeonDesignerForm : Form
    {
        public DungeonDesignerForm()
        {
            this.AddComponents();
            this.InitializeComponent();
            this.InitialiseComponents();
        }

        private void AddComponents()
        {
            this.Controls.Add(SideBar.Instance);
            this.Controls.Add(FloorGrid.Instance);
        }

        private void InitialiseComponents()
        {
            this.SetComponentSizes();
        }

        private void SetComponentSizes()
        {
            SideBar.Instance.Size = new Size(this.Width / 5, this.Height);
            SideBar.Instance.Location = new Point(0, 0);
            SideBar.Instance.ResizeComponent();
            FloorGrid.Instance.Size = new Size(this.Height / 11 * 9, this.Height / 11 * 9);
            FloorGrid.Instance.Location = new Point(
                (this.Width - FloorGrid.Instance.Width) / 2 + SideBar.Instance.Width / 2,
                (this.Height - FloorGrid.Instance.Height) / 2);
            FloorGrid.Instance.ResizeComponent();
        }

        private void OnFormResize(object sender, EventArgs e)
        {
            this.SetComponentSizes();
        }

        private void DungeonDesignerForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                SideBar.Instance.SelectItem(null);
            }
        }
    }
}