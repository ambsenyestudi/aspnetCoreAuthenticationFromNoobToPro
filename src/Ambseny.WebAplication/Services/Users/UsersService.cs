using Ambseny.WebAplication.Data;
using Ambseny.WebAplication.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Ambseny.WebAplication.Services.Users
{
    public class UsersService : IUsersService
    {
        private readonly EasyUserDbContext dbContext;

        public UsersService(EasyUserDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public EasyUserIdentity GetUserIdentity(string id)
        {
            if (TryGetUserById(id, out EasyUser user))
            {
                var result = ToUserIdentity(user);
                return result;
            }
            return default(EasyUserIdentity);
        }
        public IEnumerable<EasyUserIdentity> GetUserIdentities() =>
            dbContext.Users.AsEnumerable().Select(x => ToUserIdentity(x));

        private EasyUserIdentity ToUserIdentity(EasyUser user)
        {
            TryGetClaimValueByUserId(user.Id, AmbsenyClaimTypes.ManageUsers.ToString(), out string claim);
            return new EasyUserIdentity
            {
                Id = user.Id,
                Name = user.Name,
                Identity = GetIdentityFromId(user.Id),
                Claim = claim
            };
        }
        private string GetIdentityFromId(string id)
        {

            if (TryGetClaimCollectionByUserId(id, out IEnumerable<UserClaim> claimCollection))
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
            if (TryGetClaimCollectionByUserId(id, out IEnumerable<UserClaim> claimCollection))
            {
                var matchingClaims = claimCollection.Where(x => x.ClaimType == claim.Type);
                if (matchingClaims.Any())
                {
                    var updatingClaim = matchingClaims.First();
                    updatingClaim.ClaimValue = claim.Value;
                    dbContext.UserClaims.Update(updatingClaim);
                    var changeCount = dbContext.SaveChanges();
                    return changeCount > 0;
                }
            }
            return false;
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
        public bool TryGetClaimCollectionByUserId(string userId, out IEnumerable<UserClaim> claimCollection)
        {
            var matches = dbContext.UserClaims.Where(x => x.UserId == userId);
            var success = matches.Any();
            if (success)
            {
                claimCollection = matches;
            }
            else
            {
                claimCollection = new List<UserClaim>();
            }
            return success;
        }
        public bool TryGetClaimByUserId(string userId, string claimType, out UserClaim userClaim)
        {
            var success = TryGetClaimCollectionByUserId(userId, out IEnumerable<UserClaim> claimCollection);
            userClaim = null;
            if (success)
            {
                var matchingClaims = claimCollection.Where(x => x.ClaimType == claimType);
                if (matchingClaims.Any())
                {
                    userClaim = matchingClaims.First();
                }
            }
            return success;
        }
        public bool TryGetClaimValueByUserId(string userId, string claimType, out string claimValue)
        {
            var success = TryGetClaimByUserId(userId, claimType, out UserClaim userClaim);
            claimValue = string.Empty;
            if (success)
            {
                claimValue = userClaim.ClaimValue;
            }
            
            return false;
        }
    }
}
