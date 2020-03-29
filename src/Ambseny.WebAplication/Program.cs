using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Ambseny.WebAplication.Data;
using Ambseny.WebAplication.Data.User;
using Ambseny.WebAplication.Models.Users;
using Microsoft.AspNetCore.Hosting;
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
                Seed(dbContext);
                
            }
        }

        private static void Seed(EasyUserDbContext dbContext)
        {
            var admin = new EasyUser
            {
                Id = Guid.NewGuid().ToString(),
                Name = "admin",
                Password = "admin",
            };
            var bob = new EasyUser
            {
                Id = Guid.NewGuid().ToString(),
                Name = "bob",
                Password = "bob",
            };
            var alice = new EasyUser
            {
                Id = Guid.NewGuid().ToString(),
                Name = "alice",
                Password = "alice",
            };
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
