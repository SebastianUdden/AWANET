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
using Microsoft.AspNet.Http;
using System.IO;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNet.Hosting;
using System.Threading;

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
        //IdentityDbContext identityContext;
        UserManager<IdentityUser> userManager;
        EditUser editUser;
        private IHostingEnvironment _environment;

        public AccountController(SignInManager<IdentityUser> signInManager,
            AWAnetContext context, /*IdentityDbContext idcontext,*/ UserManager<IdentityUser> userManager, IHostingEnvironment environment)
        {
            this.signInManager = signInManager;
            this.context = context;
            this.userManager = userManager;
            editUser = new EditUser(context);
            _environment = environment;

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
            var model = new EditAccountVM();
            model.EMail = User.Identity.Name;
            string id = await GetUserId();
            var editContact = editUser.GetUser(id);
            model.ContactDetails = editContact;
            return View(model);
        }

        private async Task<string> GetUserId()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            return user.Id;
        }

        [HttpPost]
        [ActionName("EditPassword")]
        public async Task<IActionResult> MyPages(ChangePasswordVM model)
        {

            if (!ModelState.IsValid)
                return View(model);

            bool result = await ChangePassword(model);
            if (result)
            {
                var pageModel = new EditAccountVM();

                ViewData["Password"] = "1";
                return PartialView("_ChangePasswordPartial");
            }
            ModelState.AddModelError("Error", "Lösenordet måste innehålla gemener, versaler och minst en siffra.");
            return PartialView("_ChangePasswordPartial");
        }
        [HttpPost]
        [ActionName("EditDetails")]
        public async Task<IActionResult> MyPages(EditContactDetailsVM model)
        {
            if (!ModelState.IsValid)
                return PartialView("_EditContactDetailsPartial", model);
            var userId = await GetUserId();
            EditUser editUser = new EditUser(context);
            editUser.UpdateUserDetails(model, userId);
            ViewData["Message"] = "1";
            //return PartialView("_EditContactDetailsPartial",model);
            return PartialView("_EditContactDetailsPartial", model);
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

        [HttpPost]
        public async Task<IActionResult> UploadProfilePicture(IFormFile file)
        {
            var profilePictures = Path.Combine(_environment.WebRootPath, "ProfilePictures");
            if (file != null)
            {
                if (file.Length > 0)
                {
                    string fileNameId = context.Users.Where(o => o.UserName == User.Identity.Name).Select(x => x.Id).SingleOrDefault();
                    await file.SaveAsAsync(Path.Combine(profilePictures, fileNameId + ".jpg"));
                    ViewData["Status"] = "Ny profilbild uppladdad!";
                    return PartialView("_UploadPicturePartial");
                }
            }
            ViewData["Status"] = "Uppladdning misslyckades!";
            return PartialView("_UploadPicturePartial");
        }
    }
}
