using System.ComponentModel.DataAnnotations;

namespace Ambseny.WebAplication.Models.Users
{
    public class CreateEasyUser: LoginEasyUser
    {
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
