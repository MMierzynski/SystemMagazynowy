using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SystemMagazynowy.Models
{
    public class AssortmentWarehouse
    {
        public int ID { get; set; }
        public int WarehouseID { get; set; }
        public int AssortmentID { get; set; }
        [Display(Name ="Ilość")]
        [Range(minimum:0,maximum:10000)]
        public double Quantity { get; set; }

        public virtual Warehouse Warehouse{get; set;}
        public virtual Assortment Assortment { get; set; }
    }
}