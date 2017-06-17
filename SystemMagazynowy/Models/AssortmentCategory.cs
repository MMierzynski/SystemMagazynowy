using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SystemMagazynowy.Models
{
    public class AssortmentCategory
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Kategoria")]
        [StringLength(30, ErrorMessage = "Nazwa kategorii musi mieć długość między 5 a 30 znaków", MinimumLength = 5)]
        public string Name { get; set; }

        public virtual ICollection<Assortment> Assortment { get; set; }
    }
}