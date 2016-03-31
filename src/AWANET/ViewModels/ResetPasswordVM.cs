using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AWANET.ViewModels
{
    public class ResetPasswordVM
    {
        [Required]
        [EmailAddress]
        public string EMail { get; set; }
    }
}
