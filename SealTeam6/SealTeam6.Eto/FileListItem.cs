using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto.Forms;
using Eto.Drawing;
using Eto.IO;

namespace SealTeam6.EtoGUI
{
    /// <summary>
    /// A key value pair that can be kept in a list view. 
    /// </summary>
    public struct FileListItem: IListItem, IImageListItem
    {
        private string _key;
        private string _fileExtension;
        private string _fileName;
        private string _ftpServerPrefix;
        private bool _dirty;
        private string _rename;
        private Image _image;

        /// <summary>
        /// The name that will be displayed in the GUI for this item. 
        /// Setting this value sets up a job to rename this file..
        /// </summary>
        public string Text
        {
            get
            {
                if (_rename != null) return _rename;
                else return _fileName;
            }
            set
            {
                if(value != this.Text) _rename = value;
            }
        }

        /// <summary>
        /// The icon that will be displayed for a particular file
        /// </summary>
        public Image Image
        {
            get
            {
                if (_ftpServerPrefix != null) return _image;
                else return Eto.IO.SystemIcons.GetFileIcon(_key, IconSize.Small);
            }
        }
        
        /// <summary>
        /// The key, an optional identifier for this item. Ideally it should be unique.
        /// It should be a fully qualified path to some resource.
        /// </summary>
        /// <remarks>
        /// This key is never null, thus the display text will never be returned instead of a key.
        /// </remarks>
        public string Key
        {
            get
            {
                if (this.Remote) return _ftpServerPrefix + _key;
                else return _key;
            }
        }

        /// <summary>
        /// What is this file's extension? This can return null if there is none.
        /// </summary>
        public string Extension
        {
            get
            {
                return _fileExtension;
            }
        }
        
        /// <summary>
        /// Is this file local or remote?
        /// </summary>
        public bool Remote
        {
            get
            {
                return _ftpServerPrefix != null;
            }
        }

        /// <summary>
        /// If this file is a remote file, get the FTP server. This is null if it is local.
        /// </summary>
        public string FtpServer
        {
            get
            {
                return _ftpServerPrefix;
            }
        }

        /// <summary>
        /// Does this file have uncommited operations associated with it?
        /// </summary>
        public bool UncommitedOperations
        {
            get
            {
                return _dirty || _rename != null;
            }
        }
        
        /// <summary>
        /// Create an entry for a file on the local machine.
        /// </summary>
        /// <param name="path">A path to the file to make an entry for.</param>
        public FileListItem(string path)
        {
            _key = path;
            _dirty = false;
            _ftpServerPrefix = null;
            _rename = null;
            _fileName = System.IO.Path.GetFileName(_key);
            _fileExtension = System.IO.Path.GetExtension(_key);
            _image = Eto.IO.SystemIcons.GetFileIcon(_key, IconSize.Small);
        }

        //Document later, because this could probably be made smarter anyway
        public FileListItem(string path, string fileName, string fileExtension, Image fileImage, string ftpServerPrefix)
        {
            _key = path;
            _dirty = false;
            _ftpServerPrefix = ftpServerPrefix;
            _rename = null;
            _fileName = fileName;
            _fileExtension = fileExtension;
            _image = fileImage;
        }
    }
}
