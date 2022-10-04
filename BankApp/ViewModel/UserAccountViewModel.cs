using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRBD_Framework;
using BankApp.Model;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Collections;
using System.Windows;

namespace BankApp.ViewModel
{
    public class UserAccountViewModel : ViewModelCommon
    {

        public UserAccountViewModel() : base() {
            ClearFilter = new RelayCommand(() => Filter = "");
            Register(App.Messages.MSG_DATE_CHANGED, () => OnRefreshData());
            Cancel = new RelayCommand<Transfer>(transfer => {
                Context.Remove(transfer);
                Context.SaveChanges();
                OnRefreshData();
            });

            FilterCategory = new RelayCommand<Category>(category => {

                if (category.IsSelected)
                    SelectedCategory.Add(category);
                else
                    SelectedCategory.Remove(category);

                FilterListCategory();

            });
            CheckNone = new RelayCommand<Category>(f => {
                SelectedCategory.Clear();
                foreach (var categorie in CheckBoxCategory)
                {
                    categorie.IsSelected = false;
                    SelectedCategory.Add(categorie);
                }
                IsNoCategory = false;
                Categories = new ObservableCollectionFast<Category>(CheckBoxCategory);
                FilterListCategory();
            });
            CheckAll = new RelayCommand<Category>(f => {

                SelectedCategory.Clear();
                foreach (var categorie in CheckBoxCategory) {
                    categorie.IsSelected = true;
                    SelectedCategory.Add(categorie);
                }
                IsNoCategory = true;
                Categories = new ObservableCollectionFast<Category>(CheckBoxCategory);
                FilterListCategory();
            });

            PastTransactions = new RelayCommand(() => { FilterListCategory(); });
            FutureTransaction = new RelayCommand(() => { FilterListCategory(); });
            First = new RelayCommand(() => {
                CurrentPage = 0;
                FilterListCategory();
            }, () => CurrentPage != 0);
            Last = new RelayCommand(() => {
                CurrentPage = TotalPage - 1;
                FilterListCategory();

            }, () => CurrentPage != TotalPage - 1);
            Previous = new RelayCommand(() => {
                CurrentPage--;
                FilterListCategory();
            }, () => CurrentPage > 0);
            Next = new RelayCommand(() => {
                CurrentPage++;
                FilterListCategory();
            }, () => CurrentPage < TotalPage - 1);
        }
        private IntAccount _intAccount;
        private ObservableCollection<Transfer> _transfers;

        private ObservableCollectionFast<Category> _categories = new();
        private ObservableCollectionFast<Category> _checkBoxCategory = new();
        private ObservableCollectionFast<Category> _selectedCategory = new();
        private Category _selectedcombocategory;
        private bool _isrefused = false;
        private bool _ischecked = false;
        private bool _ischeckall = false;
        private bool _isnocategory = false;
        private bool _ispast = true;
        private bool _isenableall = true;
        private bool _isfuture = false;
        private string _filter;
        private double _currentBalance;
        private DateTime _dateTimeBegin;
        private DateTime _dateTimeEnd;
        private string _period;
        private IList _selectedItems = new ArrayList();
        private int _currentPage = 0;
        private int _itemsPerPage = 2;
        private int _totalPage = 0;
        public Category SelectedComboCategory
        {
            get => _selectedcombocategory;
            set => SetProperty(ref _selectedcombocategory,value);
        }
        public int CurrentPage
        {
            get => _currentPage;
            set => SetProperty(ref _currentPage, value);
        }
        public int ItemsPerPage
        {
            get => _itemsPerPage;
            set => SetProperty(ref _itemsPerPage, value);
        }
        public int TotalPage
        {
            get => _totalPage;
            set => SetProperty(ref _totalPage, value);
        }
        public IList SelectedItems
        {
            get => _selectedItems;
            set => SetProperty(ref _selectedItems, value);
        }
        public string Period
        {
            get => _period;
            set => SetProperty(ref _period, value, FilterListCategory);
        }
        public double CurrentBalance
        {
            get => _currentBalance;
            set => SetProperty(ref _currentBalance, value);
        }
        public ICommand FilterCategory { get; set; }
        public ICommand Cancel { get; set; }
        public ICommand CheckNone { get; set; }
        public ICommand CheckAll { get; set; }
        public ICommand NoCategory { get; set; }
        public ICommand ClearFilter { get; set; }
        public ICommand PastTransactions { get; set; }
        public ICommand FutureTransaction { get; set; }
        public ICommand Next { get; set; }
        public ICommand Previous { get; set; }
        public ICommand First { get; set; }
        public ICommand Last { get; set; }
       
