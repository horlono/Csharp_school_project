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
using System.Windows.Shapes;
using PRBD_Framework;
using BankApp.Model;
namespace BankApp.View
{
    /// <summary>
    /// Interaction logic for ShowAccountsDialogView.xaml
    /// </summary>
    public partial class ShowAccountsDialogView : DialogWindowBase
    {
        public ShowAccountsDialogView(Account account)
        {
            
            InitializeComponent();
            vm.Init(account);
            
        }
        private void DialogWindowBase_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // pour ne pas permettre de fermer la fenêtre => obligé de cliquer sur un des 2 boutons
            if (vm.DialogResult == null)
                e.Cancel = true;
        }
    }
}
