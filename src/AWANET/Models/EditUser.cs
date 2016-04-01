using AWANET.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AWANET.Models
{
    public class EditUser
    {
        AWAnetContext context;
        public EditUser(AWAnetContext context)
        {
            this.context = context;
        }

        public void UpdateUserDetails(EditContactDetailsVM model, string userId)
        {
            try
            {
                var selectedUser = context.UserDetails
                    .Where(o => o.Id == userId)
                    .SingleOrDefault();
                if (selectedUser != null)
                {
                    selectedUser.FirstName = model.FirstName;
                    selectedUser.LastName = model.LastName;
                    selectedUser.Phone = model.Phone;
                    selectedUser.Street = model.Street;
                    selectedUser.Zip = model.Zip;
                    selectedUser.City = model.City;
                    context.SaveChanges();
                }
                else
                {

                    context.UserDetails.Add(new UserDetail
                    {
                        Id = userId,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Phone = model.Phone,
                        Street = model.Street,
                        Zip = model.Zip,
                        City = model.City
                    });
                    context.SaveChanges();
                }

            }
            catch (Exception e)
            {
                 
            }
            
            
                
        }
        public EditContactDetailsVM GetUser(string userId)
        {
            var selectedUser =context.UserDetails
                    .Where(o => o.Id == userId)
                    .SingleOrDefault();
            if (selectedUser != null)
            {
                return new EditContactDetailsVM
                {
                    FirstName = selectedUser.FirstName,
                    LastName = selectedUser.LastName,
                    City = selectedUser.City,
                    Phone = selectedUser.Phone,
                    Street = selectedUser.Street,
                    Zip = selectedUser.Zip
                };   
            }
            return new EditContactDetailsVM();
    
        }           
        public void AddUserDetails(EditContactDetailsVM model, IdentityUser user)
        {

        }
    }
}
