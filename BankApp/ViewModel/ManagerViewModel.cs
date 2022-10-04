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
    public class ManagerViewModel : ViewModelCommon
    {
        public ManagerViewModel()
        {
            OnRefreshData();
            RaisePropertyChanged();
            Delete = new RelayCommand(() => DeleteClient());
            Cancel = new RelayCommand(() => CancelAction());
        }
        public ICommand Delete { get; set; }
        public ICommand Cancel { get; set; }
        protected Agence _selectedagence;
        protected ObservableCollectionFast<Agence> _agencies = new();
        protected ObservableCollectionFast<Client> _clients = new();
        protected Client _selectedclient;
        public Client SelectedClient
        {
            get => _selectedclient;
            set => SetProperty(ref _selectedclient, value);
        }
        public ObservableCollectionFast<Client> Clients
        {
            get => _clients;
            set => SetProperty(ref _clients, value);
        }
        public ObservableCollectionFast<Agence> Agencies
        {
            get => _agencies;
            set => SetProperty(ref _agencies, value);
        }
        public Agence SelectedAgence
        {
            get => _selectedagence;
            set => SetProperty(ref _selectedagence, value,LoadClients);
        }
        public void LoadClients()
        {
            var clients = from ac in Context.ClientAgencies
                          where ac.Agence.Name.Contains(SelectedAgence.Name)
                          select ac.Client;
            
            Clients = new ObservableCollectionFast<Client>(clients);
        }
        public void DeleteClient()
        {
            Context.Remove(SelectedClient);
            Context.SaveChanges();
            LoadClients();

        }
        public override void CancelAction()
        {
            ClearErrors();
            LoadClients();
        }
        protected override void OnRefreshData()
        {

            var agencies = from a in Context.Agences
                           where a.Manager == CurrentUser
                           select a;
           

            Agencies = new ObservableCollectionFast<Agence>(agencies);
        }
    }
}
