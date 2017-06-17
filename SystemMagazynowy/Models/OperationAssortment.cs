using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SystemMagazynowy.Models
{
    public class OperationAssortment
    {
        public int ID { get; set; }

        [Required]
        public int OperationID { get; set; }

        [Required]
        public int AssortmentID { get; set; }

        [Required]
        [Display(Name ="Ilość")]
        public double Quantity { get; set; }

        public virtual Operation Operation {get; set;}
        public virtual Assortment Assortment { get; set; }
    }
}