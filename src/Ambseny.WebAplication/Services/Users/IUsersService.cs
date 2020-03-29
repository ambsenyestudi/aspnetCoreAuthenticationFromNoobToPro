using Ambseny.WebAplication.Models.Users;
using System.Collections.Generic;
using System.Security.Claims;

namespace Ambseny.WebAplication.Services.Users
{
    public interface IUsersService
    {
        IEnumerable<EasyUserProfile> GetAllUserProfile();
        EasyUserProfile GetUserProfile(string sid);
        bool DeleteUser(string id);
        bool UpdateClaims(string id, Claim claim);
    }
}
