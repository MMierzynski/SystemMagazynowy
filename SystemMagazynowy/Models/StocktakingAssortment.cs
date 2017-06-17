using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SystemMagazynowy.Models
{
    public class StocktakingAssortment
    {
        public int ID { get; set; }
        public int StocktakingID { get; set; }
        public int AssortmentID {get; set;}
        public double BeforeQuantity { get; set; }
        public double AfterQuantity { get; set; }

        public virtual Assortment Assortment { get; set; }
    }
}