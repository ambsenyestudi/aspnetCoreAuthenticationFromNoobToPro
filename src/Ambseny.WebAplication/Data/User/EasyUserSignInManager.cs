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
        public async override Task SignInAsync(EasyUser user, bool isPersistent, string authenticationMethod = null)
        {
            await base.SignInAsync(user, isPersistent, authenticationMethod);
            await UpdateContextUserAsync(Context, user); 
        }

        public async override Task<SignInResult> PasswordSignInAsync(EasyUser user, string password, bool isPersistent, bool lockoutOnFailure)
        {
            var storedUser = await UserManager.FindByNameAsync(user.Name);
            if (storedUser != null)
            {
                if (storedUser.Password == password)
                {
                    await UpdateContextUserAsync(Context, storedUser);
                    return SignInResult.Success;
                }
            }
            return SignInResult.Failed;
        }
        private async Task UpdateContextUserAsync(HttpContext context, EasyUser user)
        {
            context.User = await ClaimsFactory.CreateAsync(user);
        }
    }
}
