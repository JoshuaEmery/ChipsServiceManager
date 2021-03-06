﻿using CSMWebCore.Data;
using CSMWebCore.Entities;
using CSMWebCore.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;


namespace CSMWebCore
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IWebHostEnvironment env)
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
            //this adds the service to access Customer data through the CustomerRepository object which implements the
            //ICusomerData Interface.  Add scoped must be used in order for services to work with EF
            services.AddScoped<IUpdateData, SqlUpdateData>();
            services.AddScoped<ITicketsHistoryData, SqlTicketsHistoryData>();
            services.AddScoped<ITicketCreator, TicketCreator>();
            services.AddScoped<IReportsService, ReportsService>();
            //Adds Identity services using the DBFramework.  This also allows for dependency injection for User
            services.AddDefaultIdentity<ChipsUser>().AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<ChipsDbContext>();
            //This line allows to check the User object inside of a view 
            services.AddScoped<IUserClaimsPrincipalFactory<ChipsUser>, UserClaimsPrincipalFactory<ChipsUser, IdentityRole>>();

            //This line of code routes my default to the login page
            //services.AddMvc().AddRazorPagesOptions(options => {
            //    options.Conventions.AddAreaPageRoute("Identity", "/Account/Login", "");
            //}).SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddRazorPages();
            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(Configuration);

            // add RuntimeCompilation package so changes in .cshtml pages update without needing to restart project
            services.AddMvc().AddRazorRuntimeCompilation();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<ChipsUser> userManager)
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

            // seed initial admin user
            ChipsDbInitializer.SeedUsers(userManager);

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapDefaultControllerRoute();
                // method above acccomlishes same as below for .NET 3.1
                //.MapRoute(
                //    name: "default",
                //    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}