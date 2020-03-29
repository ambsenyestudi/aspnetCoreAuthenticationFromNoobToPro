using Ambseny.WebAplication.Models.Users;
using Ambseny.WebAplication.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        [HttpGet]
        public IActionResult Edit(string id)
        {
            var profile = usersService.GetUserProfile(id);
            return View(profile);
        }
        [Authorize(Policy = "UserEditor")]
        [HttpPost]
        public IActionResult Edit(EasyUserProfile profile)
        {
            var claim = new Claim(AmbsenyClaimTypes.ManageUsers.ToString(), profile.ManageUserValue.ToString());
            var result = usersService.UpdateClaims(profile.Id, claim);
            return Redirect("/Manage/Index");
        }

        [Authorize(Policy = "UserAdministrator")]
        public IActionResult Delete(string id)
        {
            if(!string.IsNullOrWhiteSpace(id))
            {
                usersService.DeleteUser(id);
            }
            
            return Redirect("/Manage/Index");
        }

    }
}