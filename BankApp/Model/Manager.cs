using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Model
{
    public class Manager:User
    {
        public virtual ICollection<Agence> Agences { get; set; } = new HashSet<Agence>(); 
       
    }
}

