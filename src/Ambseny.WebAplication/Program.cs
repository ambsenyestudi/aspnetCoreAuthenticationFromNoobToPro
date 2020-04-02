using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ambseny.WebAplication.Data;
using Ambseny.WebAplication.Data.User;
using Ambseny.WebAplication.Models.Users;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Ambseny.WebAplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            Seed(host);
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static void Seed(IHost host)
        {
            using(var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var dbContext = serviceProvider.GetRequiredService<EasyUserDbContext>();
                var passwordHasher = serviceProvider.GetRequiredService<PasswordHasher<EasyUser>>();
                Seed(dbContext, passwordHasher);
                
            }
        }

        private static void Seed(EasyUserDbContext dbContext, PasswordHasher<EasyUser> passwordHasher)
        {
            var admin = new EasyUser
            {
                Id = Guid.NewGuid().ToString(),
                NormalizedName = "admin".ToUpper(),
                Name = "admin"
            };
            admin.PasswordHash = passwordHasher.HashPassword(admin, "admin");
            var bob = new EasyUser
            {
                Id = Guid.NewGuid().ToString(),
                NormalizedName = "bob".ToUpper(),
                Name = "bob"
            };
            bob.PasswordHash = passwordHasher.HashPassword(bob, "bob");
            var alice = new EasyUser
            {
                Id = Guid.NewGuid().ToString(),
                NormalizedName = "alice".ToUpper(),
                Name = "alice",
            };
            alice.PasswordHash = passwordHasher.HashPassword(alice, "alice");
            dbContext.Users.AddRange(admin, alice, bob);
            var claims = new List<UserClaim>
            {
                new UserClaim
                {
                    Id = Guid.NewGuid(),
                    UserId = admin.Id,
                    ClaimType = ClaimTypes.Sid,
                    ClaimValue = admin.Id
                },
                new UserClaim
                {
                     Id = Guid.NewGuid(),
                    UserId = admin.Id,
                    ClaimType = AmbsenyClaimTypes.ManageUsers.ToString(),
                    ClaimValue = AmbsenyManageUserClaims.Administrate.ToString()
                },
                new UserClaim
                {
                    Id = Guid.NewGuid(),
                    UserId = bob.Id,
                    ClaimType = ClaimTypes.Sid,
                    ClaimValue = bob.Id
                },
                new UserClaim
                {
                    Id = Guid.NewGuid(),
                    UserId = bob.Id,
                    ClaimType = AmbsenyClaimTypes.ManageUsers.ToString(),
                    ClaimValue = AmbsenyManageUserClaims.Review.ToString()
                },
                new UserClaim
                {
                    Id = Guid.NewGuid(),
                    UserId = alice.Id,
                    ClaimType = ClaimTypes.Sid,
                    ClaimValue = alice.Id
                },
                new UserClaim
                {
                    Id = Guid.NewGuid(),
                    UserId = alice.Id,
                    ClaimType = AmbsenyClaimTypes.ManageUsers.ToString(),
                    ClaimValue = AmbsenyManageUserClaims.Edit.ToString()
                }
            };
            dbContext.UserClaims.AddRange(claims);
            dbContext.SaveChanges();
        }

    }
}
