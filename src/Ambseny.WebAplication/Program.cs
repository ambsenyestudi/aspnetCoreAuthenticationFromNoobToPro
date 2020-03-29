using Ambseny.WebAplication.Data;
using Ambseny.WebAplication.Models.Users;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Security.Claims;

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
            var bob = new EasyUser
            {
                Id = Guid.NewGuid().ToString(),
                Name = "bob",
                Password = "bob",
            };
            dbContext.Users.AddRange(bob);
            var claims = new List<UserClaim>
            {
                
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
                }
            };
            dbContext.UserClaims.AddRange(claims);
            dbContext.SaveChanges();
        }

    }
}
