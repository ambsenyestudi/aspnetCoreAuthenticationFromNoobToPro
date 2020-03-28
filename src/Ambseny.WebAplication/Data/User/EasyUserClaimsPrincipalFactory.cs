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

        public EasyUserClaimsPrincipalFactory(
            UserManager<EasyUser> userManager, 
            RoleManager<IdentityRole> roleManager, 
            IOptions<IdentityOptions> options,
            EasyUserDbContext dbContext) : base(userManager, roleManager, options)
        {
            this.dbContext = dbContext;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(EasyUser user)
        {
            var userId = user.Id;
            var userClaims = dbContext.UserClaims.Where(x => x.UserId == user.Id);
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
