using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BankApp.ViewModel
{
    public class LoginViewModel: ViewModelCommon
    {
        public ICommand LoginCommand { get; set; }

        private string _pseudo;
        public string Pseudo
        {
            get => _pseudo;
            set => SetProperty(ref _pseudo, value, () => Validate());
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value, () => Validate());
        }

        public LoginViewModel() : base()
        {
            LoginCommand = new RelayCommand(LoginAction,
                () => { return _pseudo != null && _password != null && !HasErrors; });
        }

        private void LoginAction()
        {
            if (Validate())
            {
                var user = Context.Users.Find(Pseudo);
                NotifyColleagues(App.Messages.MSG_LOGIN, user);
            }
        }

        public override bool Validate()
        {
            ClearErrors();

            var user = Context.Users.Find(Pseudo);

            if (string.IsNullOrEmpty(Pseudo))
                AddError(nameof(Pseudo), "required");
            else if (Pseudo.Length < 3)
                AddError(nameof(Pseudo), "length must be >= 3");
            else if (user == null)
                AddError(nameof(Pseudo), "does not exist");
            else
            {
                if (string.IsNullOrEmpty(Password))
                    AddError(nameof(Password), "required");
                else if (Password.Length < 3)
                    AddError(nameof(Password), "length must be >= 3");
                else if (user != null && user.Password != Password)
                    AddError(nameof(Password), "wrong password");
            }

            return !HasErrors;
        }

        protected override void OnRefreshData()
        {
        }
    }
}
