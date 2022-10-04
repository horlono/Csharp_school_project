using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BankApp.Model;
using PRBD_Framework;
using System.ComponentModel;
namespace BankApp.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// test commit
    public partial class MainView : WindowBase
    {
        public MainView()
        {
            InitializeComponent();
            Register<IntAccount>(App.Messages.MSG_DISPLAY_ACCOUNT,
                intaccount => DoDisplayAccount(intaccount));
            Register<Transfer>(App.Messages.MSG_NEW_TRANSFER,
                transfer => DoDisplayTransfer(transfer));
            Register<IntAccount>(App.Messages.MSG_CLOSE_TRANSFER, account => DoCloseTab(account));
            

        }

        private void DoDisplayAccount(IntAccount intaccount)
        {
            OpenTab(intaccount.IBAN, intaccount.IBAN, () => new UserAccountView(intaccount));
        }
        private void DoDisplayTransfer(Transfer transfer)
        {
            OpenTab($"New Transfer '{transfer.AccountPayer.Descriptive}'", transfer.AccountPayer.IBAN, () => new TransferView(transfer));
        }
        private void OpenTab(string header,string tag,Func<UserControlBase> createView)
        {
            var tab = tabControl.FindByTag(tag);
            if (tab == null)
                tabControl.Add(createView(), header, tag);
            else
                tabControl.SetFocus(tab);
        }
        private void MenuLogout_Click(object sender,System.Windows.RoutedEventArgs e)
        {
            NotifyColleagues(App.Messages.MSG_LOGOUT);
        }
        private void WindowsBase_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Q && Keyboard.IsKeyDown(Key.LeftCtrl))
                Close();
        }
        //tuto6 logout
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            tabControl.Dispose();
        }
        private void DoCloseTab(IntAccount intaccount)
        {
            tabControl.CloseByTag(intaccount.IBAN);
        }
        private void AccountsView_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
