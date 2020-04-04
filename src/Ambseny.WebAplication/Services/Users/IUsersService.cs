using Ambseny.WebAplication.Models.Users;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ambseny.WebAplication.Services.Users
{
    public interface IUsersService
    {
        IEnumerable<EasyUserProfile> GetAllUserProfile();
        Task<EasyUserProfile> GetUserProfileAsync(string sid);
        Task<bool> DeleteUserAsync(string id);
        Task<bool> UpdateClaimsAsync(string id, Claim claim);
    }
}
