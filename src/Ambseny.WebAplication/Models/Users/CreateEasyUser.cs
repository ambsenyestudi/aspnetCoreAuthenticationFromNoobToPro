using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ambseny.WebAplication.Models.Users
{
    public class CreateEasyUser: EasyUser
    {
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
