using System;
using System.Windows.Forms;
using DungeonDesigner;

namespace DungeonDesigner
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Run the program
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new DungeonDesignerForm());
        }
    }
}