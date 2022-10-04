using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRBD_Framework;

namespace BankApp.Model
{
   public class Agence : EntityBase<BankContext>
    {
        [Key]
        public string Name { get; set; }
        [Required]
        
        public int NrAgence { get; set; }
        //public int NbUser { get; set; }

        [Required]
        public virtual Manager Manager { get; set; }

        public virtual ICollection<ClientAgencies> ListClients { get; set; } = new HashSet<ClientAgencies>();


    }
}
