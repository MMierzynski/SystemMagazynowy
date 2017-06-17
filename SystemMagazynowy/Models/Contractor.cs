using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SystemMagazynowy.Models
{
    public class Contractor
    {
        [Required]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Kontrahent")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Adres")]
        public string Address { get; set; }
        [DataType(DataType.PostalCode)]

        [Required]
        [Display(Name = "Kod pocztowy")]
        public string ZipCode { get; set; }

        [Required]
        [Display(Name = "Miejscowość")]
        public string City { get; set; }

        [Display(Name = "Numer telefonu")]
        [DataType(DataType.PhoneNumber)]
        public int? PhoneNumber { get; set; }

        [Display(Name = "Fax")]
        [DataType(DataType.PhoneNumber)]
        public int? Fax { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "NIP")]
        [StringLength(10,MinimumLength=10)]
        public string NIP { get; set; }

        [Display(Name = "REGON")]
        [StringLength(14, MinimumLength = 9)]
        public string REGON { get; set; }

        public virtual ICollection<Assortment> Assortment { get; set; }
    }
}