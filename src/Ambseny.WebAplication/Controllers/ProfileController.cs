﻿using Ambseny.WebAplication.Services.Users;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace Ambseny.WebAplication.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IUsersService usersService;

        public ProfileController(IUsersService usersService)
        {
            this.usersService = usersService;
        }
        public IActionResult Index(string id)
        {
            if(string.IsNullOrWhiteSpace(id))
            {
                id = User.Claims.Where(x => x.Type == ClaimTypes.Sid).Select(x=>x.Value).FirstOrDefault();
            }
            var userIdentity = usersService.GetUserIdentity(id);
            return View(userIdentity);
        }
    }
}