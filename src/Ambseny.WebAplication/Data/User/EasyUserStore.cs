
using Ambseny.WebAplication.Models.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Ambseny.WebAplication.Data.User
{
    public class EasyUserStore : IUserStore<EasyUser>
    {
        private readonly EasyUserDbContext dbContext;

        public EasyUserStore(EasyUserDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<IdentityResult> CreateAsync(EasyUser user, CancellationToken cancellationToken)
        {
            if (!dbContext.Users.Any(x => x.Name == user.Name))
            {
                user.Id = Guid.NewGuid().ToString();
                dbContext.Users.Add(user);
                var basicClaim = new UserClaim
                {
                    Id = Guid.NewGuid(),
                    ClaimType = ClaimTypes.Sid,
                    ClaimValue = user.Id,
                    UserId = user.Id
                };
                dbContext.UserClaims.Add(basicClaim);
                var result = await dbContext.SaveChangesAsync();
                if (result > 0)
                {
                    return IdentityResult.Success;
                }
                return IdentityResult.Failed(new IdentityError[] {
                    new IdentityError(){
                        Code = AmbsenyIdentityErrorDefaults.UnableToPersistNewUser.ToString(),
                        Description = AmbsenyAuthenticationDefaults.IdentityErrorDefaults[AmbsenyIdentityErrorDefaults.UnableToPersistNewUser]
                    }
                });

            }
            return IdentityResult.Failed(new IdentityError[] {
                new IdentityError(){
                    Code = AmbsenyIdentityErrorDefaults.ExistingName.ToString(),
                    Description = string.Format(AmbsenyAuthenticationDefaults.IdentityErrorDefaults[AmbsenyIdentityErrorDefaults.ExistingName], user.Name)
                }
            }); 
        }

        public Task<IdentityResult> DeleteAsync(EasyUser user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public Task<EasyUser> FindByIdAsync(string userId, CancellationToken cancellationToken) =>
            Task.Factory.StartNew(() =>
            {
                if (dbContext.Users.Any())
                {
                    var matches = dbContext.Users.Where(x => x.Id == userId);
                    if (dbContext.Users.Any())
                    {
                        return matches.First();
                    }
                }
                return default(EasyUser);
            });

        public Task<EasyUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken) => 
            Task.Factory.StartNew(() => dbContext.Users
                .Where(x => x.Name.ToUpper() == normalizedUserName.ToUpper())
                .FirstOrDefault());

        public Task<string> GetNormalizedUserNameAsync(EasyUser user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetUserIdAsync(EasyUser user, CancellationToken cancellationToken) =>
            Task.Factory.StartNew(() =>
            {
                if (dbContext.Users.Any())
                {
                    var matches = dbContext.Users.Where(x => x.Name == user.Name && x.Password == x.Password);
                    if (dbContext.Users.Any())
                    {
                        return matches.First().Id;
                    }
                }
                return string.Empty;
            });
            
        

        public Task<string> GetUserNameAsync(EasyUser user, CancellationToken cancellationToken) =>
            Task.FromResult(user.Name);

        public Task SetNormalizedUserNameAsync(EasyUser user, string normalizedName, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task SetUserNameAsync(EasyUser user, string userName, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(EasyUser user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
        
    }
}