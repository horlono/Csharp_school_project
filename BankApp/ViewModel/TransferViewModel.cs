using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BankApp.Model;
using PRBD_Framework;

namespace BankApp.ViewModel
{
    public class TransferViewModel : ViewModelCommon
    {
        public void Init(Transfer transfer)
        {
            Payer = transfer.AccountPayer;
            RaisePropertyChanged();
            OnRefreshData();
        }
        public TransferViewModel()  {
            DoTransfer = new RelayCommand(AddTransfer,CanTransfer);
            Cancel = new RelayCommand(CancelAction);
            Register(App.Messages.MSG_DATE_CHANGED, () => OnRefreshData());
            ShowAccounts = new RelayCommand<Account>(thisaccount => {
                Account account = (Account)App.ShowDialog<ShowAccountsDialogViewModel, User, BankContext>(thisaccount);
                SelectedAccount = account;
                
            });
        }

        private ObservableCollectionFast<Category> _categories = new();
        public ICommand DoTransfer { get; set; }  
        private IntAccount _selectfromaccount;
        private Category _selectedCategory;
        public ICommand ShowAccounts { get; set; }
        public ICommand Cancel { get; set; }
        private Account _payer;
        private DateTime _dateTime;
        private double _amount;
        private string _desciptive;
        private ObservableCollectionFast<Account> _accounts;
        private ObservableCollectionFast<Account> _intaccounts;
        private int _index;
        public ObservableCollectionFast<Category> Categories
        {
            get => _categories;
            set => SetProperty(ref _categories, value);
        }
        public Category SelectedCategory
        {
            get => _selectedCategory;
            set => SetProperty(ref _selectedCategory, value);
        }
        public int Index
        {
            get => _index;
            set => SetProperty(ref _index, value);
        }
        public String NewDescriptive
        {
            get => _desciptive;
            set => SetProperty(ref _desciptive, value);
        }
        public ObservableCollectionFast<Account> Accounts
        {
            get => _accounts;
            set => SetProperty(ref _accounts, value);

        }
        public ObservableCollectionFast<Account> IntAccounts
        {
            get => _intaccounts;
            set => SetProperty(ref _intaccounts, value);

        }
        public DateTime EffectiveDate
        {
            get => _dateTime;
            set => SetProperty(ref _dateTime, value);
        }
        public double AmountNewTransfer
        {
            get => _amount;
            set => SetProperty(ref _amount, value);
        }
       
        public void AddTransfer()
        {
            
            var addTransfer = new Transfer
            {
                AccountPayer = SelectFromAccount,
                AccountCreditor = SelectedAccount,
                Amount = AmountNewTransfer,
                Descriptive = NewDescriptive,
                EffectiveTime = EffectiveDate,
                Category=SelectedCategory


            };
            

            Context.Add(addTransfer);
            Context.SaveChanges();
            NotifyColleagues(App.Messages.MSG_CLOSE_TRANSFER, Payer);

        }
        private bool CanTransfer()
        {
            
            ClearErrors();
            if (Payer?.Balance - AmountNewTransfer < Payer?.Floor && EffectiveDate.Date == App.DateSystem.Date)
                AddError(nameof(AmountNewTransfer), "is out");
            else if (EffectiveDate.Date < App.DateSystem.Date)
                AddError(nameof(EffectiveDate), "is not anterior to now");
            else if (SelectedAccount == null)
                AddError(nameof(SelectedAccount.Descriptive), "not selected");
            else if (NewDescriptive == null)
                AddError(nameof(NewDescriptive), "not empty");
            
            return !HasErrors;
           
        }
        public override void CancelAction()
        {
            ClearErrors();
            NotifyColleagues(App.Messages.MSG_CLOSE_TRANSFER, Payer);
        }
        public Account Payer
        
        {
            get => _payer;
            set => SetProperty(ref _payer, value);
        }

        private Account _selectedaccount;
        public Account SelectedAccount
        {
            get => _selectedaccount;
            set => SetProperty(ref _selectedaccount, value);
            
        }
        public IntAccount SelectFromAccount
        {
            get => _selectfromaccount;
            set => SetProperty(ref _selectfromaccount, value);
        }
        protected override void OnRefreshData()
        {
            var access = from e in Context.AccessClients
                         where e.Pseudo.Contains(CurrentUser.Pseudo)
                         select e.IntAccount;

            var intaccounts = from a in access
                           where a.CreationTime.Date <= App.DateSystem.Date
                           select a;

            IntAccounts = new ObservableCollectionFast<Account>(intaccounts);
            var accounts = from a in Context.Accounts
                           select a ;
            var categories = from c in Context.Categories
                             select c;
            Index = IntAccounts.IndexOf(Payer);   
            Accounts = new ObservableCollectionFast<Account>(accounts.Where(f => f.IBAN !=Payer.IBAN));            
            EffectiveDate = App.DateSystem;
            Categories = new ObservableCollectionFast<Category>(categories.OrderBy(c => c.Name));
        }
    }
}
