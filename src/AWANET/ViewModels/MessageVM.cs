﻿using Microsoft.AspNet.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AWANET.ViewModels
{
    public class MessageVM
    {
        public bool IsEdit { get; set; }

        public int Id { get; set; }
        public string UserRole { get; set; }
        public string Sender { get; set; }
        public string FullName { get; set; }
        public DateTime TimeCreated { get; set; }
        public bool IsCurrentUser { get; set; }

        [Required(ErrorMessage ="Meddelandet får inte vara tomt")]
        [StringLength(7000, MinimumLength = 10, ErrorMessage = "Meddelandet måste vara minst 10 tecken.")]

        public string MessageBody { get; set; }
        public string ImageLink { get; set; }
        [Display(Name = "På förstasidan?")]
        public bool OnFirstPage { get; set; }

        [Display(Name = "Mottagargrupp")]
        [Required(ErrorMessage = "Du måste välja mottagargrupp")]
        [StringLength(15, MinimumLength = 3, ErrorMessage = "Gruppens namn måste vara minst 3 tecken och max 15")]
        public string Receiver { get; set; }
        [Display(Name = "Rubrik")]
        [Required(ErrorMessage ="Fyll i en rubrik")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Rubriken måste vara 4 till 50 tecken lång.")]
        public string Title { get; set; }
        public IFormFile MessagePicture { get; set; }
        public List<string> Groups { get; set; }
        public CommentsVM Comments { get; set; }
    }
}