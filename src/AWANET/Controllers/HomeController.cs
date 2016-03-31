using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AWANET.ViewModels
{
    [Authorize]
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Redirect()
        {
            //Den här metoden tillåter ej inloggade användare att riktas om till logga in sidan. Enda metoden som är tillåten för ej inloggade användare.
            return RedirectToAction("Login", "Account");
        }
    }
}
