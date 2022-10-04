using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRBD_Framework;

namespace BankApp.Model
{
    public class CheckingAccount : IntAccount
    {
        
     public CheckingAccount()
        {
            if(Floor == null)
            {
                Floor = 0;
            }
        }
    }
}
