using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using BankApp.Model;
using PRBD_Framework;
using Microsoft.EntityFrameworkCore;
using BankApp.ViewModel;

namespace BankApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : ApplicationBase<User,BankContext>
    {
        public enum Messages
        {
            MSG_DISPLAY_ACCOUNT,
            MSG_NEW_TRANSFER,
            MSG_REFRESH_TRANSFERS,
            MSG_DATE_CHANGED,    
            MSG_LOGIN,
            MSG_LOGOUT,
            MSG_CLOSE_TRANSFER
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            
         
            base.OnStartup(e);
            App.DateSystem= new DateTime(2022, 01, 12);
            Context.Database.EnsureDeleted();
            Context.Database.EnsureCreated();
            Context.SeedData();
            
            Register<User>(this, Messages.MSG_LOGIN, user =>
             {
                 Login(user);

                 NavigateTo<MainViewModel, User, BankContext>();
             });
            Register(this, Messages.MSG_LOGOUT,()=>{
                Logout();
                NavigateTo<LoginViewModel, User, BankContext>();
            });

            
            
        }
        public static DateTime DateSystem{get;set;}

     
    }
}
 