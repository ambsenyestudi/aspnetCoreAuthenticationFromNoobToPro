using Ambseny.WebAplication.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ambseny.WebAplication.Data.User
{
    public class EasyUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<EasyUser, IdentityRole>
    {
        private readonly EasyUserDbContext dbContext;
        private readonly EasyUserManager userManager;

        public EasyUserClaimsPrincipalFactory(
            EasyUserManager userManager, 
            RoleManager<IdentityRole> roleManager, 
            IOptions<IdentityOptions> options,
            EasyUserDbContext dbContext) : base(userManager, roleManager, options)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(EasyUser user)
        {
            var storedUser = await userManager.FindByNameAsync(user.NormalizedName);
            var userClaims = dbContext.UserClaims.Where(x => x.UserId == storedUser.Id);
            if(userClaims.Any())
            {
                var identity = await base.GenerateClaimsAsync(user);
                foreach (var claim in userClaims)
                {
                    identity.AddClaim(new Claim(claim.ClaimType, claim.ClaimValue));
                }
                return identity;
            }
            return null;
        }
        public override async Task<ClaimsPrincipal> CreateAsync(EasyUser user)
        {
            var identity = new ClaimsIdentity[] { await GenerateClaimsAsync(user) };
            var principal = new ClaimsPrincipal(identity);
            return principal;
        }
    }
}
