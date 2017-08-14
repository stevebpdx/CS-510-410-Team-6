using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SealTeam6.EtoGUI
{
    class PasswordField : LabelledTextInput
    {
        public PasswordField(string label): base()
        {
            Label = label;
            _label = new Eto.Forms.Label();
            _label.TextBinding.Bind(this, "Label");
            _input = new Eto.Forms.PasswordBox();
            _input.TextBinding.Bind(this, "Text");
            base.View = new Eto.Forms.StackLayout();
            var view = (Eto.Forms.StackLayout)View;
            view.Orientation = Eto.Forms.Orientation.Horizontal;
            view.Items.Add(_label);
            view.Items.Add(_input);
        }

        public PasswordField(): this("Password:")
        {
        }
    }
}
