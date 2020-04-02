using Ambseny.WebAplication.Models.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ambseny.WebAplication.Data.User
{
    public class EasyUserSignInManager : SignInManager<EasyUser>
    {
        private readonly UserManager<EasyUser> userManager;
        public EasyUserSignInManager(EasyUserManager userManager, 
            IHttpContextAccessor contextAccessor, 
            IUserClaimsPrincipalFactory<EasyUser> claimsFactory, 
            IOptions<IdentityOptions> optionsAccessor, 
            ILogger<SignInManager<EasyUser>> logger, 
            IAuthenticationSchemeProvider schemes, 
            IUserConfirmation<EasyUser> confirmation) : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
        {
            this.userManager = userManager;
        }
        public override Task SignInAsync(EasyUser user, bool isPersistent, string authenticationMethod = null) =>
            SignInAsync(user, new AuthenticationProperties { IsPersistent = isPersistent }, authenticationMethod);

        public async override Task SignInAsync(EasyUser user, AuthenticationProperties authenticationProperties, string authenticationMethod = null)
        {
            var userPrincipal = await ClaimsFactory.CreateAsync(user);
            if (authenticationMethod != null)
            {
                userPrincipal.Identities.First().AddClaim(new Claim(ClaimTypes.AuthenticationMethod, authenticationMethod));
            }
            await Context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,//IdentityConstants.ApplicationScheme,
            userPrincipal,
                authenticationProperties ?? new AuthenticationProperties());
        }

        public async override Task<SignInResult> PasswordSignInAsync(EasyUser user, string password, bool isPersistent, bool lockoutOnFailure)
        {
            if(await userManager.CheckPasswordAsync(user, password))
            {
                await SignInAsync(user, new AuthenticationProperties { IsPersistent = isPersistent });
                return SignInResult.Success;
            }
            return SignInResult.Failed;
            
            
        }
    }
}
