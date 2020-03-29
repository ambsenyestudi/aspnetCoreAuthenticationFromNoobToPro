using Ambseny.WebAplication.Data;
using Ambseny.WebAplication.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Ambseny.WebAplication.Services.Claims
{
    public class ClaimsService: IClaimsService
    {
        private readonly EasyUserDbContext dbContext;

        public ClaimsService(EasyUserDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public bool CreateNewClaim(string userId, Claim claim)
        {
            dbContext.UserClaims.Add(CreateNewEntity(userId, claim));
            return dbContext.SaveChanges() > 0;
        }

        public bool TryGetClaimByUserId(string userId, string claimType, out UserClaim userClaim)
        {
            var success = TryGetClaimCollectionByUserId(userId, out IEnumerable<UserClaim> claimCollection);
            userClaim = null;
            if (success)
            {
                var matchingClaims = claimCollection.Where(x => x.ClaimType == claimType);
                var claimsSuccess = matchingClaims.Any();
                if (claimsSuccess)
                {
                    userClaim = matchingClaims.First();
                }
                return claimsSuccess;
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
        public bool UpdateExistingClaim(string userId, Claim claim)
        {
            var success = TryGetClaimByUserId(userId, claim.Type, out UserClaim userClaim);
            if(success)
            {
                userClaim.ClaimValue = claim.Value;
                dbContext.Update(userClaim);
                return dbContext.SaveChanges() > 0;
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

        private UserClaim CreateNewEntity(string userId, Claim claim) =>
            new UserClaim
            {
                ClaimType = claim.Type,
                ClaimValue = claim.Value,
                Id = Guid.NewGuid(),
                UserId = userId
            };
    }
}
