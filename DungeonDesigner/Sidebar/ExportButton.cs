using System;
using System.Diagnostics;
using System.Windows.Forms;
using DungeonDesigner.Grid;

namespace DungeonDesigner.Sidebar
{
    public sealed class ExportButton : Button
    {
        public static readonly ExportButton Instance = new ExportButton();
        
        private ExportButton()
        {
            this.Name = "EXPORT_BUTTON";
            this.Text = @"EXPORT";
            this.TabStop = false;
            this.Click += new EventHandler(ExportFloor);
        }

        private void ExportFloor(object sender, EventArgs e)
        {
            var exportString = FloorGrid.Instance.Export();
            Debug.Write(exportString);
            Clipboard.SetText(exportString);
        }
    }
}