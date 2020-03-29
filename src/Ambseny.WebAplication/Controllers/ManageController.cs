using Ambseny.WebAplication.Models.Users;
using Ambseny.WebAplication.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
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
            var identites = usersService.GetUserIdentities();
            return View(identites);
        }
        [HttpGet]
        public IActionResult Edit(string id)
        {
            var identites = usersService.GetUserIdentity(id);
            return View(identites);
        }
        [HttpPost]
        public IActionResult Edit(EasyUserIdentity userIdentity)
        {
            var claim = new Claim(userIdentity.Identity, userIdentity.Claim);
            var result = usersService.UpdateClaims(userIdentity.Id, claim);
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