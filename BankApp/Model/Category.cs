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
    public  class Category : EntityBase<BankContext>   
    {
        [Key]
        public int CategoryId { get; set; }
        [Required]
        public string Name { get; set; }
        [NotMapped]
        public bool IsSelected { get; set; }
        public virtual ICollection<Transfer> Transfers { get; set; } = new HashSet<Transfer>();
    }
}
