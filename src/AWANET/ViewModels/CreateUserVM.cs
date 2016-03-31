using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AWANET.ViewModels
{
    public class CreateUserVM
    {
        [Required(ErrorMessage = "Lägg in E-postadress.")]
        [EmailAddress(ErrorMessage = "Ange giltig E-postadress, email@academic.se")]
        public string EMail { get; set; }
    }
}
