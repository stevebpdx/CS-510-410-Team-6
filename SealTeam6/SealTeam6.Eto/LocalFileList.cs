using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using Eto;
using Eto.Forms;
using Eto.IO;
using Eto.Threading;

namespace SealTeam6.EtoGUI
{
    class LocalFileList: FileList
    {
        private object _lock;
        private string _pwd;
        private string _pwd_next;
        private bool _enabled;
        private System.IO.FileSystemWatcher _watcher;

        override protected void Initialize()
        {
            base.Initialize();
            _lock = new object();
            _watcher = new System.IO.FileSystemWatcher();
            _watcher.IncludeSubdirectories = false;
            _watcher.Changed += (s, e) =>
            {
                MainForm.OnGuiThread(updateCollection);
            };
        }

        //private void OnChangedThreadHelper(object sender, EventArgs e)
        //{
        //    if (!Eto.Threading.Thread.CurrentThread.IsMain)
        //        Eto.Threading.Thread.MainThread.Properties.TriggerEvent(_lock, sender, e);
        //    else updateCollection();
        //}

        //private void OnChanged(object sender, EventArgs e)
        //{
        //    if (_guithread == System.Threading.SynchronizationContext.Current) updateCollection();
        //    else _guithread.Send(updateCollection, e);
        //}

        private void updateCollection(object state)
        {
            updateCollection();
        }

        private void updateCollection()
        {
            _data.Clear();
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
                if (value != null && System.IO.Directory.Exists(value))
                {
                    _enabled = true;
                    _pwd = value;
                    _pwd_next = null;
                    updateCollection();
                    WatcherOn();
                }
                else
                {
                    _pwd_next = value;
                }
            }
        }

        protected void WatcherOn()
        {
            _watcher.Path = _pwd;
            _watcher.EnableRaisingEvents = true;
        }

        protected void WatcherOff()
        {
            _watcher.EnableRaisingEvents = false;
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
            WatcherOff();
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
