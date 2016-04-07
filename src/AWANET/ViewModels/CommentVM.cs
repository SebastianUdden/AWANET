using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AWANET.ViewModels
{
    public class CommentVM
    {
        public string CommentBody { get; set; }
        public string SenderName { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
