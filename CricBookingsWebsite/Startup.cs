using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CricBookingsWebsite.Factory;
using CricBookingsWebsite.Helpers;
using CricBookingsWebsite.Models;
using CricBookingsWebsite.Models.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace CricBookingsWebsite
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
            services.AddDbContext<ApplicationContext>(opts =>
                opts.UseSqlServer(Configuration.GetConnectionString("CricBookingsDbConnection")));

            //services.AddDbContext<CricBookingsContext>(opts =>
            //    opts.UseSqlServer(Configuration.GetConnectionString("CricBookingsDbConnection")));

            //services.AddIdentity<User, IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationContext>();
            

            services.AddIdentity<User, IdentityRole>(opt =>
            {
                opt.Password.RequiredLength = 7;
                opt.Password.RequireDigit = false;
                opt.Password.RequireUppercase = false;
                opt.User.RequireUniqueEmail = true;

            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationContext>();
                

            services.AddScoped<IUserClaimsPrincipalFactory<User>, UserClaimsPrincipalFactory<User, IdentityRole>>();

            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                // options.Filters.Add(new AuthorizeFilter(policy));
            }).AddXmlSerializerFormatters();

            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Administrator/AccessDenied");
            });

            services.AddAutoMapper(typeof(Startup));

            
            //services.AddControllers(config =>
            //{
            //    var policy = new AuthorizationPolicyBuilder()
            //    .RequireAuthenticatedUser()
            //    .Build();

            //    config.Filters.Add(new AuthorizeFilter(policy));
            //});

            //services.AddAuthorization(options =>
            //{

            //    options.AddPolicy("AdminAccess",
            //        authBuilder =>
            //        {
            //            authBuilder.RequireRole("Administrators");
            //        });

            //});

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("Administrator", policy => policy.RequireClaim("Administrator"));
            //});

            //services.AddMvc(options =>
            //{
            //    var policy = new AuthorizationPolicyBuilder()
            //        .RequireRole("Administrator");

            //    options.Filters.Add(new AuthorizeFilter(policy));
            //});
            services.AddAuthentication();

            services.AddAuthorization();

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddServerSideBlazor();
            
        }

       

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapBlazorHub();
            });
            
        }
    }
}
