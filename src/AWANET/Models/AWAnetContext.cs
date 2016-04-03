using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AWANET.Models
{
    public class AWAnetContext : IdentityDbContext
    {
        public DbSet<UserDetail> UserDetails { get; set; }
        public DbSet<UserCategory> UserCategory { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserDetail>().ToTable("UserInformation");
            builder.Entity<UserCategory>().ToTable("UserCategory");
            builder.Entity<Message>().ToTable("Messages");
        }
    }
}
