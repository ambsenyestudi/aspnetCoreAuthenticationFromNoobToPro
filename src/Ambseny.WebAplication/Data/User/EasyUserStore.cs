
using Ambseny.WebAplication.Models.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Ambseny.WebAplication.Data.User
{
    public class EasyUserStore : IUserStore<EasyUser>, IUserPasswordStore<EasyUser>, IUserClaimStore<EasyUser>
    {
        private readonly EasyUserDbContext dbContext;
        private readonly AmbsenyIdentityErrorDescriber errorDescriber;

        public EasyUserStore(EasyUserDbContext dbContext, AmbsenyIdentityErrorDescriber errorDescriber)
        {
            this.dbContext = dbContext;
            this.errorDescriber = errorDescriber;
        }

        public async Task AddClaimsAsync(EasyUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            var validatedUser = await ProcessUserValidationAsync(user, cancellationToken);

            var newClaimCollection = claims.Select(x => new UserClaim
            {
                ClaimType = x.Type,
                ClaimValue = x.Value,
                Id = Guid.NewGuid(),
                UserId = validatedUser.Id
            });
            await dbContext.UserClaims.AddRangeAsync(newClaimCollection);
            await dbContext.SaveChangesAsync();

        }

        public async Task<IdentityResult> CreateAsync(EasyUser user, CancellationToken cancellationToken)
        {
            if (!dbContext.Users.Any(x => x.NormalizedName == user.NormalizedName))
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

        public async Task<IList<Claim>> GetClaimsAsync(EasyUser user, CancellationToken cancellationToken)
        {
            var validatedUser = await ProcessUserValidationAsync(user, cancellationToken);

            return dbContext.UserClaims
                .Where(x => x.UserId == user.Id)
                .Select(x => new Claim(x.ClaimType, x.ClaimValue))
                .ToList();
        }

        public Task<string> GetNormalizedUserNameAsync(EasyUser user, CancellationToken cancellationToken) =>
            Task.FromResult(user.NormalizedName);

        public Task<string> GetPasswordHashAsync(EasyUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUserIdAsync(EasyUser user, CancellationToken cancellationToken) =>
            Task.Factory.StartNew(() =>
            {
                if (dbContext.Users.Any())
                {
                    var matches = dbContext.Users.Where(x => x.NormalizedName == user.NormalizedName);
                    if (matches.Any())
                    {
                        return matches.First().Id;
                    }
                }
                return string.Empty;
            });
            
        

        public Task<string> GetUserNameAsync(EasyUser user, CancellationToken cancellationToken) =>
            Task.FromResult(user.Name);

        public Task<IList<EasyUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasPasswordAsync(EasyUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveClaimsAsync(EasyUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task ReplaceClaimAsync(EasyUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {

            var userClaimCollection = dbContext.UserClaims.Where(x => x.UserId.ToString() == user.Id).AsEnumerable();
            var oldClaim = userClaimCollection.Where(x => x.ClaimType == claim.Type).FirstOrDefault();
            if(oldClaim!=null)
            {
                oldClaim.ClaimType = newClaim.Type;
                oldClaim.ClaimValue = newClaim.Value;
                dbContext.UserClaims.Update(oldClaim);
                await dbContext.SaveChangesAsync();
            }
            
        }

        public Task SetNormalizedUserNameAsync(EasyUser user, string normalizedName, CancellationToken cancellationToken) =>
            Task.FromResult(user.NormalizedName = normalizedName);

        public Task SetPasswordHashAsync(EasyUser user, string passwordHash, CancellationToken cancellationToken) =>
            Task.FromResult(user.PasswordHash = passwordHash);

        public Task SetUserNameAsync(EasyUser user, string userName, CancellationToken cancellationToken) =>
            Task.FromResult(user.Name = userName);

        public async Task<IdentityResult> UpdateAsync(EasyUser user, CancellationToken cancellationToken)
        {
            var storedUser = await FindByIdAsync(user.Id, cancellationToken);
            storedUser.Name = user.Name;
            storedUser.NormalizedName = user.NormalizedName;
            storedUser.PasswordHash = user.PasswordHash;
            dbContext.Users.Update(storedUser);
            var updates = await dbContext.SaveChangesAsync();
            if(updates>0)
            {
                return IdentityResult.Success;
            }
            var des = new AmbsenyIdentityErrorDescriber();
            return IdentityResult.Failed(des.UnableToPersistNewUser(user.NormalizedName));
        }
        private async Task<EasyUser> ProcessUserValidationAsync(EasyUser user, CancellationToken cancellationToken)
        {
            if (!IsValidUser(user))
            {
                var normalizedName = await GetNormalizedUserNameAsync(user, cancellationToken);
                var validatedUser = await FindByNameAsync(normalizedName, cancellationToken);
                return validatedUser;
            }
            return user;
        }

        private bool IsValidUser(EasyUser user)
        {
            var isUser = dbContext.Users.Any(x => x == user);
            return isUser;
        }


        
    }
}