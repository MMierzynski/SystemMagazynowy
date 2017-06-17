using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SystemMagazynowy.Models
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class UserViewModel
    {
        [Required]
        [Display(Name ="Nazwa użytkownika")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name ="Numer telefonu")]
        public string PhoneNumber { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name ="Adres email")]
        public string Email { get; set; }


    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Nazwa Użytkownika")]
        public string UserName{ get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [Display(Name = "Zapamiętaj mnie")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        

        [Required]
        [StringLength(30, MinimumLength = 5)]
        [Display(Name = "Nazwa użytkownika")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} musi być dłuższe niż {2} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        
        [DataType(DataType.Password)]
        [Display(Name = "Potwierdź hasło")]
        [Compare("Password", ErrorMessage = "Hasła muszą byc identyczne")]
        public string ConfirmPassword { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Numer telefonu")]
        public string PhoneNumber { get; set; }

        

        [Display(Name = "Zarządzanie użytkownikami")]
        public bool canManageUsers { get; set; }

        [Display(Name = "Zarządzanie kontrahentami")]
        public bool canManageContractors { get; set; }

        [Display(Name = "Zarządzanie kategoriami")]
        public bool canManageCategories { get; set; }

        [Display(Name = "Zarządzanie magazynami")]
        public bool canManageWarehouses { get; set; }

        [Display(Name = "Zarządzanie asortymentem")]
        public bool canManageAssortment { get; set; }

        [Display(Name = "Przeprowadzanie inwentaryzacji")]
        public bool canPerformStocktaking { get; set; }

        [Display(Name = "Działania na asortmencie")]
        public bool canOperateWithAssortment { get; set; }
    }


    public class EditUserViewModel
    {
        public string ID { get; set; }

        [Required]
        [StringLength(30, MinimumLength = 5)]
        [Display(Name = "Nazwa użytkownika")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Numer telefonu")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Zarządzanie użytkownikami")]
        public bool canManageUsers { get; set; }

        [Display(Name = "Zarządzanie kontrahentami")]
        public bool canManageContractors { get; set; }

        [Display(Name = "Zarządzanie kategoriami")]
        public bool canManageCategories { get; set; }

        [Display(Name = "Zarządzanie magazynami")]
        public bool canManageWarehouses { get; set; }

        [Display(Name = "Zarządzanie asortymentem")]
        public bool canManageAssortment { get; set; }

        [Display(Name = "Przeprowadzanie inwentaryzacji")]
        public bool canPerformStocktaking { get; set; }

        [Display(Name = "Działania na asortmencie")]
        public bool canOperateWithAssortment { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class RoleCheckBox
    {
        public string Name { get; set; }
        public bool IsChecked { get; set; }
        //public string Disabled { get; set; }
    }
}
