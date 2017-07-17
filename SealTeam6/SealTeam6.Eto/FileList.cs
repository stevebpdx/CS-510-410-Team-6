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
            _view = new ListBox();
            _data = new ObservableCollection<object>();
            this.View.DataStore = _data;
            this.View.ItemTextBinding = new PropertyBinding<string>("Text");
            this.View.ItemKeyBinding = new PropertyBinding<string>("Key");
            this.View.ItemImageBinding = new PropertyBinding<Image>("Image");
        }

        /// <summary>
        /// A visual representation of the data in this View Model
        /// </summary>
        public ListBox View
        {
            get
            {
                return _view;
            }
        }

        protected virtual void AddToList(FileListItem datum)
        {
            _data.Add(datum);
        }

        protected virtual void ClearList()
        {
            _data.Clear();
        }
    }
}
