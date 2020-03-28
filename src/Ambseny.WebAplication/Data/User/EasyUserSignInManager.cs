using Ambseny.WebAplication.Models.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ambseny.WebAplication.Data.User
{
    public class EasyUserSignInManager : SignInManager<EasyUser>
    {
        public EasyUserSignInManager(UserManager<EasyUser> userManager, 
            IHttpContextAccessor contextAccessor, 
            IUserClaimsPrincipalFactory<EasyUser> claimsFactory, 
            IOptions<IdentityOptions> optionsAccessor, 
            ILogger<SignInManager<EasyUser>> logger, 
            IAuthenticationSchemeProvider schemes, 
            IUserConfirmation<EasyUser> confirmation) : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {
        }
        public override Task SignInAsync(EasyUser user, bool isPersistent, string authenticationMethod = null) =>
            base.SignInAsync(user, isPersistent, authenticationMethod);

        public async override Task<SignInResult> PasswordSignInAsync(EasyUser user, string password, bool isPersistent, bool lockoutOnFailure)
        {
            var storedUser = await UserManager.FindByNameAsync(user.Name);
            if (storedUser != null)
            {
                if (storedUser.Password == password)
                {
                    user = storedUser;
                    
                    return SignInResult.Success;
                }
            }
            return SignInResult.Failed;
        }
    }
}
