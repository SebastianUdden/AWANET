using AWANET.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AWANET.Models
{
    public class ContactList
    {
        public ContactListVM[] GetAllContacts(AWAnetContext context, UserManager<IdentityUser> userManager)
        {
            try
            {
                var userList = context.Users.Select(o => new ContactListVM
                {
                    Id = o.Id,
                    EMail = o.UserName //Email
                }).ToArray();

                foreach (var u in userList)
                {
                    var user = context.UserDetails.Where(o => o.Id == u.Id).SingleOrDefault();
                    if (user != null)
                    {
                        u.FirstName = user.FirstName != null ? user.FirstName : String.Empty;
                        u.LastName = user.LastName != null ? user.LastName : String.Empty;
                        u.Phone = user.Phone != null ? user.Phone : String.Empty;
                    }
                }
                return userList;
            }
            catch (Exception)
            {
                return new ContactListVM[0];
            }
        }

        public ContactInfoVM GetContact(string email, string userId, AWAnetContext context)
        {
            ContactInfoVM user = new ContactInfoVM
            {
                EMail = email,
            };
            var x = context.UserDetails.Where(o => o.Id == userId).SingleOrDefault();
            if (x != null)
            {
                user.FirstName = x.FirstName != null ? x.FirstName : "Förnamn ej ifyllt";
                user.LastName = x.LastName != null ? x.LastName : "Efternamn ej ifyllt";
                user.Phone = x.Phone != null ? x.Phone : "Telefonnumer ej ifyllt";
                user.Street = x.Street != null ? x.Street : "Gata ej ifyllt";
                user.Zip = x.Zip != null ? x.Zip : "Postnummer ej ifyllt";
                user.City = x.City != null ? x.City : "Stad ej ifyllt";
                //Lägg till Bild senare!!!!!!!!!!
            }
            return user;
        }
    }
}
