using Ambseny.WebAplication.Models.Users;
using System.Collections.Generic;
using System.Security.Claims;

namespace Ambseny.WebAplication.Services.Users
{
    public interface IUsersService
    {
        IEnumerable<EasyUserIdentity> GetUserIdentities();
        EasyUserIdentity GetUserIdentity(string id);
        bool DeleteUser(string id);
        bool UpdateClaims(string id, Claim claim);
    }
}
