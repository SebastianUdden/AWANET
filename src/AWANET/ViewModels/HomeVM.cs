using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AWANET.ViewModels
{
    public class HomeVM
    {
        public List<MessageVM> MessageVMList { get; set; }
        public List<GroupVM> GroupVMList { get; set; }
        public int PageId { get; set; }
    }
}
