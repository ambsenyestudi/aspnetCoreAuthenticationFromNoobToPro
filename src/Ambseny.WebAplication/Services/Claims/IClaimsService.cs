using Ambseny.WebAplication.Models.Users;
using System.Collections.Generic;
using System.Security.Claims;

namespace Ambseny.WebAplication.Services.Claims
{
    public interface IClaimsService
    {
        bool TryGetClaimByUserId(string userId, string claimType, out UserClaim userClaim);
        bool TryGetClaimCollectionByUserId(string userId, out IEnumerable<UserClaim> claimCollection);
        bool UpdateExistingClaim(string userId, Claim claim);
        bool CreateNewClaim(string userId, Claim claim);
        bool TryGetClaimValueByUserId(string userId, string claimType, out string userValue);

    }
}
