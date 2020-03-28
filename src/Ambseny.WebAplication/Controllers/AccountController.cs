using Ambseny.WebAplication.Data.User;
using Ambseny.WebAplication.Models.Users;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Ambseny.WebAplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly EasyUserSignInManager signInManager;

        public AccountController(EasyUserSignInManager signInManager)
        {
            this.signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View(new EasyUser());
        }
        [HttpPost]
        public async Task<IActionResult> Login(EasyUser user)
        {
            await signInManager.SignInAsync(user, false);
            return Redirect("/Home");
            //return View(new EasyUser());
        }
    }
}