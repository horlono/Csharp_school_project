using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRBD_Framework;

namespace BankApp.Model
{
    public abstract class Account : EntityBase<BankContext>
    {
        [Key]
        public string IBAN { get; set; }
        public double Floor { get; set; }
        [NotMapped]
        public double Balance { get; set; }

        public Account()
        {
            if (IBAN == null)
                IBAN = Model.IBAN.Random();
            Balance = 0;

        }
        public string Descriptive { get; set; }
        public virtual ICollection<Transfer> TransfersIn { get; set; } = new HashSet<Transfer>();
        public virtual ICollection<Transfer> TransfersOut { get; set; } = new HashSet<Transfer>();
        
    }
}
        

    

