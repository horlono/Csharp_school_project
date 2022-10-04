using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankApp.Model
{
    public class Transfer
    {
        [Key]
        public int TransferId { get; set; }

        [Required , ForeignKey(nameof(AccountPayer))]
        public string IBANPayer { get; set; }
        public virtual  Account AccountPayer { get; set; }

        [Required , ForeignKey(nameof(AccountCreditor))]
        public string IBANCreditor { get; set; }

        public virtual Account AccountCreditor { get; set; }

        public string Descriptive { get; set; }
        public DateTime CreationTime { get; set; } 
        
        public DateTime EffectiveTime { get; set; }
        [NotMapped]
        public virtual Category Category { get; set; }
        public virtual User Payer { get; set; }
        public double Amount { get; set; }
        [NotMapped]
        public bool IsAccepted { get; set; }
        [NotMapped]
        public bool IsFuture { get; set; }
        [NotMapped]
        public bool Visibility { get; set; }

        public Transfer()
        {
                EffectiveTime = App.DateSystem;
        }

    }
}
