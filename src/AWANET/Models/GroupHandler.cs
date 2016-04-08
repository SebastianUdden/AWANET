using AWANET.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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

        public void AddToStartGroup(AWAnetContext context, string id)
        {
            UserGroup usergroup = new UserGroup();
            usergroup.GroupId = 1;
            usergroup.UserId = id;
            context.UserGroups.Add(usergroup);
            context.SaveChanges();
        }
        public bool RemoveFromGroup(AWAnetContext context,string userId, int groupId)
        {

            UserGroup userGroup = context.UserGroups
                .Where(o => o.GroupId == groupId && o.UserId == userId)
                .SingleOrDefault();

            context.UserGroups.Remove(userGroup);
            var result = context.SaveChanges();
            if (result > 0)
                return true;
            else
                return false;
        }
    }
}
