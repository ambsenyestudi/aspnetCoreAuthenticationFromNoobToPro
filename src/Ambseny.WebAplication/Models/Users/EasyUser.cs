using System.ComponentModel.DataAnnotations;

namespace Ambseny.WebAplication.Models.Users
{
    public class EasyUser
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
