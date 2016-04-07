using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AWANET.ViewModels
{
    public class CommentsVM
    {
        public List<CommentVM> CommentList { get; set; }
        public int ParentMessageId { get; set; }
    }
}
