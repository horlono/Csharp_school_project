using BankApp.Model;
using PRBD_Framework;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace BankApp.ViewModel
{
    public class AccountsViewModel : ViewModelCommon
    {
        //tuto 4 User controls raiseproperty changed

        private ObservableCollection<IntAccount> _Intaccounts;
        private ObservableCollectionFast<IntAccount> _accounts ;
        private ObservableCollectionFast<Transfer> _rejectedtransfers = new();
        public ICommand ClearFilter { get; set; }
        public ICommand DisplayAccount { get; set; }
        public ICommand NewTransfer { get; set; }
        public ICommand AccountFilter { get; set; }
        public ICommand SavingAccount { get; set; }
        
        private double _currentBalance;
        private bool _isSaving;
        private bool _isChecking;
        private bool _isAll;
        private string _filter;
    
        public string Filter
        {
            get => _filter;
            set => SetProperty(ref _filter, value, FilterAccount);
        }
        public double CurrentBalance
        {
            get => _currentBalance;
            set => SetProperty(ref _currentBalance, value);
        }
        private ObservableCollectionFast<IntAccount> _selectedAccounts = new();
        public bool IsAll
        {
            get => _isAll;
            set => SetProperty(ref _isAll, value);
        }
        public bool IsSaving
        {
            get => _isSaving;
            set => SetProperty(ref _isSaving, value);
        }
        public bool IsChecking
        {
            get => _isChecking;
            set => SetProperty(ref _isChecking, value);
        }
        public ObservableCollectionFast<IntAccount> Accounts
        {
            get => _accounts;
            set => SetProperty(ref _accounts, value);
        }
        public ObservableCollection<IntAccount> IntAccounts
        {
            get => _Intaccounts;
            set => SetProperty(ref _Intaccounts, value);
        }
        public ObservableCollectionFast<Transfer> RejectedTransfers
        {
            get => _rejectedtransfers;
            set => SetProperty(ref _rejectedtransfers, value);
        }
        public ObservableCollectionFast<IntAccount> SelectedAccounts
        {
            get => _selectedAccounts;
            set => SetProperty(ref _selectedAccounts, value);
        }
        private void FilterAccount()
        {
            var access = from e in Context.AccessClients
                         where e.Pseudo.Contains(CurrentUser.Pseudo)
                         select e.IntAccount;
            var accounts = from a in access
                           where (a.CreationTime.Date <= App.DateSystem.Date)&&(a.IBAN.Contains(Filter)||a.Descriptive.Contains(Filter))
                           select a;
            if (IsChecking)
            {
                
                SelectedAccounts.Clear();
                foreach(var a in accounts)
                {
                    if (a is CheckingAccount )
                        
                        SelectedAccounts.Add(a);
                    

                }
                
            }
            else if(IsSaving)
            {
                
                SelectedAccounts.Clear();
                foreach (var a in accounts)
                {
                    if (a is SavingAccount)
                        
                    SelectedAccounts.Add(a);


                }
            }
            else
            {
                SelectedAccounts.Clear();
                foreach (var a in accounts)
                    SelectedAccounts.Add(a);
            }
            
            IntAccounts = new ObservableCollection<IntAccount>(SelectedAccounts);
        }
       
        public AccountsViewModel()
        {
            Filter = "";
            OnRefreshData();
            ClearFilter = new RelayCommand(() => Filter = "");
            AccountFilter = new RelayCommand<IntAccount>(f =>
            {
                FilterAccount();
            });
            DisplayAccount = new RelayCommand<IntAccount>(intAccount => NotifyColleagues(App.Messages.MSG_DISPLAY_ACCOUNT, intAccount));
            NewTransfer = new RelayCommand<Account>(intAccount =>
            NotifyColleagues(App.Messages.MSG_NEW_TRANSFER, new Transfer { AccountPayer = intAccount }));
            Register(App.Messages.MSG_DATE_CHANGED, () => OnRefreshData());
            
            
        }
        

        protected override void OnRefreshData()
        {
            
            
            var allAccounts = from a in Context.IntAccounts
                              select a;
            var access = from e in Context.AccessClients
                         where e.Pseudo.Contains(CurrentUser.Pseudo)
                         select e.IntAccount;
            var accounts = from a in access
                           where a.CreationTime.Date <= App.DateSystem.Date
                           select a;
            var transfers = from t in Context.Transfers
                            orderby t.EffectiveTime
                            select t;
            Accounts = new ObservableCollectionFast<IntAccount>(allAccounts);
            ObservableCollectionFast<Transfer> allTransfer = new(transfers);

            foreach (var a in accounts)
            {
                
                a.getBalance(allTransfer);
                
                
            }


            IntAccounts = new ObservableCollection<IntAccount>(accounts);
            
            
            
        }
    }
}
