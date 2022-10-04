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
    public class ClientAgencies : EntityBase<BankContext>
    {
        
        public string Agency { get; set; }
        [Required,ForeignKey(nameof(Agency))]
        public virtual Agence Agence { get; set; }
        
        public string Pseudo { get; set; }
        [Required,ForeignKey(nameof(Pseudo))]
        public virtual Client Client { get; set; }

    }
}