        public ObservableCollection<Transfer> Transfers {
            get => _transfers;
            set => SetProperty(ref _transfers, value);
        }
        public bool IsRefused
        {
            get => _isrefused;
            set => SetProperty(ref _isrefused, value,FilterListCategory);
        }
       
        public IntAccount IntAccount { 
            get => _intAccount;  
            set=> SetProperty(ref _intAccount, value); 
        }
        public ObservableCollectionFast<Category> SelectedCategory
        {
            get => _selectedCategory;
            set => SetProperty(ref _selectedCategory, value);
        }
        public ObservableCollectionFast<Category> CheckBoxCategory
        {
            get => _checkBoxCategory;
            set => SetProperty(ref _checkBoxCategory, value);

        }
        public ObservableCollectionFast<Category> Categories
        {
            get => _categories;
            set => SetProperty(ref _categories, value);
        }
        public bool IsChecked
        {
            get => _ischecked;
            set => SetProperty(ref _ischecked, value);
        }
        public bool IsCheckAll
        {
            get => _ischeckall;
            set => SetProperty(ref _ischeckall, value);
        }
        public bool IsNoCategory
        {
            get => _isnocategory;
            set => SetProperty(ref _isnocategory, value, FilterListCategory);
        }
        public bool IsPast
        {
            get => _ispast;
            set => SetProperty(ref _ispast, value);
        }
        public bool IsEnableAll
        {
            get => _isenableall;
            set => SetProperty(ref _isenableall, value);
        }
        public bool IsFutureTransaction
        {
            get => _isfuture;
            set => SetProperty(ref _isfuture, value);
        }
        public  string Filter
        {
            get => _filter;
            set => SetProperty(ref _filter, value,FilterListCategory);
        }
        public DateTime DateTimeBegin
        {
            get => _dateTimeBegin;
            set => SetProperty(ref _dateTimeBegin, value);
        }
        public DateTime DateTimeEnd
        {
            get => _dateTimeEnd;
            set => SetProperty(ref _dateTimeEnd, value);
        }
        
