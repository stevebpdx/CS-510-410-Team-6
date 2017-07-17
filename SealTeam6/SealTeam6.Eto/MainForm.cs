using System;
using System.Collections.Generic;
using Eto.Forms;
using Eto.Drawing;

namespace SealTeam6.EtoGUI
{
    public partial class MainForm : Form
    {
        LocalFileList fileList = new LocalFileList();
        public MainForm(): base()
        {
            this.Title = "SampleFileBrowser";
            this.Content = fileList.View;
        }
    }
}
