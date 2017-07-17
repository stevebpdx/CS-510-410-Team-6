using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SealTeam6.EtoGUI
{
    class LocalFileList: FileList
    {
        private string _pwd;
        private string _pwd_next;
        private bool _enabled;

        private void updateCollection()
        {
            ClearList();
            if(_enabled) foreach(string path in System.IO.Directory.EnumerateFiles(_pwd))
            {
                    AddToList(new FileListItem(path));
            }
        }

        public string PathToWorkingDirectory
        {
            get
            {
                return _pwd;
            }
            set
            {
                if (System.IO.Directory.Exists(value))
                {
                    _enabled = true;
                    _pwd = value;
                    _pwd_next = null;
                    updateCollection();
                }
                else
                {
                    _pwd_next = value;
                }
            }
        }

        
        public bool ViewPathStringIsValid
        {
            get
            {
                return _pwd_next == null;
            }
        }

        public void Clear()
        {
            _enabled = false;
            _pwd = null;
            _pwd_next = null;
            ClearList();
        }

        public LocalFileList()
        {
            Initialize();
            this.PathToWorkingDirectory = System.IO.Directory.GetCurrentDirectory();
        }

        public LocalFileList(string pwd)
        {
            Initialize();
            this.PathToWorkingDirectory = pwd;
        }
    }
}
