using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SystemMagazynowy.Models
{
    public class Warehouse
    {
        public int ID { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 3)]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        public virtual ICollection<Assortment> Assortment { get; set; }
    }
}