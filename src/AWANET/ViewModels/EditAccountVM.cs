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

        public EditContactDetailsVM ContactDetails{ get; set; }

        //Lägg in övriga proppar som ska synas på mina sidor
        [Display(Name = "Förnamn")]
        public string FirstName { get; set; }
        [Display(Name = "Efternamn")]
        public string LastName { get; set; }
        [Display(Name = "Adress")]
        public string Address { get; set; }
    }
}
