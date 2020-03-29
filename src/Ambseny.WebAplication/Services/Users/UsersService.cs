using Ambseny.WebAplication.Data;
using Ambseny.WebAplication.Models.Users;
using Ambseny.WebAplication.Services.Claims;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Ambseny.WebAplication.Services.Users
{
    public class UsersService : IUsersService
    {
        private readonly EasyUserDbContext dbContext;
        private readonly IClaimsService claimsService;

        public UsersService(EasyUserDbContext dbContext, IClaimsService claimsService)
        {
            this.dbContext = dbContext;
            this.claimsService = claimsService;
        }
        public EasyUserIdentity GetUserIdentity(string id)
        {
            if (TryGetUserById(id, out EasyUser user))
            {
                var result = ToUserIdentity(user);
                return result;
            }
            return null;
        }
        public IEnumerable<EasyUserIdentity> GetUserIdentities() =>
            dbContext.Users.AsEnumerable().Select(x => ToUserIdentity(x));

        private EasyUserIdentity ToUserIdentity(EasyUser user)
        {
            claimsService.TryGetClaimValueByUserId(user.Id, AmbsenyClaimTypes.ManageUsers.ToString(), out string claimValue);
            return new EasyUserIdentity
            {
                Id = user.Id,
                Name = user.Name,
                Identity = GetIdentityFromId(user.Id),
                Claim = claimValue
            };
        }
        private string GetIdentityFromId(string id)
        {

            if (claimsService.TryGetClaimCollectionByUserId(id, out IEnumerable<UserClaim> claimCollection))
            {
                var claimsList = claimCollection.ToList();
                if (claimsList.Count > 1)
                {
                    return EasyUserIdenityType.Multiple.ToString();
                }
                return EasyUserIdenityType.Default.ToString();
            }
            return EasyUserIdenityType.None.ToString();
        }

        public bool DeleteUser(string id)
        {
            if (TryGetUserById(id, out EasyUser user))
            {
                dbContext.Remove(user);
                return dbContext.SaveChanges() > 0;
            }
            return false;
        }

        public bool UpdateClaims(string id, Claim claim)
        {
            if(claimsService.TryGetClaimByUserId(id, claim.Type, out UserClaim userClaim))
            {
                return claimsService.UpdateExistingClaim(id, claim);
            }
            else
            {
                return claimsService.CreateNewClaim(id, claim);
            }
        }
        public bool TryGetUserById(string id, out EasyUser user)
        {
            var matches = dbContext.Users.Where(x => x.Id == id);
            var success = matches.Any();
            if (success)
            {
                user = matches.First();
            }
            else
            {
                user = null;
            }
            return success;
        }
        
    }
}
