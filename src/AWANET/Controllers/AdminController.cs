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

namespace AWANET.Controllers
{
    [Authorize(Roles = "Admin")]

    public class AdminController : Controller
    {
        MailSender mailSender;
        UserManager<IdentityUser> userManager;
        SignInManager<IdentityUser> signInManager;
        IdentityDbContext context;
        Random rand;
        public AdminController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager, IdentityDbContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
            mailSender = new MailSender();
            rand = new Random();
        }

        // GET: /<controller>/
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserVM model)
        {
            if (!ModelState.IsValid)
                return View(model);
            await context.Database.EnsureCreatedAsync();
            string password = CreatePassword();
            var result = await userManager.CreateAsync(new IdentityUser(model.EMail), password);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("Email", result.Errors.First().Description);
                return View(model);
            }
            //Kolla resultat på mailutskicket?? läskvitto till billy
            mailSender.SendTo(model.EMail, password);
            return RedirectToAction(nameof(AdminController.CreateUser));
        }

        private string CreatePassword()
        {
            char[] password = new char[7];
            for (int i = 0; i < 7; i++)
            {
                int caseSwitch = (i % 3) + 1;
                switch (caseSwitch)
                {
                    case 1:
                        password[i] = (char)rand.Next(65, 90 + 1);
                        break;

                    case 2:
                        password[i] = (char)rand.Next(97, 122 + 1);
                        break;

                    case 3:
                        password[i] = (char)rand.Next(48, 57 + 1);
                        break;
                }
            }

            string s = new string(password);
            return s + "!";
        }
    }
}