        private void FilterListCategory()
        {
            var transfers = IntAccount.GetAllTransfers(IntAccount,Filter,DateTimeBegin,DateTimeEnd);
            ObservableCollection<Transfer> PagingTransfers = new();
            
            foreach (var transfer in transfers)
            {
                
                if (IsPast)
                {
                    
                    if (transfer.EffectiveTime < App.DateSystem)
                        transfer.Visibility = true;
                }
                if (Period == "One Week")
                {

                    if (transfer.EffectiveTime >= App.DateSystem.AddDays(-7)&& transfer.EffectiveTime < App.DateSystem)
                    {
                        transfer.Visibility = true;

                    }
                    else
                        transfer.Visibility = false;
                    
                }
                if (Period == "One Day")
                {
                    if(transfer.EffectiveTime >= App.DateSystem.AddDays(-1)&& transfer.EffectiveTime < App.DateSystem)
                    {
                        transfer.Visibility = true;
                    }
                    else
                        transfer.Visibility = false;

                }
                if (Period == "Two Weeks")
                {
                    if (transfer.EffectiveTime >= App.DateSystem.AddDays(-14)&& transfer.EffectiveTime < App.DateSystem)
                        transfer.Visibility = true;
                    else
                        transfer.Visibility = false;
                }
                if (Period == "One Month")
                {
                    if (transfer.EffectiveTime >= App.DateSystem.AddMonths(-1)&& transfer.EffectiveTime < App.DateSystem)
                        transfer.Visibility = true;
                    else
                        transfer.Visibility = false;
                }
                if (Period == "One Year")
                {
                    if (transfer.EffectiveTime >= App.DateSystem.AddYears(-1)&& transfer.EffectiveTime < App.DateSystem)
                        transfer.Visibility = true;
                    else
                        transfer.Visibility = false;
                }
                if (Period == "All")
                {
                    transfer.Visibility = true;
                }
                if (IsRefused)
                {
                    if (!transfer.IsAccepted) {
                        if (transfer.IBANPayer == IntAccount.IBAN)
                            transfer.Visibility = true;
                        else
                            transfer.Visibility = false;
                    }
                        
                    
                }
                if (!IsRefused)
                {
                    if (!transfer.IsAccepted)
                        transfer.Visibility = false;
                }
                if (IsFutureTransaction)
                {
                    
                    if (transfer.IsFuture)
                    transfer.Visibility = true;
                }
                if (!IsFutureTransaction)
                {
                    if (transfer.IsFuture)
                        transfer.Visibility = false;
                }

            }
            
           
            if (SelectedCategory.Count() > 0 && !IsNoCategory) {
                var filtered = from t in transfers
                               where SelectedCategory.Any(f => f == t.Category)
                               
                               select t;
                PagingTransfers = new ObservableCollection<Transfer>(filtered.AsEnumerable().Where(t => t.Visibility));
            }
            if (SelectedCategory.Count() > 0 && IsNoCategory)
            {
                var filtered = from t in transfers
                               where SelectedCategory.Any(f => f == t.Category)
                               ||(t.Category==null)
                               select t;
                PagingTransfers = new ObservableCollection<Transfer>(filtered.AsEnumerable().Where(t => t.Visibility));
            }
            if (SelectedCategory.Count() == 0 && !IsNoCategory)
            {
                
                PagingTransfers = new ObservableCollection<Transfer>(transfers.AsEnumerable().Where(t => t.Visibility));
                
            }
            if (SelectedCategory.Count() == 0 && IsNoCategory)
            {
                var filtered = from t in transfers
                               where (t.Category == null)
                               select t;
                PagingTransfers = new ObservableCollection<Transfer>(filtered.AsEnumerable().Where(t => t.Visibility));

            }
            int itemcount = PagingTransfers.Count();
            TotalPage = itemcount / ItemsPerPage;
            if (itemcount % ItemsPerPage != 0) { TotalPage += 1; }
            Pagination(PagingTransfers);

        }
        public  void   Pagination(ObservableCollection<Transfer> transfers)
        {

            Transfers = new();
            foreach (var transfer in transfers)
            {
                int index = transfers.IndexOf(transfer);
                if (index >= ItemsPerPage * CurrentPage && index < ItemsPerPage * (CurrentPage + 1))
                    Transfers.Add(transfer);
            }
            
        }
       
        public void Init(IntAccount intAccount){
            IntAccount = intAccount;
            Filter = "";
            SelectedItems.Add("One Day");
            SelectedItems.Add("One Week");
            SelectedItems.Add("Two Weeks");
            SelectedItems.Add("One Month");
            SelectedItems.Add("One Year");
            SelectedItems.Add("All");
            RaisePropertyChanged();
            OnRefreshData();
           
        }
        
        protected override void OnRefreshData()
        {
            
            var querytransfers = from t in Context.Transfers
                            orderby t.EffectiveTime
                            select t;
            ObservableCollectionFast<Transfer> allTransfer = new(querytransfers);
            
            IntAccount.getBalance(allTransfer);
            CurrentBalance = IntAccount.Balance;
            FilterListCategory();
            
            var categories = from c in Context.Categories
                             select c;
            CheckBoxCategory = new ObservableCollectionFast<Category>(categories.OrderBy(c => c.Name));
            Categories = new ObservableCollectionFast<Category>(categories.OrderBy(c => c.Name));
            

        

            
        }
    }
}
