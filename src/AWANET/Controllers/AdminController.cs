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
        //Metoden är satt till async task för att metoden i sig är en async metod. Task säger att metoden är en asynkron operation.
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserVM model)
        {
            //Returnerar vy-modellen om något modellvärde är felaktigt
            if (!ModelState.IsValid)
                return View(model);
            await context.Database.EnsureCreatedAsync();
            string password = CreatePassword();
            var result = await userManager.CreateAsync(new IdentityUser(model.EMail), password);
            //Returnerar ett felmeddelande och vy-modellen ifall skapandet av användare misslyckats
            if (!result.Succeeded)
            {
                ModelState.AddModelError("Email", result.Errors.First().Description);
                return View(model);
            }
            //Kolla resultat på mailutskicket??
            //Metod som skickar ett lösenord till specificerad emailadress
            mailSender.SendTo(model.EMail, password);
            return RedirectToAction(nameof(AdminController.CreateUser));
            
        }
        //Här skapas ett slumpat lösenord som sedan ska skickas till den skapade användaren
        private string CreatePassword()
        {
            char[] password = new char[7];
            for (int i = 0; i < 7; i++)
            {
                //caseSwitch roterar över switchen för att generera lösenord. Detta garanterar att vi har liten och stor bokstav, samt siffra
                int caseSwitch = (i % 3) + 1;
                switch (caseSwitch)
                {
                    //Stora bokstäver ligger mellan ascii 65-90
                    case 1:
                        password[i] = (char)rand.Next(65, 90 + 1);
                        break;
                    //Små bokstäver ligger mellan 97-122
                    case 2:
                        password[i] = (char)rand.Next(97, 122 + 1);
                        break;
                    //Siffror ligger mellan 48-57
                    case 3:
                        password[i] = (char)rand.Next(48, 57 + 1);
                        break;
                }
            }
            //Returnerar lösenordet
            return new string(password);
        }
    }
}
