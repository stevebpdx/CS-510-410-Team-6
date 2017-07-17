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
            new Application(Platform.Detect).Run(new MainForm());
        }
    }
}
