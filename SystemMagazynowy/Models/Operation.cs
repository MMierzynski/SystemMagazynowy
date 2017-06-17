using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SystemMagazynowy.Models
{
    public class Operation
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Operacja")]
        public string Type { get; set; }

        [Required]
        [Display(Name = "Numer")]
        public int Number { get; set; }

        [NotMapped]
        [Display(Name="Numer")]
        public string FullNumber
        {
            get { return Number+"/"+OperationDate.Year; }
        }

        [Required]
        [Display(Name="Data operacji")]
        [DataType(DataType.Date)]
        public DateTime OperationDate { get; set; }

        public int? ContractorID { get; set; }
        
        public string UserId { get; set; }

       
        [Display(Name="Magazyn źródłowy")]
        public int? FromWarehouseID { get; set; }

        
        [Display(Name = "Magazyn docelowy")]
        public int? ToWarehouseID { get; set; }

        public int? OperationID { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Contractor Contractor { get; set;}
        public virtual Warehouse ToWarehouse { get; set; }
        public virtual Warehouse FromWarehouse { get; set; }
        public virtual Operation DocumentSource{ get; set; }
        public virtual ICollection<OperationAssortment> OperationAssortment { get; set; }

    }
}