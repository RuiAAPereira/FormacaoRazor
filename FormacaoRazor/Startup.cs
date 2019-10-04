using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FormacaoRazor.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartBreadcrumbs.Extensions;
using System.Globalization;
using FormacaoRazor.Areas.Identity.Models;
using Microsoft.AspNetCore.Localization;

namespace FormacaoRazor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _ = services.Configure<RequestLocalizationOptions>(options =>
              {
                  options.DefaultRequestCulture = new RequestCulture("pt-PT");
                  options.SupportedCultures = new List<CultureInfo> { new CultureInfo("pt-PT") };
                  options.RequestCultureProviders.Clear();
              });

            _ = services.Configure<CookiePolicyOptions>(options =>
              {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                  options.MinimumSameSitePolicy = SameSiteMode.None;
              });

            _ = services.AddDbContext<ApplicationDbContext>(options =>
                  options.UseSqlServer(
                      Configuration.GetConnectionString("DefaultConnection")));

            _ = services.AddDefaultIdentity<ApplicationUser>()
                .AddRoles<ApplicationRole>()
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            //configuração de segurança "login"
            _ = services.Configure<IdentityOptions>(options =>
              {
                // Password settings.
                options.Password.RequireDigit = true;
                  options.Password.RequiredLength = 6;
                  options.Password.RequireNonAlphanumeric = false;
                  options.Password.RequireUppercase = false;
                  options.Password.RequireLowercase = false;
                  options.Password.RequiredUniqueChars = 4;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                  options.Lockout.MaxFailedAccessAttempts = 10;
                  options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                  "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                  options.User.RequireUniqueEmail = true;
              });

            _ = services.ConfigureApplicationCookie(options =>
              {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                  options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                  options.LoginPath = "/Identity/Account/Login";
                  options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                  options.SlidingExpiration = true;
              });

            _ = services.AddMvc()
                .AddRazorPagesOptions(options =>
                {
                    _ = options.Conventions.AuthorizeFolder("/");
                    _ = options.Conventions.AuthorizeAreaPage("Identity", "/Account/Register", "RequireAdminRights");
                    _ = options.Conventions.AuthorizeAreaFolder("Identity", "/Admin", "RequireAdminRights");
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddBreadcrumbs(GetType().Assembly, options =>
            {
                options.TagName = "nav";
                options.TagClasses = "";
                options.OlClasses = "breadcrumb";
                options.LiClasses = "breadcrumb-item";
                options.ActiveLiClasses = "breadcrumb-item active";
                options.SeparatorElement = "<li class=\"separator\">/</li>";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationDbContext context, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            if (env.IsDevelopment())
            {
                _ = app.UseDeveloperExceptionPage();
                _ = app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            _ = new[] { new CultureInfo("pt-PT") };
            _ = app.UseRequestLocalization(new RequestLocalizationOptions
            {
                SupportedCultures = new List<CultureInfo> { new CultureInfo("pt-PT") },
                SupportedUICultures = new List<CultureInfo> { new CultureInfo("pt-PT") },
                DefaultRequestCulture = new RequestCulture("pt-PT")
            });

            _ = app.UseHttpsRedirection();
            _ = app.UseStaticFiles();

            _ = app.UseAuthentication();

            _ = app.UseMvc();

            _ = app.UseCookiePolicy();

            InitialUserData.Initialize(context, userManager, roleManager).Wait();
        }
    }
}
