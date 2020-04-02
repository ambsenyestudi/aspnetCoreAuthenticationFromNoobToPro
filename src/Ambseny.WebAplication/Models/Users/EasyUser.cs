using System.ComponentModel.DataAnnotations;

namespace Ambseny.WebAplication.Models.Users
{
    public class EasyUser
    {
        [Key]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        public string NormalizedName { get; internal set; }
    }
}
