using System.ComponentModel.DataAnnotations;

namespace Ambseny.WebAplication.Models.Users
{
    public class CreateEasyUser: EasyUser
    {
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
