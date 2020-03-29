using System;
using System.ComponentModel.DataAnnotations;

namespace Ambseny.WebAplication.Models.Users
{
    public enum AmbsenyClaimTypes
    {
        None, ManageUsers
    }
    public enum AmbsenyManageUserClaims
    {
        None, Review, Edit, Administrate
    }
    public class UserClaim
    {
        [Key]
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
