using Ambseny.WebAplication.Models.Users;
using System.Collections.Generic;

namespace Ambseny.WebAplication.Services.Users
{
    public interface IUsersService
    {
        IEnumerable<EasyUserProfile> GetAllUserProfile();
        EasyUserProfile GetUserProfile(string sid);
    }
}
