using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CSMWebCore.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using CSMWebCore.Services;
using CSMWebCore.Entities;

namespace CSMWebCore
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true);
            //The connection string is stored in a json object called secrets.  This keeps it independent of version control
            //the secrets json is only used if environment is development
            //if (env.IsDevelopment())
            //    builder.AddUserSecrets<Startup>();
            Configuration = builder.Build();

        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            //connects the database ChipsDbContext to the connection string stored in the with the value matching the
            //DefaultConnect key in a JSON obejct
            services.AddDbContext<ChipsDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            //this adds the service to access Customer data through the SqlCustomerData object which implements the
            //ICusomerData Interface.  Add scoped must be used in order for services to work with EF
            services.AddScoped<ICustomerData, SqlCustomerData>();
            services.AddScoped<IDeviceData, SqlDeviceData>();
            services.AddScoped<ILogData, SqlLogData>();
            services.AddScoped<ITicketData, SqlTicketData>();
            services.AddScoped<IUpdateData, SqlUpdateData>();

            //Adds Identity services using the DBFramework.  This also allows for dependency injection for User
            services.AddDefaultIdentity<ChipsUser>().AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<ChipsDbContext>();
            //This line allows to check the User object inside of a view 
            services.AddScoped<IUserClaimsPrincipalFactory<ChipsUser>, UserClaimsPrincipalFactory<ChipsUser, IdentityRole>>();

            //This line of code routes my default to the login page
            //services.AddMvc().AddRazorPagesOptions(options => {
            //    options.Conventions.AddAreaPageRoute("Identity", "/Account/Login", "");
            //}).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();

            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
