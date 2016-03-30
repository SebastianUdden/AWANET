﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AWANET.ViewModels
{
    public class CreateUserVM
    {
        [Required]
        [EmailAddress(ErrorMessage = "Ange giltig email adress, email@academic.se")]
        public string EMail { get; set; }
    }
}