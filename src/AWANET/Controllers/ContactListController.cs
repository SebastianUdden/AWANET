using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using AWANET.Models;
using Microsoft.AspNet.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AWANET.Controllers
{
    [Authorize(Roles ="User")]
    
        public class ContactListController : Controller
    {
        UserManager<IdentityUser> userManager;
        AWAnetContext context;
        ContactList contactList;

        public ContactListController(UserManager<IdentityUser> userManager, AWAnetContext context)
        {
            this.userManager = userManager;
            this.context = context;
            contactList = new ContactList();
        }
        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            var listUser = await contactList.GetAllContacts(context, userManager);
            return View(listUser);
        }
        public IActionResult GetContact(string Email, string UserId)
        {
            var model = contactList.GetContact(Email, UserId, context);
            return PartialView("_ShowContactPartial", model);
        }


    }
}
