using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using SystemMagazynowy.Models;

namespace SystemMagazynowy.ViewModel
{

    public class AssortmentViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string BarCode { get; set; }
        public double Quantity { get; set; }
    }

    public class StocktakingAssortmentViewModel
    {
        public int ID { get; set; }
        public int AssortmentID { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public string WarehouseName { get; set; }
        public string Unit { get; set; }
        public double BeforeQuantity { get; set; }
        public double AfterQuantity { get; set; }
    }

    public class Assortment4ExcelViewModel
    {
        

        public string Name { get; set; }

        public string Barcode { get; set; }

        public string Unit { get; set; }

        public double BeforeQuantity { get; set; }

        public double AfterQuantity { get; set; }
        public double Difference { get; set; }
    }

    public class AssortmentDetailsViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string BarCode { get; set; }
        public string Unit { get; set; }
        public double MinQuantity { get; set; }
        public double MaxQuantity { get; set; }
        public string CategoryName { get; set; }
        public string ContractorName { get; set; }
        public int ContractorID { get; set; }

        public ICollection<ShortAssortmentInWarehouse> Replanishment { get; set; }
        public ICollection<ShortOperationViewModel> Operations { get; set; }

    }

    public class ShortAssortmentInWarehouse
    {
        public string WarehouseName { get; set; }
        public double Quantity { get; set; }
    }



    public class AssortmentInWarehouseViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string BarCode { get; set; }
        public double Quantity { get; set; }
        public string Unit { get; set; }
        public string Warehouse { get; set; }
    }
 
     
}