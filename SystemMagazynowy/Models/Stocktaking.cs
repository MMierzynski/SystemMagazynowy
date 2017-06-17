using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace SystemMagazynowy.Models
{
    public class Stocktaking
    {
        public int ID { get; set; }

        [Display(Name ="Numer")]
        public int Number { get; set; }

        [Display(Name ="Data rozpoczęcia")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Display(Name ="Data zakończenia")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        public bool IsOpen { get; set; }

        [Display(Name ="Magazyn")]
        public int WarehouseID { get; set; }

        [Display(Name ="Użytkownik")]
        public string UserId { get; set; }

        public virtual ICollection<StocktakingAssortment> Assortment { get; set; }
        public virtual Warehouse Warehouse { get; set; }
        public virtual ApplicationUser User { get; set; }

    }
}