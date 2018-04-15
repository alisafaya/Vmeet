using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace Vmeet.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public string Email { get; set; }
        public string AD { get; set; }
        public string Soyad { get; set; }
        public int ProfilResmi { get; set; } 
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Eski Şifre")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Tekrar Yeni Şifre")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ProfileInfoViewModel
    {
        [StringLength(100, ErrorMessage = "Adınız en az {2} uzunluklu olmalı.", MinimumLength = 2)]
        [Display(Name = "Ad")]
        [Required(ErrorMessage = "Lütfen bir ad giriniz")]
        [DataType(DataType.Text)]
        public string Ad { get; set; }

        [StringLength(100, ErrorMessage = "Adınız en az {2} uzunluklu olmalı.", MinimumLength = 2)]
        [Display(Name = "Soyad")]
        [Required(ErrorMessage = "Lütfen bir soyad giriniz")]
        [DataType(DataType.Text)]
        public string Soyad { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Lütfen bir Email giriniz")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }

    public class ProfileChangeViewModel
    {
        public string UserId { get; set; }
        public ProfileInfoViewModel Profil { get; set; }
        public ChangePasswordViewModel Password { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }
}