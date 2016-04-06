using AWANET.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AWANET.Models
{
    public class GroupHandler
    {
        public List<GroupVM> GetUserGroups(AWAnetContext context, string userId)
        {
            var listOfGroups = context.UserGroups.Where(x => x.UserId == userId).Select(o => o.GroupId).ToList();
            var listOfGroupNames = new List<GroupVM>();
            foreach (var group in listOfGroups)
            {
                var tmp = (context.Groups.Where(x => x.Id == group).SingleOrDefault());
                if (tmp != null)
                    listOfGroupNames.Add(new GroupVM { Id = tmp.Id, GroupName = tmp.GroupName });
            }
            return listOfGroupNames;
        }
        public List<string> GetAllGroups(AWAnetContext context)
        {
            return context.Groups.Where(o => o.IsOpen == true).Select(o => o.GroupName).ToList();

        }

        public List<GroupVM> GetAllGroupVMs(AWAnetContext context, string userID)
        {
            var listOfGroupIDConnectedToUser = context.UserGroups
                .Where(u => u.UserId == userID)
                .Select(o => o.GroupId)
                .ToList();
            foreach (var item in listOfGroupIDConnectedToUser)
            {
                 Debug.WriteLine(item);
            }
            var ListofGroupVM = context.Groups
                .Where(o => o.IsOpen == true && !(listOfGroupIDConnectedToUser.Contains(o.Id)))
                .Select(o => new GroupVM
                {
                    Id = o.Id,
                    GroupName = o.GroupName
                }).ToList();
            
            
            return ListofGroupVM;
        }
    }
}
