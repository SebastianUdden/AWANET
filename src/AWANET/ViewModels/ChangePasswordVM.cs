using System.ComponentModel.DataAnnotations;

namespace AWANET.ViewModels
{
    public class ChangePasswordVM
    {
        [Display(Name = "Nuvarande lösenord")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Fältet kan inte vara tomt")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Lösenordet måste vara 6 till 20 tecken långt.")]
        public string OldPassword { get; set; }

        [Display(Name = "Nytt lösenord")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Fältet kan inte vara tomt")]
        [StringLength(20, MinimumLength = 6,ErrorMessage =  "Lösenordet måste vara 6 till 20 tecken långt.")]
        public string NewPassword { get; set; }

        [CompareAttribute("NewPassword", ErrorMessage = "Lösenorden matchar inte.")]
        [Display(Name = "Bekräfta nytt lösenord")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Fältet kan inte vara tomt")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Lösenordet måste vara 6 till 20 tecken långt.")]
        public string ConfirmNewPassword { get; set; }
    }
}