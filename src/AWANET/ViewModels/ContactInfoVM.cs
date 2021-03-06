﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AWANET.ViewModels
{
    public class ContactInfoVM
    {
        public string Id { get; set; }
        [Display(Name = "Förnamn")]
        public string FirstName { get; set; }

        [Display(Name = "Efternamn")]
        public string LastName { get; set; }

        [Display(Name = "Telefonnummer")]
        public string Phone { get; set; }

        [Display(Name = "E-post")]
        public string EMail { get; set; }

        [Display(Name = "Adress")]
        public string Street { get; set; }

        [Display(Name = "Postnummer")]
        public string Zip { get; set; }

        [Display(Name = "Stad")]
        public string City { get; set; }
    }
}
