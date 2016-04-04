using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using AWANET.Models;
using Microsoft.AspNet.Identity.EntityFramework;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AWANET.ViewModels
{
    [Authorize]
    public class HomeController : Controller
    {
        AWAnetContext context;
        UserManager<IdentityUser> userManager;
        SignInManager<IdentityUser> signInManager;

        public HomeController(AWAnetContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            var listMessages = context.Messages.Where(o=>o.OnFirstPage == true).Select(o => new MessageVM
            {
                Sender = o.Sender,
                Title = o.Title,
                MessageBody = o.MessageBody,
                TimeCreated = o.TimeCreated,
                ImageLink = o.ImageLink != String.Empty ? o.ImageLink : String.Empty
            }).OrderByDescending(o=>o.TimeCreated).ToList();

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
        public async Task<IActionResult> PostMessage(MessageVM message)
        {
            if (!ModelState.IsValid)
            {
                return Content("false");
            }
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var userId = await userManager.GetUserIdAsync(user);
            Message newMessage = new Message();
            newMessage.Title = message.Title;
            newMessage.MessageBody = message.MessageBody;
            newMessage.OnFirstPage = message.OnFirstPage;
            newMessage.Sender = userId;
            newMessage.Receiver = "All";
            newMessage.TimeCreated = DateTime.Now;
            if (message.ImageLink != null)
            {
                newMessage.ImageLink = message.ImageLink;
            }



            context.Messages.Add(newMessage);
            var result = await context.SaveChangesAsync();
            if(result > 0)
            {
                ViewData["MessageData"] = "Meddelande sparat.";
            }
            else
            {
                ViewData["MessageData"] = "It went to shajt!!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
