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
        private string _pwd;
        private string _pwd_next;
        private bool _enabled;
        private System.IO.FileSystemWatcher _watcher;

        override protected void Initialize()
        {
            //Run the initializer for a regular file list
            base.Initialize();
            //Create a new file system watcher and instruct it how
            //to push to the GUI
            _watcher = new System.IO.FileSystemWatcher();
            _watcher.IncludeSubdirectories = false;
            _watcher.Changed += (s, e) =>
            {
                MainForm.OnGuiThread(updateCollection);
            };
        }

        /// <summary>
        /// Refresh the list of files
        /// </summary>
        protected void updateCollection()
        {
            _data.Clear();
            if(_enabled) foreach(string path in System.IO.Directory.EnumerateFiles(_pwd))
            {
                    AddToList(new FileListItem(path));
            }
        }

        /// <summary>
        /// Set the string for the current directory of this file view
        /// </summary>
        /// <remarks>
        /// Setting this property will ensure the path is valid. If it is not,
        /// the path will not update and <see cref="ViewPathStringIsValid"/>
        /// will become <c>false</c> until a new valid path is assigned.
        /// </remarks>
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

        /// <summary>
        /// Turns on the file system watcher to get updates from the OS.
        /// </summary>
        protected void WatcherOn()
        {
            _watcher.Path = _pwd;
            _watcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// Turns off the file system watcher to stop updates from the OS.
        /// </summary>
        protected void WatcherOff()
        {
            _watcher.EnableRaisingEvents = false;
        }
        
        /// <summary>
        /// Was the last submitted path a valid path?
        /// </summary>
        public bool ViewPathStringIsValid
        {
            get
            {
                return _pwd_next == null;
            }
        }

        /// <summary>
        /// Not in any directory, empty everything
        /// </summary>
        public void Clear()
        {
            _enabled = false;
            _pwd = null;
            _pwd_next = null;
            WatcherOff();
            ClearList();
        }

        /// <summary>
        /// Initialize a new local file list, inside of the current directory
        /// </summary>
        public LocalFileList()
        {
            Initialize();
            this.PathToWorkingDirectory = System.IO.Directory.GetCurrentDirectory();
        }

        /// <summary>
        /// Initialize a new local file list, inside the given directory
        /// </summary>
        /// <param name="pwd">A path to the directory to start in</param>
        public LocalFileList(string pwd)
        {
            Initialize();
            this.PathToWorkingDirectory = pwd;
        }
    }
}
