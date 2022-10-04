using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRBD_Framework;
using BankApp.Model;
using System.Windows.Input;

namespace BankApp.ViewModel
{
    public class ShowAccountsDialogViewModel :  DialogViewModelBase<User,BankContext>
    {
        public void Init(Account account)
        {
            ThisAccount = account;
            RaisePropertyChanged();
            OnRefreshData();
            
        }
        public ICommand Ok { get; set; }
        public ICommand Cancel { get; set; }
        private Account _thisaccount;
        private ObservableCollectionFast<Account> _accounts;
        private ObservableCollectionFast<Account> _intaccounts;
        private Account _selectedaccount;
        public Account SelectedAccount
        {
            get => _selectedaccount;
            set => SetProperty(ref _selectedaccount, value);
        }
        public ObservableCollectionFast<Account> IntAccounts
        {
            get => _intaccounts;
            set => SetProperty(ref _intaccounts, value);

        }
        public ObservableCollectionFast<Account> Accounts
        {
            get => _accounts;
            set => SetProperty(ref _accounts, value);

        }
        public Account ThisAccount
        {
            get => _thisaccount;
            set => SetProperty(ref _thisaccount, value);
        }
        protected override void OnRefreshData()
        {
            
            var access = from e in Context.AccessClients
                         where e.Pseudo.Contains(CurrentUser.Pseudo)
                         select e.IntAccount;

            var intaccounts = from a in access
                              where a.CreationTime.Date <= App.DateSystem.Date
                              select a;
            var allaccounts = from a in Context.Accounts
                              where intaccounts.Any(f => f.IBAN != a.IBAN)
                              select a;
            
            IntAccounts = new ObservableCollectionFast<Account>(intaccounts.Where(f => f.IBAN != ThisAccount.IBAN));
            Accounts = new ObservableCollectionFast<Account>(allaccounts);
            
           

        }
        public ShowAccountsDialogViewModel()
        {
            
            Ok = new RelayCommand<Account>(account => DialogResult=account);
            Cancel = new RelayCommand<Account>(account => DialogResult=null);
        }
        
            
    }
   
}
