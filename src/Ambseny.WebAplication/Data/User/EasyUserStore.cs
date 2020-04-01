
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
        private readonly AmbsenyIdentityErrorDescriber errorDescriber;

        public EasyUserStore(EasyUserDbContext dbContext, AmbsenyIdentityErrorDescriber errorDescriber)
        {
            this.dbContext = dbContext;
            this.errorDescriber = errorDescriber;
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
                
                return IdentityResult.Failed(errorDescriber.UnableToPersistNewUser(user.Name));
                

            }
           
            return IdentityResult.Failed(errorDescriber.InvalidUserName(user.Name)); 
        }

        public async Task<IdentityResult> DeleteAsync(EasyUser user, CancellationToken cancellationToken)
        {
            var result = await FindByNameAsync(user.NormalizedName, cancellationToken);
            if(result!=null)
            {
                dbContext.Remove(result);
                var count = await dbContext.SaveChangesAsync();
                if(count > 0)
                {
                    return IdentityResult.Success;
                }
                return IdentityResult.Failed(errorDescriber.UnableToDeleteNewUser(user.Name));
            }
            return IdentityResult.Failed(errorDescriber.NonExistingUser(user.Name));
        }

        public void Dispose()
        {
            //throw new System.NotImplementedException();
        }

        public Task<EasyUser> FindByIdAsync(string userId, CancellationToken cancellationToken) =>
            Task.Factory.StartNew(() =>
            {
                if (dbContext.Users.Any())
                {
                    var matches = dbContext.Users.Where(x => x.Id == userId).AsEnumerable();
                    if (matches.Any())
                    {
                        return matches.First();
                    }
                }
                return default(EasyUser);
            });

        
        public Task<EasyUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken) => 
            Task.Factory.StartNew(() => dbContext.Users
                .Where(x => x.NormalizedName == normalizedUserName)
                .FirstOrDefault());

        public Task<string> GetNormalizedUserNameAsync(EasyUser user, CancellationToken cancellationToken) =>
            Task.FromResult(user.NormalizedName);

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

        public Task SetNormalizedUserNameAsync(EasyUser user, string normalizedName, CancellationToken cancellationToken) =>
            Task.FromResult(user.NormalizedName = normalizedName);

        public Task SetUserNameAsync(EasyUser user, string userName, CancellationToken cancellationToken) =>
            Task.FromResult(user.Name = userName);

        public Task<IdentityResult> UpdateAsync(EasyUser user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
        
    }
}