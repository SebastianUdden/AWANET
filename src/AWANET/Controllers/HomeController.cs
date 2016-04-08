using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Identity;
using AWANET.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Hosting;
using System.IO;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AWANET.ViewModels
{
    [Authorize(Roles = "User")]
    public class HomeController : Controller
    {
        IHostingEnvironment _environment;
        AWAnetContext context;
        UserManager<IdentityUser> userManager;
        SignInManager<IdentityUser> signInManager;

        public HomeController(AWAnetContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IHostingEnvironment _environment)
        {
            this._environment = _environment;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
        }
        // GET: /<controller>/
        public IActionResult Index(int id = 1)
        {
            var user = context.Users.Where(x => x.UserName == User.Identity.Name).SingleOrDefault();
            GroupHandler groupHandler = new GroupHandler();
            var listOfGroupVM = groupHandler.GetUserGroups(context, user.Id);
            List<string> listOfGroupNames = new List<string>();

            foreach (var group in listOfGroupVM)
            {
                listOfGroupNames.Add(group.GroupName);
            }

            List<MessageVM> listMessages = new List<MessageVM>();
            // || listOfGroupNames.Contains(o.Receiver)
            if (id == 1)
            {
                listMessages = context.Messages.Where(o => o.OnFirstPage == true).Select(o => new MessageVM
                {
                    Id = o.Id,
                    Sender = o.Sender,
                    Title = o.Title,
                    Receiver = o.Receiver,
                    MessageBody = o.MessageBody,
                    TimeCreated = o.TimeCreated,
                    ImageLink = o.ImageLink != String.Empty ? o.ImageLink : String.Empty,
                    IsCurrentUser = o.Sender == user.Id ? true : false,
                    Comments = GetComments(o.Id)
                }).OrderByDescending(o => o.TimeCreated).ToList();
            }
            else
            {
                var groupname = listOfGroupVM.Where(x => x.Id == id).Select(x => x.GroupName).SingleOrDefault();
                listMessages = context.Messages.Where(o => o.Receiver == groupname).Select(o => new MessageVM
                {
                    Id = o.Id,
                    Sender = o.Sender,
                    Title = o.Title,
                    Receiver = o.Receiver,
                    MessageBody = o.MessageBody,
                    TimeCreated = o.TimeCreated,
                    ImageLink = o.ImageLink != String.Empty ? o.ImageLink : String.Empty,
                    IsCurrentUser = o.Sender == user.Id ? true : false,
                    Comments = GetComments(o.Id)
                }).OrderByDescending(o => o.TimeCreated).ToList();
            }
            // Sortera på id
            foreach (var message in listMessages)
            {
                var temp = context.UserDetails.Where(u => u.Id == message.Sender).SingleOrDefault();
                if (temp != null)
                {
                    message.UserRole = context.UserCategory
                        .Where(o => o.Id == temp.SemesterId)
                        .Select(x => x.CategoryName)
                        .SingleOrDefault();

                    message.FullName = temp.FirstName + " " + temp.LastName;
                    if (message.FullName.Length < 2)
                    {
                        message.FullName = "Användare";
                    }
                }
            }

            HomeVM homeVM = new HomeVM();
            homeVM.PageId = id;
            homeVM.GroupVMList = listOfGroupVM;
            homeVM.MessageVMList = listMessages;

            return View(homeVM);
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
            var groupHandler = new GroupHandler();
            var groupList = groupHandler.GetAllGroups(context);
            var model = new MessageVM { Groups = groupList };
            return PartialView("_EditorPartial", model);
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
            if (context.Groups.Where(o => o.GroupName == message.Receiver).Count() < 1)
            {
                var newGroup = new Group();
                newGroup.GroupName = message.Receiver;
                newGroup.CreatorId = context.Users.Where(o => o.UserName == User.Identity.Name).Select(o => o.Id).SingleOrDefault();
                newGroup.IsOpen = true;
                context.Groups.Add(newGroup);
                context.SaveChanges();
                context.UserGroups.Add(new UserGroup
                {
                    GroupId = newGroup.Id,
                    UserId = newGroup.CreatorId
                });
            }
            Message newMessage = new Message();
            newMessage.Title = message.Title;
            newMessage.MessageBody = message.MessageBody;
            newMessage.OnFirstPage = message.OnFirstPage;
            newMessage.Sender = userId;
            newMessage.Receiver = message.Receiver;
            newMessage.TimeCreated = DateTime.Now;

            if (message.ImageLink != null)
            {
                newMessage.ImageLink = message.ImageLink;
            }
            context.Messages.Add(newMessage);
            var result = await context.SaveChangesAsync();
            bool isPictureSaved = await UploadMessagePicture(message.MessagePicture, newMessage.Id);

            if (result > 0)
            {
                ViewData["MessageData"] = "Meddelande sparat.";
                if (isPictureSaved)
                {
                    ViewData["MessageData"] = "Meddelande och bild sparad.";
                }
            }
            else
            {
                ViewData["MessageData"] = "Meddelande ej sparat!!";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> UploadMessagePicture(IFormFile file, int id)
        {
            var messagePicture = Path.Combine(_environment.WebRootPath, "MessagePictures");

            if (file != null)
            {
                if (file.Length > 0)
                {
                    await file.SaveAsAsync(Path.Combine(messagePicture, id.ToString() + ".jpg"));
                    ViewData["MessageStatus"] = "Ny meddelandebild uppladdad!";
                    var message = context.Messages.Where(o => o.Id == id).SingleOrDefault();
                    message.ImageLink = id.ToString() + ".jpg";
                    context.SaveChanges();
                    return true;
                }
            }
            ViewData["MessageStatus"] = "Uppladdning misslyckades!";
            return false;
        }

        public IActionResult JoinGroup()
        {
            var user = context.Users.Where(x => x.UserName == User.Identity.Name).SingleOrDefault();
            var groupHandler = new GroupHandler();
            var groupVMList = groupHandler.GetAllGroupVMs(context, user.Id);
            return PartialView("_GroupPartial", groupVMList);
        }

        [HttpPost]
        public async Task<IActionResult> JoinGroup(int id)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var userId = await userManager.GetUserIdAsync(user);
            context.UserGroups.Add(new UserGroup
            {
                UserId = userId,
                GroupId = id
            });
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult RemoveMessage(int id, int groupId)
        {
            var message = context.Messages.Where(o => o.Id == id).SingleOrDefault();
            var user = context.Users.Where(o => o.UserName == User.Identity.Name).SingleOrDefault();

            if (User.IsInRole("Admin") || message.Sender == user.Id)
            {
                context.Messages.Remove(message);
                context.SaveChanges();
            }

            //Just nu kommer man till index, vi vill komma till den fliken vi var i.
            return RedirectToAction("index", new { id = groupId });
        }

        [HttpPost]
        public async Task<IActionResult> EditMessage(MessageVM message, int id)
        {
            if (!ModelState.IsValid)
            {
                return Content("false");
            }

            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var userId = await userManager.GetUserIdAsync(user);

            if (context.Groups.Where(o => o.GroupName == message.Receiver).Count() < 1)
            {
                var newGroup = new Group();
                newGroup.GroupName = message.Receiver;
                newGroup.CreatorId = context.Users.Where(o => o.UserName == User.Identity.Name).Select(o => o.Id).SingleOrDefault();
                newGroup.IsOpen = true;
                context.Groups.Add(newGroup);
                context.SaveChanges();
                context.UserGroups.Add(new UserGroup
                {
                    GroupId = newGroup.Id,
                    UserId = newGroup.CreatorId
                });
            }

            var oldMessage = context.Messages.Where(o => o.Id == id).SingleOrDefault();

            oldMessage.Title = message.Title;
            oldMessage.MessageBody = message.MessageBody;
            oldMessage.OnFirstPage = message.OnFirstPage;
            oldMessage.Receiver = message.Receiver;

            context.Messages.Update(oldMessage);
            var result = await context.SaveChangesAsync();
            bool isPictureSaved = await UploadMessagePicture(message.MessagePicture, oldMessage.Id);

            if (result > 0)
            {
                ViewData["MessageData"] = "Meddelande sparat.";
                if (isPictureSaved)
                {
                    ViewData["MessageData"] = "Meddelande och bild sparad.";
                }
            }
            else
            {
                ViewData["MessageData"] = "Meddelande ej sparat!!";
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult EditMessage(int id)
        {
            var message = context.Messages.Where(o => o.Id == id).SingleOrDefault();
            MessageVM messageContains = new MessageVM
            {
                Title = message.Title,
                MessageBody = message.MessageBody,
                OnFirstPage = message.OnFirstPage,
                IsEdit = true,
                Receiver = message.Receiver,
                Groups = context.Groups.Select(o => o.GroupName).ToList(),
                Id = id
            };

            var user = context.Users.Where(o => o.UserName == User.Identity.Name).SingleOrDefault();

            if (message.Sender == user.Id)
            {
                return PartialView("_EditorPartial", messageContains);
            }

            //Just nu kommer man till index, vi vill komma till den fliken vi var i.
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Chat()
        {
            var user = context.Users.Where(o => o.UserName == User.Identity.Name).SingleOrDefault();
            var model = context.UserDetails.Where(o => o.Id == user.Id).SingleOrDefault();
            ChatUserVM chatUser = new ChatUserVM();

            chatUser.TimeStamp = DateTime.Now.ToString("HH:mm");
            chatUser.Fullname = model.FirstName + " " + model.LastName;

            if (chatUser.Fullname.Length < 2)
            {
                chatUser.Fullname = user.UserName;
            }

            return View(chatUser);
        }

        public async Task<IActionResult> PostComment(string commentBody, int id)
        {

            //if (!ModelState.IsValid)
            //{
            //    return Content("false");
            //}
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var userId = await userManager.GetUserIdAsync(user);

            Comment newComment = new Comment();
            newComment.SenderId = user.Id;
            newComment.CommentBody = commentBody;
            newComment.PostId = id;
            newComment.TimeStamp = DateTime.Now;

            context.Comments.Add(newComment);
            var result = await context.SaveChangesAsync();

            if (result > 0)
            {
                ViewData["MessageData"] = "Kommentar sparad.";
            }
            else
            {
                ViewData["MessageData"] = "Kommentar ej sparad!";
            }

            return PartialView("_CommentPartial", GetComments(id));
        }

        public CommentsVM GetComments(int messageId)
        {
            var comments = context.Comments.Where(o => o.PostId == messageId).ToList();
            var commentsVM = new CommentsVM();
            commentsVM.CommentList = new List<CommentVM>();
            commentsVM.ParentMessageId = messageId;

            foreach (var comment in comments)
            {
                var tmp = new CommentVM();
                tmp.CommentBody = comment.CommentBody;
                tmp.SenderName = context.UserDetails.Where(o => o.Id == comment.SenderId).Select(x => x.FirstName + " " + x.LastName).SingleOrDefault();
                tmp.TimeStamp = comment.TimeStamp;
                commentsVM.CommentList.Add(tmp);
            }
            return commentsVM;
        }
        public async Task<IActionResult> removeUserFromGroup(int id)
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            GroupHandler groupHandler = new GroupHandler();
            groupHandler.RemoveFromGroup(context, user.Id, id);
            return RedirectToAction(nameof(Index));
        }
    }
}
