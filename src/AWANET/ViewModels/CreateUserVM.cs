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
        [Display(Name = "E-post")]
        public string EMail { get; set; }

        [Required(ErrorMessage = "Lägg in en kategori.")]
        [Display(Name = "Kategori")]
        public string CategoryName { get; set; }

        public List<string> CategoryList { get; set; }
    }
}
