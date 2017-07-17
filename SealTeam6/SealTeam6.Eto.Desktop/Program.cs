using System;
using Eto;
using Eto.Forms;

namespace SealTeam6.EtoGUI.Desktop
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var app = new Application(Platform.Detect);
            app.Run(new MainForm(app.Invoke));
        }
    }
}
