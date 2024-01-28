using System;
using System.Diagnostics;
using System.IO;
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
            var sfd = new SaveFileDialog();
            sfd.Title = @"Save To Text File";
            sfd.Filter = "Text Files | *.txt";
            var result = sfd.ShowDialog();

            if (result == DialogResult.OK)
            {
                File.WriteAllText(sfd.FileName, exportString);
            }
        }
    }
}