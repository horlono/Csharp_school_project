using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApp.Model

{
    public class Client:User
    {
       
        public virtual ICollection<AccessClient> AccessClients { get; set; } = new HashSet<AccessClient>();
        public virtual ClientAgencies ClientAgencies { get; set; }
    }
}

