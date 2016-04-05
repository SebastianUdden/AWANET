using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AWANET.ViewModels
{
    public class MessageVM
    {
        public string Sender { get; set; }
        public string FullName { get; set; }
        public DateTime TimeCreated { get; set; }

        [Required(ErrorMessage ="Meddelandet får inte var tom")]
        public string MessageBody { get; set; }
        public string ImageLink { get; set; }
        [Display(Name = "På förstasidan?")]
        public bool OnFirstPage { get; set; }
        public string Receiver { get; set; }

        [Required(ErrorMessage ="Fyll i en titel")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Titeln måste vara 4 till 50 tecken lång.")]
        public string Title { get; set; }

    }
}
