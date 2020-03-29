using Ambseny.WebAplication.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace Ambseny.WebAplication.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private readonly IUsersService usersService;

        public ManageController(IUsersService usersService)
        {
            this.usersService = usersService;
        }
        [Authorize(Policy = "Minimal")]
        public IActionResult Index()
        {
            var claimsDicitonary = HttpContext.User.Claims.ToDictionary(x => x.Type, x => x.Value);
            if(claimsDicitonary.ContainsKey(ClaimTypes.Sid))
            {
                var userId = claimsDicitonary[ClaimTypes.Sid];
                var user = usersService.GetUser(userId);
                return View(user);
            }

            return Unauthorized();
        }
        
        [Authorize(Policy = "UserReviewer")]
        public IActionResult UserClaims()
        {
            var identites = usersService.GetUserIdentities();
            return View(identites);
        }
    }
}