using Ambseny.WebAplication.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ambseny.WebAplication.Controllers
{
    [Authorize(Policy = "UserReviewer")]
    public class ManageController : Controller
    {
        private readonly IUsersService usersService;

        public ManageController(IUsersService usersService)
        {
            this.usersService = usersService;
        }
        
        public IActionResult Index()
        {
            var profileCollection = usersService.GetAllUserProfile();
            return View(profileCollection);
        }

    }
}