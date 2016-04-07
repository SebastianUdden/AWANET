using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using AWANET.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using AWANET.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AWANET.Controllers
{
    public class FirstTimeUserController : Controller
    {
        UserManager<IdentityUser> userManager;
        AWAnetContext context;

        public FirstTimeUserController(UserManager<IdentityUser> userManager, AWAnetContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(EditContactDetailsVM model)
        {
            if (!ModelState.IsValid)
                return PartialView("_EditContactDetailsPartial", model);

            var userId = await GetUserId();
            EditUser editUser = new EditUser(context);
            editUser.UpdateUserDetails(model, userId);
            ViewData["Message"] = "1";
            //return PartialView("_EditContactDetailsPartial",model);

            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var result1 = await userManager.RemoveFromRoleAsync(user, "Default");
            var result = await userManager.AddToRoleAsync(user, "User");

            return PartialView("_EditContactDetailsPartial", model);
            //return View();
        }
        private async Task<string> GetUserId()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            return user.Id;
        }
    }
}
