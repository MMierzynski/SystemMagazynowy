using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace SystemMagazynowy.ViewModel
{
    public class OpenStacktakingViewModel
    {
        [Display(Name ="Numer")]
        public int Number { get; set; }

        [Display(Name ="Data rozpoczęcia")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Data zakończenia")]
        //[DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        public bool IsOpen { get; set; }

        [Display(Name ="Magazyn")]
        public int WarehouseID { get; set; }

        public string UserID { get; set; }

        public string AssortmentInStocktaking { get; set; }

        public ICollection<StocktakingAssortmentViewModel> Assortment { get; set; }
    }
}