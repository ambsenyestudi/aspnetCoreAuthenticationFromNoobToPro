using Ambseny.WebAplication.Models.Users;
using Ambseny.WebAplication.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

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
            var profiles = usersService.GetAllUserProfile();
            return View(profiles);
        }
        [Authorize(Policy = "UserEditor")]
        [HttpGet("Edit")]
        public async Task<IActionResult> EditAsync(string id)
        {
            var profile = await usersService.GetUserProfileAsync(id);
            return View(profile);
        }
        [Authorize(Policy = "UserEditor")]
        [HttpPost("Edit")]
        public async Task<IActionResult> EditAsync(EasyUserProfile profile)
        {
            var claim = new Claim(AmbsenyClaimTypes.ManageUsers.ToString(), profile.ManageUserValue.ToString());
            var result = await usersService.UpdateClaimsAsync(profile.Id, claim);
            //todo show some type of error if fails
            return Redirect("/Manage/Index");
            
        }

        [Authorize(Policy = "UserAdministrator")]
        [HttpGet("Delete")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            if(!string.IsNullOrWhiteSpace(id))
            {
                if(await usersService.DeleteUserAsync(id))
                {
                    //todo show some type of error if fails
                }
            }

            
            return Redirect("/Manage/Index");
        }

    }
}