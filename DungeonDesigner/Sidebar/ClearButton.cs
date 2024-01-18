using System;
using System.Windows.Forms;
using DungeonDesigner.Grid;

namespace DungeonDesigner.Sidebar
{
    public class ClearButton : Button
    {
        public static readonly ClearButton Instance = new ClearButton();
        
        private ClearButton()
        {
            this.Name = "CLEAR_BUTTON";
            this.Text = @"CLEAR FLOOR";
            this.TabStop = false;
            this.Click += new EventHandler(ClearFloor);
        }

        private void ClearFloor(object sender, EventArgs e)
        {
            FloorGrid.Instance.Controls.Clear();
        }
    }
}