using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using AWANET.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Authorization;
using AWANET.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AWANET.ViewModels
{
    public class AccountController : Controller
    {
        // Använder Identity-ramverket
        // - SignInManager är en klass för att hantera in och utloggning, autentisering
        SignInManager<IdentityUser> signInManager;
        // - Databaskopplingen
        AWAnetContext context;
        UserManager<IdentityUser> userManager;
        EditUser editUser;

        public AccountController(SignInManager<IdentityUser> signInManager,
            AWAnetContext context, UserManager<IdentityUser> userManager)
        {
            this.signInManager = signInManager;
            this.context = context;
            this.userManager = userManager;
            editUser = new EditUser(context);
            
        }
        // GET: /<controller>/
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginvm)
        {
            if (!ModelState.IsValid)
            {
                return View(loginvm);
            }
            // Inbyggd metod för att sköta dekrypteringar och säkerhet, de sista två är isPersistent (loginfail !=> rensa inloggningsuppgifter) och lockOutOnFailure
            var result = await signInManager.PasswordSignInAsync(loginvm.EMail, loginvm.Password, false, false);
            //return RedirectToAction(nameof(HomeController.Index);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("errormessage", "Inloggning misslyckades.");
                loginvm.Password = "";
                return View(loginvm);
            }
            // Skickar användaren till home, första variabeln är IAction, andra är Controller
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Logout()
        {
            // Metod för att logga ut användaren.
            signInManager.SignOutAsync();
            // Skickar användaren till inloggninssidan, nameof används för att använda action inom nuvarande controller istället för en absolut URL.
            return RedirectToAction(nameof(Login));
        }
        public async Task<IActionResult> MyPages()
        {
            //var model = new EditAccountVM();
            //model.EMail = User.Identity.Name;
            string id =await GetUserId();
            var model =editUser.GetUser(id);
            return View(model);
        }

        private async Task<string> GetUserId()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            return user.Id;
        }

        [HttpPost]
        public async Task<IActionResult> MyPages(ChangePasswordVM model)
        {

            if (!ModelState.IsValid)
                return View(model);

            bool result = await ChangePassword(model);
            if (result)
            {

                var pageModel = new EditAccountVM();
                pageModel.EMail = User.Identity.Name;
                return View(pageModel);
            }

            return PartialView("_ChangePasswordPartial", model);
        }
        [HttpPost]
        public async Task<IActionResult> MyPages(EditContactDetailsVM model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var userId = await GetUserId();
            EditUser editUser=new EditUser(context);
            editUser.UpdateUserDetails(model, userId);
            return View(model);
        }

        private async Task<bool> ChangePassword(ChangePasswordVM editModel)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var result = await userManager.ChangePasswordAsync(user, editModel.OldPassword, editModel.NewPassword);

            if (result.Succeeded)
                return true;

            ModelState.AddModelError("errormessage", result.Errors.First().Description);
            return false;
        }

        public IActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await userManager.FindByNameAsync(model.EMail);
            if (user != null)
            {

                await userManager.RemovePasswordAsync(user);
                string password = CreatePassword.CreateNewPassword();
                var result = await userManager.AddPasswordAsync(user, password);
                //var result = await userManager.CreateAsync(new IdentityUser(model.EMail), password);
                ////Returnerar ett felmeddelande och vy-modellen ifall skapandet av användare misslyckats
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("Email", result.Errors.First().Description);
                    return View(model);
                }
                //Kolla resultat på mailutskicket??
                //Metod som skickar ett lösenord till specificerad emailadress
                MailSender.SendTo(model.EMail, password);
                return RedirectToAction(nameof(Login));
            }
            ModelState.AddModelError("error", "Hittar ej E-post");
            return View(model);
        }
    }
}
