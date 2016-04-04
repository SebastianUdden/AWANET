using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using AWANET.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AWANET.ViewModels
{
    [Authorize]
    public class HomeController : Controller
    {
        AWAnetContext context;

        public HomeController(AWAnetContext context)
        {
            this.context = context;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            var listMessages = context.Messages.Where(o=>o.OnFirstPage == true).Select(o => new MessageVM
            {
                Sender = o.Sender,
                MessageBody = o.MessageBody,
                TimeCreated = o.TimeCreated,
                ImageLink = o.ImageLink != String.Empty ? o.ImageLink : String.Empty
            }).OrderBy(o=>o.TimeCreated).ToList();

            foreach (var message in listMessages)
            {
                var temp = context.UserDetails.Where(u => u.Id == message.Sender).SingleOrDefault();
                if (temp != null)
                    message.FullName = temp.FirstName + " " + temp.LastName;
            }
            return View(listMessages);
        }
        [AllowAnonymous]
        public IActionResult Redirect()
        {
            //Den här metoden tillåter ej inloggade användare att riktas om till logga in sidan. Enda metoden som är tillåten för ej inloggade användare.
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Calendar()
        {
            return View();
        }

        public IActionResult PostMessage()
        {
            return PartialView("_EditorPartial");
        }
        [HttpPost]
        public IActionResult PostMessage(MessageVM message)
        {
            var 
            return Content(message.MessageBody);
        }
    }
}
