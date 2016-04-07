using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AWANET.ViewModels
{
    public class EditContactDetailsVM
    {
        [Required(ErrorMessage ="Fyll i ditt förnamn.")]
        [Display(Name ="Förnamn")]
        public string FirstName { get; set; }
        [Required(ErrorMessage ="Fyll i ditt efternamn.")]
        [Display(Name = "Efternamn")]
        public string LastName { get; set; }
        [Required(ErrorMessage ="Fyll i ditt telefonnummer.")]
        [Display(Name = "Telefonnummer")]
        public string Phone { get; set; }

        [Display(Name = "Adress")]
        public string Street { get; set; }

        [Display(Name = "Postnummer")]
        public string Zip { get; set; }

        [Display(Name = "Stad")]
        public string City { get; set; }
    }
}
