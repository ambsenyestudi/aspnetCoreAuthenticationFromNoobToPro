using Ambseny.WebAplication.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
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
            return Store.CreateAsync((EasyUser)user, new CancellationToken());
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
                if (storedUser.Password == password)
                {
                    return true;
                }
            }
            return false;
            //return base.CheckPasswordAsync(user, password);
        }
    }
}
