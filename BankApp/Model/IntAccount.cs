using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRBD_Framework;

namespace BankApp.Model
{
    public class IntAccount : Account

    {
        [Required]
        public virtual ICollection<AccessClient> AccessClients { get; set; } = new HashSet<AccessClient>();
        public DateTime CreationTime { get; set; }
        
        
     
        
       
        public IQueryable<Transfer> GetAllTransfers(IntAccount observer,string filter,DateTime begin,DateTime end)
        {
            
            var transfers = Context.Transfers.Where(t => (t.IBANCreditor == observer.IBAN
            || t.IBANPayer == observer.IBAN)
            && (t.IBANPayer.Contains(filter)
            || t.IBANCreditor.Contains(filter)
            || t.Descriptive.Contains(filter)
            || t.Amount.ToString().Contains(filter)
            || t.AccountPayer.Descriptive.Contains(filter)
            || t.AccountCreditor.Descriptive.Contains(filter))
            
                )
            .OrderByDescending(t => t.EffectiveTime);
            return transfers;
        }
        
     
        public void getBalance(ObservableCollectionFast<Transfer> transfers)
        {
            
            foreach (var t in transfers)
            {
                
                t.AccountCreditor.Balance = 0;
                t.AccountPayer.Balance = 0;
            }
            foreach (var t in transfers)
            {
               
                if (t.EffectiveTime <= App.DateSystem)
                {

                    if (t.AccountPayer.Balance - t.Amount >= t.AccountPayer.Floor)
                    {

                        t.AccountPayer.Balance -= t.Amount;
                        t.AccountCreditor.Balance += t.Amount;
                        t.IsAccepted = true;
                        t.IsFuture = false;
                    }
                    else {
                        t.IsAccepted = false;
                        t.IsFuture = false;
                    }
                }
                        
                else {
                    
                    t.IsFuture = true; }
                    


            }
        }
      

        public IntAccount()
        {
           
           
        }
       



    }
}
