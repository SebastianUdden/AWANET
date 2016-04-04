using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.AspNet.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using AWANET.Models;
using Owin;

namespace AWANET
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Vår connection-string till Azure-databasen. 
            const string connString = "Server=tcp:oscarii.database.windows.net,1433;Database=AWANET;User ID=awanet@oscarii;Password=awa2016!;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;MultipleActiveResultSets=True";

            // Lägger till Mvc, Entity och Identity
            services.AddMvc();
            services.AddEntityFramework().AddSqlServer().AddDbContext<AWAnetContext>(o => o.UseSqlServer(connString));
            // Identity kopplar upp sig mot en databas, skapar nya tabeller om det inte finns tidigare. 
            // - IdentityUser är användaren, skickas in och säger vad tabellen ska innehålla. IdentityRole är ?
            // - AddEntityFrameworkStores talar om var de ska finnas persisterade, Context.
            // - AddDefaultTokenProviders, Tokens är något användaren får tillbaka som en "biljett" för att kommunicera och få tillåtelse att ändra Identity.
            services.AddIdentity<IdentityUser, IdentityRole>(o =>
            {
                o.Password.RequiredLength = 6;
                o.Password.RequireNonLetterOrDigit = false;                
            } ).AddEntityFrameworkStores<AWAnetContext>().AddDefaultTokenProviders();

            //services.AddTransient<IUserRepository, DbUserRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            // UseStaticFiles behövs för statiska filer i wwwwroot
            app.UseStaticFiles();
            // CookieAuthentication skapa cookies...
            app.UseIdentity().UseCookieAuthentication(o => o.AutomaticChallenge = true); 
            app.UseDeveloperExceptionPage();
            app.UseMvcWithDefaultRoute();
        }

        // Entry point for the application.
        public static void Main(string[] args) => WebApplication.Run<Startup>(args);
    }
}
