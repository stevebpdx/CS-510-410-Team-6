using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eto.Forms;

namespace SealTeam6.EtoGUI
{
    class LabelledTextInput
    {
        public string Label { get; set; }
        public string Text { get; set; }
        public Control View { get; protected set; }

        protected Label _label;
        protected TextControl _input;

        //This allows custom views to be added in subtypes
        protected LabelledTextInput()
        {

        }

        public LabelledTextInput(string label)
        {
            Label = label;
            _label = new Label();
            _label.TextBinding.Bind(this, "Label");
            _input = new TextBox();
            _input.TextBinding.Bind(this, "Text");
            View = new StackLayout();
            var view = (StackLayout)View;
            view.Items.Add(_label);
            view.Items.Add(_input);
            view.Orientation = Orientation.Horizontal;
        }
    }
}
