using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AWANET.ViewModels
{
    public class LoginVM
    {
        [Required]
        [EmailAddress (ErrorMessage = "Ange giltig email adress, email@academic.se")]
        public string EMail { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Field can't be empty")]
        [StringLength(20, MinimumLength = 6)]
        public string Password { get; set; }
    }
}
