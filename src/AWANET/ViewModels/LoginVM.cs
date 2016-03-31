using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AWANET.ViewModels
{
    public class LoginVM
    {
        [Display(Name ="E-post")]
        [Required(ErrorMessage ="Ange en e-postadress.")]
        [EmailAddress (ErrorMessage = "Ange giltig email adress, email@academic.se")]
        public string EMail { get; set; }

        [Display(Name = "Lösenord")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Ange ditt lösenord.")]
        [StringLength(20, MinimumLength = 6,ErrorMessage ="Lösenordet måste vara 6 till 20 tecken långt.")]
        public string Password { get; set; }
    }
}
