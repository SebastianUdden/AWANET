using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AWANET.Models
{
    public class Message
    {
        public int Id { get; set; }
        public DateTime TimeCreated { get; set; }
        public string MessageBody { get; set; }
        public string ImageLink { get; set; }
        public bool OnFirstPage { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string Title { get; set; }
    }
}
