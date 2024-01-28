using System;
using System.Diagnostics;
using System.Windows.Forms;
using DungeonDesigner.Grid;

namespace DungeonDesigner.Sidebar
{
    public sealed class CopyButton : Button
    {
        public static readonly CopyButton Instance = new CopyButton();
        
        private CopyButton()
        {
            this.Name = "COPY_BUTTON";
            this.Text = @"COPY";
            this.TabStop = false;
            this.Click += new EventHandler(CopyFloor);
        }

        private void CopyFloor(object sender, EventArgs e)
        {
            var exportString = FloorGrid.Instance.Export();
            Clipboard.SetText(exportString);
        }
    }
}