using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SystemMagazynowy.ViewModel
{
    public class ShortOperationViewModel
    {
        public string Type { get; set; }
        public string FullNumber { get; set; }
        public DateTime OperationDate { get; set; }
        public double Quantity { get; set; }
        public string User { get; set; }

    }

    public class EditOperationViewModel
    {

        public string Type { get; set; }

        public int Number { get; set; }

        [Display(Name = "Data operacji")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime OperationDate { get; set; }

        [Display(Name = "Kontrahent")]
        public int? ContractorID { get; set; }

        [Display(Name = "Magazyn docelowy")]
        public int? ToWarehouseID { get; set; }


        [Display(Name = "Magazyn źródłowy")]
        public int? FromWarehouseID { get; set; }

        public int? DocumentSourceID { get; set; }


        public string SelectedAssortment { get; set; }
        public ICollection<AssortmentViewModel> Assortment { get; set; }
    }

    public class PzViewModel
    {
        public string Type { get; set; }

        [Display(Name="Numer")]
        public int Number { get; set; }

        [Display(Name="Data operacji")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime OperationDate { get; set; }

        [Display(Name="Kontrahent")]
        public int ContractorID { get; set; }

        [Display(Name="Magazyn docelowy")]
        public int ToWarehouseID { get; set; }

        public string UserID { get; set; }

        public string SelectedAssortment { get; set; }
        public ICollection<AssortmentViewModel> Assortment { get; set; }

    }

    public class PwViewModel
    {
        public int ID { get; set; }

        public string Type { get; set; }

        [Display(Name = "Numer")]
        public int Number { get; set; }

        [Display(Name = "Data operacji")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime OperationDate { get; set; }

        [Display(Name = "Magazyn docelowy")]
        public int ToWarehouseID { get; set; }

        public string UserID { get; set; }

        public string SelectedAssortment { get; set; }
        public ICollection<AssortmentViewModel> Assortment { get; set; }
    }

    public class RwViewModel
    {
        public string Type { get; set; }

        [Display(Name = "Numer")]
        public int Number { get; set; }

        [Display(Name = "Data operacji")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime OperationDate { get; set; }

        [Display(Name = "Magazyn źródłowy")]
        public int FromWarehouseID { get; set; }

        public string UserID { get; set; }

        public string SelectedAssortment { get; set; }
        public ICollection<AssortmentViewModel> Assortment { get; set; }
    }

    public class WzViewModel
    {
        public string Type { get; set; }

        [Display(Name = "Numer")]
        public int Number { get; set; }

        [Display(Name = "Data operacji")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime OperationDate { get; set; }

        [Display(Name = "Kontrahent")]
        public int ContractorID { get; set; }

        [Display(Name = "Magazyn źródłowy")]
        public int FromWarehouseID { get; set; }

        public string UserID { get; set; }

        public string SelectedAssortment { get; set; }
        public ICollection<AssortmentViewModel> Assortment { get; set; }
    }

    public class MmViewModel
    {
        public string Type { get; set; }

        [Display(Name = "Numer")]
        public int Number { get; set; }

        [Display(Name = "Data operacji")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime OperationDate { get; set; }

        [Display(Name = "Magazyn źródłowy")]
        public int FromWarehouseID { get; set; }

        [Display(Name = "Magazyn docelowy")]
        public int ToWarehouseID { get; set; }

        public string UserID { get; set; }

        public string SelectedAssortment { get; set; }
        public ICollection<AssortmentViewModel> Assortment { get; set; }
    }

    
}