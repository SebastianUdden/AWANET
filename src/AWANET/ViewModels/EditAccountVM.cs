using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AWANET.ViewModels
{
    public class EditAccountVM
    {
        [Display(Name ="E-Post")]
        [Required]
        [EmailAddress]
        public string EMail { get; set; }
        public ChangePasswordVM ChangePassword { get; set; }
        //[Display(Name = "Nuvarande lösenord")]
        //[DataType(DataType.Password)]
        //[Required(ErrorMessage = "Fältet kan inte vara tomt")]
        //[StringLength(20, MinimumLength = 6)]
        //public string OldPassword { get; set; }

        //[Display(Name = "Nytt lösenord")]
        //[DataType(DataType.Password)]
        //[Required(ErrorMessage = "Fältet kan inte vara tomt")]
        //[StringLength(20, MinimumLength = 6)]
        //public string NewPassword { get; set; }

        //[CompareAttribute("NewPassword", ErrorMessage = "Lösenorden matchar inte.")]
        //[Display(Name = "Bekräfta nytt lösenord")]
        //[DataType(DataType.Password)]
        //[Required(ErrorMessage = "Fältet kan inte vara tomt")]
        //[StringLength(20, MinimumLength = 6)]
        //public string ConfirmNewPassword { get; set; }

        //Lägg in övriga proppar som ska synas på mina sidor
        [Display(Name = "Förnamn")]
        public string FirstName { get; set; }
        [Display(Name = "Efternamn")]
        public string LastName { get; set; }
        [Display(Name = "Adress")]
        public string Address { get; set; }
    }
}
