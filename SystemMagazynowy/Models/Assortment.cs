using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SystemMagazynowy.Models
{
    public class Assortment
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Kod kreskowy")]
        public string BarCode { get; set; }
       
        //[Required]
        //[Display(Name = "Ilość")]
        //public double Quantity { get; set; }

        [Required]
        [StringLength(10,MinimumLength=1)]
        [Display(Name = "Jednostka miary")]
        public string Unit { get; set; }

        [Required]
        [Display(Name = "Stan minimalny")]
        public double MinimumQuantity { get; set; }

        [Required]
        [Display(Name = "Stan maksymalny")]
        public double MaximumQuantity { get; set; }

        [Required]
        [Display(Name = "Stan początkowy")]
        public double InitialQuantity { get; set; }

        public int ContractorID { get; set; }
        public int AssortmentCategoryID { get; set; }

        public virtual AssortmentCategory Category { get; set; }
        public virtual ICollection<Warehouse> Warehouse { get; set; }
        public virtual Contractor Contractor { get; set; }
        public virtual ICollection<AssortmentWarehouse> AssortmentInWarehouse { get; set; }
        public virtual ICollection<OperationAssortment> OperationOnAssortment { get; set; }
    }
}