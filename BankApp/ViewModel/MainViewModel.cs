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

    public class MainViewModel : ViewModelCommon
    {
        
        public ICommand ReloadDataCommand { get; set; }
        
        private DateTime _datesystem=new DateTime(2022,01,12);
        public void refreshDate()
        {
            App.DateSystem = DateSystem;
            NotifyColleagues(App.Messages.MSG_DATE_CHANGED);
        }
        public DateTime DateSystem
        {
            get => _datesystem;
            set => SetProperty(ref _datesystem, value, ()=>refreshDate());
        }
        public MainViewModel()
        {
            
            App.DateSystem = DateSystem;
           
                

           

        }
       
        protected override void OnRefreshData()
        {
           
            
            
          
        }
    }
}
