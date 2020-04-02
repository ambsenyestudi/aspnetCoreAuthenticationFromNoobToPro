using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ambseny.WebAplication.Data;
using Ambseny.WebAplication.Data.User;
using Ambseny.WebAplication.Models.Users;
using Ambseny.WebAplication.Services.Claims;
using Ambseny.WebAplication.Services.Users;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Ambseny.WebAplication
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
            services.AddControllersWithViews();
            services.AddDbContext<EasyUserDbContext>(options =>
                options
                .UseInMemoryDatabase("Memory")
                //.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddTransient<EasyUserSignInManager>();
            services.AddTransient<EasyUserManager>();
            services.AddTransient<EasyUserStore>();
            services.AddTransient<AmbsenyIdentityErrorDescriber>();
            services.AddTransient<PasswordHasher<EasyUser>>();

            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IClaimsService, ClaimsService>();

            services.AddIdentity<EasyUser, IdentityRole>(config => {
                //just for the sake of rapid testing
                config.Password.RequiredLength = 3;
                config.Password.RequireDigit = false;
                config.Password.RequireNonAlphanumeric = false;
                config.Password.RequireUppercase = false;
            })
                .AddUserStore<EasyUserStore>()
                .AddRoleStore<EasyRoleStore>()
                .AddSignInManager<EasyUserSignInManager>()
                .AddErrorDescriber<AmbsenyIdentityErrorDescriber>()
                
                .AddClaimsPrincipalFactory<EasyUserClaimsPrincipalFactory>();
            //all of the following is reundand (check Password.md)
            services.AddAuthentication(config => 
            {
                config.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                .AddCookie(config => {
                    config.Cookie.Name = "IdentityAutheticate.Cookie";
                    config.LoginPath = "/Account/Login";
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Minimal", policy => policy.RequireClaim(ClaimTypes.Sid));
                options.AddPolicy("UserReviewer", policy => 
                    policy.RequireClaim(AmbsenyClaimTypes.ManageUsers.ToString(), 
                        AmbsenyManageUserClaims.Review.ToString(),
                        AmbsenyManageUserClaims.Edit.ToString(),
                        AmbsenyManageUserClaims.Administrate.ToString()
                    )
                );
                options.AddPolicy("UserEditor", policy => 
                    policy.RequireClaim(AmbsenyClaimTypes.ManageUsers.ToString(),
                        AmbsenyManageUserClaims.Edit.ToString(),
                        AmbsenyManageUserClaims.Administrate.ToString()
                   )
                );
                options.AddPolicy("UserAdministrator", policy => policy.RequireClaim(AmbsenyClaimTypes.ManageUsers.ToString(), AmbsenyManageUserClaims.Administrate.ToString()));
            });
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
