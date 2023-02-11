using Kontakt.DAL;
using Kontakt.Helpers;
using Kontakt.Models;
using Kontakt.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontakt
{
    public class Startup
    {
        private readonly IConfiguration _config;

        public Startup(IConfiguration config)
        {
            _config = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddIdentity<AppUser, IdentityRole>(identityOptions =>
            {
                identityOptions.Password.RequiredLength = 8;
                identityOptions.Password.RequireDigit = true;
                identityOptions.Password.RequireLowercase = true;
                identityOptions.Password.RequireUppercase = true;
                identityOptions.Password.RequireNonAlphanumeric = true;

                identityOptions.User.RequireUniqueEmail = true;

                identityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                identityOptions.Lockout.MaxFailedAccessAttempts = 3;
                identityOptions.Lockout.AllowedForNewUsers = true;
            }).AddDefaultTokenProviders()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddErrorDescriber<AzIdentityErrorDesc>();

            services.AddDbContext<AppDbContext>(options => {

                options.UseSqlServer(_config["ConnectionStrings:Default"]);
            });
            services.AddScoped<LayoutService>();
            services.AddHttpContextAccessor();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=account}/{action=Login}/{id?}"
                );
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}"

                    );
            });
        }
    }
}
