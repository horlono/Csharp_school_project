using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRBD_Framework;

namespace BankApp.Model
{
    public enum Access
    {
        Holder=1,
        Agent=2
    }
    public class AccessClient : EntityBase<BankContext>
    {
        public string Iban { get; set; }
        [Required]
        public virtual IntAccount IntAccount { get; set; }
        public string Pseudo { get; set; }
        [Required]
        public virtual Client Client { get; set; }
        [Required]
        
        public Access Access { get;  set; }
         


    }
}
