using Ambseny.WebAplication.Data;
using Ambseny.WebAplication.Models.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ambseny.WebAplication.Services.Users
{
    public class UsersService : IUsersService
    {
        private readonly EasyUserDbContext dbContext;
        private readonly UserManager<EasyUser> userManager;

        public UsersService(EasyUserDbContext dbContext, UserManager<EasyUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }
        public IEnumerable<EasyUserProfile> GetAllUserProfile()
        {
            var users = dbContext.Users.AsEnumerable();
            return users.Select(x => ToUserProfile(x));
        }

        private EasyUserProfile ToUserProfile(EasyUser user)
        {
            var manageUserClaim = AmbsenyManageUserClaims.None;
            var userClaimCollection = userManager.GetClaimsAsync(user).Result.Where(x => x.Type == AmbsenyClaimTypes.ManageUsers.ToString());
            if (userClaimCollection.Any())
            {
                var claimValue = userClaimCollection.First().Value;
                if (Enum.IsDefined(typeof(AmbsenyManageUserClaims), claimValue))
                {
                    manageUserClaim = (AmbsenyManageUserClaims)Enum.Parse(typeof(AmbsenyManageUserClaims), claimValue);
                }
            }

            return new EasyUserProfile
            {
                Id = user.Id,
                ManageUserValue = manageUserClaim,
                Name = user.Name
            };
        }

        public async Task<EasyUserProfile> GetUserProfileAsync(string sid)
        {
            (var success, var user) = await TryGetUserByIdAsync(sid);
            if (success)
            {
                return ToUserProfile(user);
            }
            return null;
        }


        public async Task<bool> DeleteUserAsync(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            var result = await userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> UpdateClaimsAsync(string id, Claim claim)
        {
            var user = await userManager.FindByIdAsync(id);
            var userClaimCollection = await userManager.GetClaimsAsync(user);
            var oldClaim = userClaimCollection.Where(x => x.Type == claim.Type).FirstOrDefault();
            if(oldClaim!=null)
            {
                var result = await userManager.ReplaceClaimAsync(user, oldClaim, claim);
                return result.Succeeded; 
            }
            return false;
        }
        public async Task<(bool, EasyUser)> TryGetUserByIdAsync(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            return (user != null, user);
        }
        
    }
}
