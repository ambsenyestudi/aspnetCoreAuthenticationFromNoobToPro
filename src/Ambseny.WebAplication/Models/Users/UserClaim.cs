using System;
using System.ComponentModel.DataAnnotations;

namespace Ambseny.WebAplication.Models.Users
{
    public class UserClaim
    {
        [Key]
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
