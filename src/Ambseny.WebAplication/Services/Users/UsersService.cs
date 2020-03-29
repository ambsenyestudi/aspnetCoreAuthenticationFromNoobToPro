using Ambseny.WebAplication.Data;
using Ambseny.WebAplication.Models.Users;
using Ambseny.WebAplication.Services.Claims;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public IEnumerable<EasyUserProfile> GetAllUserProfile()
        {
            var users = dbContext.Users.AsEnumerable();
            return users.Select(x => ToUserProfile(x));
        }

        private EasyUserProfile ToUserProfile(EasyUser user)
        {
            var manageUserClaim = AmbsenyManageUserClaims.None;
            if (claimsService.TryGetClaimByUserId(user.Id, AmbsenyClaimTypes.ManageUsers.ToString(), out UserClaim claim))
            {
                if (Enum.IsDefined(typeof(AmbsenyManageUserClaims), claim.ClaimValue))
                {
                    manageUserClaim = (AmbsenyManageUserClaims)Enum.Parse(typeof(AmbsenyManageUserClaims), claim.ClaimValue);
                }
            }
            return new EasyUserProfile
            {
                Id = user.Id,
                ManageUserValue = manageUserClaim,
                Name = user.Name
            };
        }

        public EasyUserProfile GetUserProfile(string sid)
        {
            if(TryGetUserById(sid, out EasyUser user))
            {
                return ToUserProfile(user);
            }
            return null;
            
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
