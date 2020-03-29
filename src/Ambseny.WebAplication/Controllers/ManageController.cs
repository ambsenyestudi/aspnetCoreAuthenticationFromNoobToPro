using Ambseny.WebAplication.Models.Users;
using Ambseny.WebAplication.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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
            var identity = usersService.GetUserIdentity(id);
            var claim = AmbsenyManageUserClaims.None;
            if(Enum.IsDefined(typeof(AmbsenyManageUserClaims), identity.Claim))
            {
                claim = (AmbsenyManageUserClaims)Enum.Parse(typeof(AmbsenyManageUserClaims), identity.Claim);
            }
            var editIdentity = new EditEasyManageUser
            {
                Id = identity.Id,
                Name = identity.Name,
                Identity = claim
            };
            return View(editIdentity);
        }
        [HttpPost]
        public IActionResult Edit(EditEasyManageUser editUserClaims)
        {
            var claim = new Claim(AmbsenyClaimTypes.ManageUsers.ToString(), editUserClaims.Identity.ToString());
            var result = usersService.UpdateClaims(editUserClaims.Id, claim);
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