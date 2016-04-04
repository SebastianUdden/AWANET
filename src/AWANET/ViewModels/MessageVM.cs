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
        public string MessageBody { get; set; }
        public string ImageLink { get; set; }
        [Display(Name = "Wharsch ska he va? på förstn?")]
        public bool OnFirstPage { get; set; }

    }
}
