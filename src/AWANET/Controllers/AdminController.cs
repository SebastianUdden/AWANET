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
            var list = context.UserCategory.Select(o => o.CategoryName).ToList();
            model.CategoryList = list;

            //Returnerar ett felmeddelande och vy-modellen ifall skapandet av användare misslyckats
            if (!result.Succeeded)
            {
                ModelState.AddModelError("Email", result.Errors.First().Description);
                return View(model);
            }
            var userId = await userManager.GetUserIdAsync(newUser);
            await userManager.AddToRoleAsync(newUser, "Default");
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
            ViewData["UserCreated"] = "1";
            //Kolla resultat på mailutskicket??
            //Metod som skickar ett lösenord till specificerad emailadress
            
            MailSender.SendTo(model.EMail, password,false);
            return View(model);
        }
        public async Task<IActionResult> AdminTemp()
        {
            var rmgr = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context), null, null, null, null, null);

            await rmgr.CreateAsync(new IdentityRole("Admin"));
            var user = await userManager.FindByNameAsync("jonas@meljoner.se");
            var result = await userManager.AddToRoleAsync(user, "Admin");

            return Content(result.Succeeded.ToString());
        }
        public async Task<IActionResult> ToggleAdmin(string eMail, string adminAction)
        {
            var rmgr = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context), null, null, null, null, null);

            if (adminAction == "addAdmin")
            {
                var user = await userManager.FindByNameAsync(eMail);
                var result = await userManager.AddToRoleAsync(user, "Admin");
                if (result.Succeeded)
                {
                    ViewData["AdminAction"] = "Adminrättighet tillagd för " + eMail;
                }
                else
                {
                    ViewData["AdminAction"] = result.Errors.First().ToString();
                }
            }
            else if (adminAction == "removeAdmin")
            {
                var user = await userManager.FindByNameAsync(eMail);
                var result = await userManager.RemoveFromRoleAsync(user, "Admin");
                if (result.Succeeded)
                {
                    ViewData["AdminAction"] = "Adminrättighet borttagen för " + eMail;
                }
                else
                {
                    ViewData["AdminAction"] = result.Errors.First().ToString();
                }
            }

            rmgr.Dispose();
            ContactList contactList = new ContactList();
            var model = await contactList.GetAllContacts(context, userManager);
            return PartialView("_ContactListPartial", model);
        }
        public async Task<IActionResult> TerminateUser(string eMail)
        {
            var user = await userManager.FindByNameAsync(eMail);
            var tmp = context.UserDetails.Where(o => o.Id == user.Id).SingleOrDefault();
            if (tmp != null)
            {
                context.UserDetails.Remove(tmp);
                context.SaveChanges();
            }

            await userManager.DeleteAsync(user);
            ContactList contactList = new ContactList();
            var model = await contactList.GetAllContacts(context, userManager);
            return PartialView("_ContactListPartial", model);
        }

        //public async  Task<IActionResult> addRole()
        //{
        //    var rmgr = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context), null, null, null, null, null);
        //    var users = context.Users.ToList();
        //    await rmgr.CreateAsync(new IdentityRole("User"));
        //    await rmgr.CreateAsync(new IdentityRole("Default"));

        //    foreach (var user in users)
        //    {
        //        var tmp = context.UserDetails.Where(o => o.Id == user.Id).SingleOrDefault();
        //        if (tmp.FirstName == "" || tmp.LastName == "")
        //            await userManager.AddToRoleAsync(user, "Default");
        //        else
        //            await userManager.AddToRoleAsync(user, "User");
        //    }
        //    rmgr.Dispose();
        //    return Content("Baaaam");
        //}
    }
}
