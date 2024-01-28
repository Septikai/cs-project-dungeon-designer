using System;
using System.Windows.Forms;
using DungeonDesigner.Grid;

namespace DungeonDesigner.Sidebar
{
    public class QuitButton : Button
    {
        public static readonly QuitButton Instance = new QuitButton();
        
        private QuitButton()
        {
            this.Name = "QUIT_BUTTON";
            this.Text = @"EXIT DESIGNER";
            this.TabStop = false;
            this.Click += new EventHandler(QuitApp);
        }

        private void QuitApp(object sender, EventArgs e)
        {
            Application.ExitThread();
        }
    }
}