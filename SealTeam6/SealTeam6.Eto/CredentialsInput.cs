using Eto.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SealTeam6.EtoGUI
{
    class CredentialsInput
    {
        protected LabelledTextInput Username;
        protected PasswordField Password;
        public Control View { get; protected set; }
        public NetworkCredential Credential
        {
            get
            {
                return new NetworkCredential(Username.Text, Password.Text);
            }
            set
            {
                Username.Text = value.UserName;
                Password.Text = value.Password;
            }
        }

        public CredentialsInput()
        {
            Username = new LabelledTextInput("Username:");
            Password = new PasswordField();
            View = new StackLayout();
            var view = (StackLayout)View;
            view.Items.Add(Username.View);
            view.Items.Add(Password.View);
            view.Orientation = Orientation.Vertical;
        }
    }
}
