using Ambseny.WebAplication.Data.User;
using Ambseny.WebAplication.Models.Users;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Ambseny.WebAplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly EasyUserSignInManager signInManager;
        private readonly EasyUserManager userManager;

        public AccountController(EasyUserSignInManager signInManager, EasyUserManager userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View(new EasyUser());
        }
        [HttpPost]
        public async Task<IActionResult> Login(EasyUser user)
        {
            var signinResult = await signInManager.PasswordSignInAsync(user, user.Password, false, false);
            if (signinResult.Succeeded)
            {
                return Redirect("/Home");
            }
            else
            {
                //use name validation to output 
                var errorMessage = string.Format(AmbsenyAuthenticationDefaults.IdentityErrorDefaults[AmbsenyIdentityErrorDefaults.NonExistingUser], user.Name);
                ModelState.AddModelError(nameof(user.Name), errorMessage);
            }
            return View(user);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new CreateEasyUser());
        }
        [HttpPost]
        public async Task<IActionResult> Register(CreateEasyUser user)
        {
            
            var creationResult = await userManager.CreateAsync(user);
            
            if (creationResult.Succeeded)
            {
                await signInManager.SignInAsync(user, false);
                return Redirect("/Home");
            }
            else
            {
                //use name validation to output 
                var errorMessage = creationResult.Errors.First().Description;
                ModelState.AddModelError(nameof(user.Name), errorMessage);
            }
            return View(user);
        }
    }
}