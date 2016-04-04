using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using AWANET.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using AWANET.Models;
using Microsoft.AspNet.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AWANET.ViewModels
{
    [Authorize(Roles = "Admin")]

    public class AdminController : Controller
    {
        UserManager<IdentityUser> userManager;
        SignInManager<IdentityUser> signInManager;
        AWAnetContext context;

        public AdminController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager, AWAnetContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
        }

        // GET: /<controller>/
        public IActionResult CreateUser()
        {
            var list = context.UserCategory.Select(o => o.CategoryName).ToList();
            CreateUserVM newUser = new CreateUserVM();
            newUser.CategoryList = list;

            return View(newUser);
        }
        //Metoden är satt till async task för att metoden i sig är en async metod. Task säger att metoden är en asynkron operation.
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserVM model)
        {
            //Returnerar vy-modellen om något modellvärde är felaktigt
            if (!ModelState.IsValid)
                return View(model);

            await context.Database.EnsureCreatedAsync();
            string password = CreatePassword.CreateNewPassword();
            IdentityUser newUser = new IdentityUser(model.EMail);
            var result = await userManager.CreateAsync(newUser, password);

            //Returnerar ett felmeddelande och vy-modellen ifall skapandet av användare misslyckats
            if (!result.Succeeded)
            {
                ModelState.AddModelError("Email", result.Errors.First().Description);
                return View(model);
            }
            var userId = await userManager.GetUserIdAsync(newUser);

            var category = context.UserCategory.Where(x => x.CategoryName == model.CategoryName).SingleOrDefault();

            UserDetail userDetail = new UserDetail();
            userDetail.Id = userId;

            if (category != null)
            {
                userDetail.SemesterId = category.Id;
                context.UserDetails.Add(userDetail);
                context.SaveChanges();
            }
            else
            {
                UserCategory userCategory = new UserCategory();
                userCategory.CategoryName = model.CategoryName;
                context.UserCategory.Add(userCategory);
                context.SaveChanges();
                userDetail.SemesterId = userCategory.Id;
            }

            //Kolla resultat på mailutskicket??
            //Metod som skickar ett lösenord till specificerad emailadress
            MailSender.SendTo(model.EMail, password);
            return RedirectToAction(nameof(AdminController.CreateUser));
        }

        public async Task<IActionResult> AdminTemp()
        {
            var rmgr = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context), null, null, null, null, null);

            await rmgr.CreateAsync(new IdentityRole("Admin"));
            var user = await userManager.FindByNameAsync("jonas@meljoner.se");
            var result = await userManager.AddToRoleAsync(user, "Admin");

            return Content(result.Succeeded.ToString());
        }
        //public async Task<IActionResult> UserTemp()
        //{
        //    var rmgr = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context), null, null, null, null, null);

        //    await rmgr.CreateAsync(new IdentityRole("User"));
        //    foreach (var u in context.Users)
        //    {
        //        await userManager.Add
        //    }
        //        var user = await userManager.FindByNameAsync("jonas@meljoner.se");
        //        var result = await userManager.AddToRoleAsync(user, "Admin");

        //    return Content(result.Succeeded.ToString());
        //}
    }
}
