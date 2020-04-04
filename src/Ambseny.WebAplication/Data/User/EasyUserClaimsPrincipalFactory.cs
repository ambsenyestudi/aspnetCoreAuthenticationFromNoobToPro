using Ambseny.WebAplication.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ambseny.WebAplication.Data.User
{
    public class EasyUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<EasyUser, IdentityRole>
    {
        private readonly EasyUserManager userManager;

        public EasyUserClaimsPrincipalFactory(
            EasyUserManager userManager, 
            RoleManager<IdentityRole> roleManager, 
            IOptions<IdentityOptions> options) : base(userManager, roleManager, options)
        {
            this.userManager = userManager;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(EasyUser user)
        {
            var storedUser = await userManager.FindByNameAsync(user.NormalizedName);
            var identity = await base.GenerateClaimsAsync(user);
            var aditionalClaims = await UserManager.GetClaimsAsync(storedUser);
            foreach (var claim in aditionalClaims)
            {
                identity.AddClaim(claim);
            }
            return identity;
        }
        public override async Task<ClaimsPrincipal> CreateAsync(EasyUser user)
        {
            var identity = new ClaimsIdentity[] { await GenerateClaimsAsync(user) };
            var principal = new ClaimsPrincipal(identity);
            return principal;
        }
    }
}
