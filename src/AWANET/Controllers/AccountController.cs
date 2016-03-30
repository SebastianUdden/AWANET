using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using AWANET.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AWANET.Controllers
{
    public class AccountController : Controller
    {
        SignInManager<IdentityUser> signInManager;
        IdentityDbContext context;

        public AccountController(SignInManager<IdentityUser> signInManager,
            IdentityDbContext context)
        {
            this.signInManager = signInManager;
            this.context = context;
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
            await signInManager.PasswordSignInAsync(loginvm.EMail, loginvm.Password, false, false);
            //return RedirectToAction(nameof(HomeController.Index);
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Logout()
        {
            signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
