using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRBD_Framework;
using BankApp.Model;
namespace BankApp.ViewModel
{
    public abstract class ViewModelCommon: ViewModelBase<User,BankContext>
    {
        public static bool IsManager => App.IsLoggedIn && App.CurrentUser is Manager;
        public static bool IsNotManager => !IsManager;
    }
}
