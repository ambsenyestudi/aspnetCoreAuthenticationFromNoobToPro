using Ambseny.WebAplication.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Ambseny.WebAplication.Data.User
{
    public class EasyUserManager : UserManager<EasyUser>
    {
        public EasyUserManager(
            EasyUserStore store, 
            IOptions<IdentityOptions> optionsAccessor, 
            IPasswordHasher<EasyUser> passwordHasher, 
            IEnumerable<IUserValidator<EasyUser>> userValidators, 
            IEnumerable<IPasswordValidator<EasyUser>> passwordValidators, 
            ILookupNormalizer keyNormalizer, 
            IdentityErrorDescriber errors, 
            IServiceProvider services, 
            ILogger<UserManager<EasyUser>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            this.Store = store;
        }
        public override Task<IdentityResult> CreateAsync(EasyUser user)
        {
            return Store.CreateAsync(user, new CancellationToken());
        }
        public override Task<EasyUser> FindByNameAsync(string userName)
        {
            return Store.FindByNameAsync(userName, new CancellationToken());
        }
        
        public override async Task<bool> CheckPasswordAsync(EasyUser user, string password)
        {

            var storedUser = await FindByNameAsync(user.NormalizedName);
            if (storedUser != null)
            {
                var result = PasswordHasher.VerifyHashedPassword(storedUser, storedUser.PasswordHash, password);
                return result != PasswordVerificationResult.Failed;
            }
            return false;
        }
        public override Task<IdentityResult> AddClaimAsync(EasyUser user, Claim claim)
        {
            return base.AddClaimAsync(user, claim);
        }
        public override Task<IdentityResult> AddClaimsAsync(EasyUser user, IEnumerable<Claim> claims)
        {
            return base.AddClaimsAsync(user, claims);
        }
        public override Task<IList<Claim>> GetClaimsAsync(EasyUser user)
        {
            return base.GetClaimsAsync(user);
        }
    }
}
