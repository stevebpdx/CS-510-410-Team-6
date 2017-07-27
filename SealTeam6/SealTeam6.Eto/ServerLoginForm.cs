using System;
using Eto.Forms;
using FluentFTP;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SealTeam6.EtoGUI
{
    class ServerLoginForm
    {
        protected CredentialsInput _credentials;
        protected LabelledTextInput _hostName;
        protected Button _submit;
        public event EventHandler<FtpClient> SubmitButtonPressed;
        public Control View { get; protected set; }
        
        public FtpClient StartSession()
        {
            return new FtpClient(_hostName.Text, _credentials.Credential);
        }

        protected void submit()
        {
            if(SubmitButtonPressed != null) SubmitButtonPressed.Invoke(this, StartSession());
        }

        public ServerLoginForm()
        {
            _submit = new Button((s,e) => submit());
            _submit.Text = "Login";
            _credentials = new CredentialsInput();
            _hostName = new LabelledTextInput("Hostname:");
            View = new StackLayout();
            var view = (StackLayout)View;
            view.Items.Add(_hostName.View);
            view.Items.Add(_credentials.View);
            view.Items.Add(_submit);
            view.Orientation = Orientation.Vertical;
        }
    }
}
