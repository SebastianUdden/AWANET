using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AWANET.Models
{
    public class GroupHandler
    {
        public List<string> GetUserGroups(AWAnetContext context, string userId)
        {
            var listOfGroups = context.UserGroups.Where(x => x.UserId == userId).ToList();
            var listOfGroupNames = new List<string>();
            foreach (var group in listOfGroups)
            {
                listOfGroupNames.Add(context.Groups.Where(x => x.Id == group.GroupId).Select(x => x.GroupName).SingleOrDefault());
            }
            return listOfGroupNames;
        }
    }
}
