using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using AWANET.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using AWANET.Models;

namespace AWANET.Controllers
{
    public class FirstTimeUserController : Controller
    {
        UserManager<IdentityUser> userManager;
        AWAnetContext context;
        SignInManager<IdentityUser> signInManager;

        public FirstTimeUserController(UserManager<IdentityUser> userManager, AWAnetContext context, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.context = context;
            this.signInManager = signInManager;
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
            await userManager.RemoveFromRoleAsync(user, "Default");
            await userManager.AddToRoleAsync(user, "User");
            
            GroupHandler grp = new GroupHandler();
            grp.AddToStartGroup(context, user.Id);

            //await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        private async Task<string> GetUserId()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            return user.Id;
        }
    }
}
