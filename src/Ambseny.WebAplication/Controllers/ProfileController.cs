using Ambseny.WebAplication.Services.Users;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ambseny.WebAplication.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IUsersService usersService;

        public ProfileController(IUsersService usersService)
        {
            this.usersService = usersService;
        }
        public async Task<IActionResult> Index(string id)
        {
            var sid = id;
            if(string.IsNullOrWhiteSpace(sid))
            {
                sid = User.Claims.Where(x => x.Type == ClaimTypes.Sid).Select(x=>x.Value).FirstOrDefault();
            }
            var userProfile = await usersService.GetUserProfileAsync(sid);
            return View(userProfile);
        }
    }
}