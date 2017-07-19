using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Eto.Forms;
using Eto.Drawing;
using Eto.Serialization.Json;
using Eto;
using System.Threading;

namespace SealTeam6.EtoGUI
{
    public abstract class FileList
    {
        protected ListBox _view;
        protected ObservableCollection<object> _data;
        protected virtual void Initialize()
        {
            //Construct the view and its data store
            _view = new ListBox();
            _data = new ObservableCollection<object>();
            this.View.DataStore = _data;

            //Connect the view to its data store.
            //These "data bindings" allow the view to update itself when the
            //underlying ObservableCollection changes
            this.View.ItemTextBinding = new PropertyBinding<string>("Text");
            this.View.ItemKeyBinding = new PropertyBinding<string>("Key");
            this.View.ItemImageBinding = new PropertyBinding<Image>("Image");
        }

        /// <summary>
        /// A visual representation of the files for building a GUI
        /// </summary>
        public ListBox View
        {
            get
            {
                return _view;
            }
        }

        /// <summary>
        /// Put a new file onto this file list
        /// </summary>
        /// <param name="datum">A new file to add to the list</param>
        protected virtual void AddToList(FileListItem datum)
        {
            _data.Add(datum);
        }

        /// <summary>
        /// Clear the file list
        /// </summary>
        protected virtual void ClearList()
        {
            _data.Clear();
        }
    }
}
