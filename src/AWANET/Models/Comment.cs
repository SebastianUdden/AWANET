using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AWANET.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string CommentBody { get; set; }
        public int PostId { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
