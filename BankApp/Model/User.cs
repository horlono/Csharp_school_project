using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PRBD_Framework;

namespace BankApp.Model
{
    public enum Role
    {
        Client = 1,
        Manager = 2,
        Admin = 3



    }
    // on met public abstract car elle ne sera jamais instancier 
    public abstract class User : EntityBase<BankContext>
    {
       

        [Key]
        public string Pseudo { get; set; }
        
        [Required]
        public int NbUser { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string FirstName { get; set; }

        //chaque new User aura la valeur 1 par defaut 
        public Role Role { get; protected set; }
        public virtual ICollection<IntAccount> IntAccounts { get; set; } = new HashSet<IntAccount>();

    }
}
