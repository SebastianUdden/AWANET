using System.ComponentModel.DataAnnotations;

namespace AWANET.ViewModels
{
    public class ChangePasswordVM
    {
        [Display(Name = "Nuvarande lösenord")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Fältet kan inte vara tomt")]
        [StringLength(20, MinimumLength = 6)]
        public string OldPassword { get; set; }

        [Display(Name = "Nytt lösenord")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Fältet kan inte vara tomt")]
        [StringLength(20, MinimumLength = 6)]
        public string NewPassword { get; set; }

        [CompareAttribute("NewPassword", ErrorMessage = "Lösenorden matchar inte.")]
        [Display(Name = "Bekräfta nytt lösenord")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Fältet kan inte vara tomt")]
        [StringLength(20, MinimumLength = 6)]
        public string ConfirmNewPassword { get; set; }
    }
}