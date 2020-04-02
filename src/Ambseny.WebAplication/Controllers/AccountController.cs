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
            return View(new LoginEasyUser());
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginEasyUser user)
        {
           
            var signinResult = await signInManager.PasswordSignInAsync(user.Name.ToUpper(), user.Password, false, false);
            if (signinResult.Succeeded)
            {
                return Redirect("/Home");
            }
            else
            {
                //use name validation to output 
                //var errorMessage = string.Format(AmbsenyAuthenticationDefaults.IdentityErrorDefaults[AmbsenyIdentityErrorDefaults.NonExistingUser], user.Name);
                //fix this
                ModelState.AddModelError(nameof(user.Name), "Non existing");
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
            //user usernormalizer dependency
            var normalizedName = user.Name.ToUpper();
            var easyUser = new EasyUser { Name = user.Name, NormalizedName = normalizedName };
            var creationResult = await userManager.CreateAsync(easyUser, user.Password);
            
            if (creationResult.Succeeded)
            {
                await signInManager.SignInAsync(easyUser, false);
                
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

        [HttpGet]
        public IActionResult Logout()
        {
            signInManager.SignOutAsync();
            return Redirect("/Home");
        }
    }
}