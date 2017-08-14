using System;
using System.Collections.Generic;
using Eto.Forms;
using Eto.Drawing;
using System.Threading;

namespace SealTeam6.EtoGUI
{
    public partial class MainForm : Form
    {
        public static Action<Action> OnGuiThread;

        LocalFileList fileList = new LocalFileList();
        public MainForm(Action<Action> caller): base()
        {
            this.Title = "SampleFileBrowser";
            this.Content = new ServerLoginForm().View; //fileList.View;
            OnGuiThread = caller;
        }
    }
}
