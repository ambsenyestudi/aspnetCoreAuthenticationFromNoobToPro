namespace Ambseny.WebAplication.Models.Users
{
    public class EasyUserProfile
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public AmbsenyManageUserClaims ManageUserValue { get; set; }
        public string ManageUserStatus { get => ManageUserValue.ToString(); }
    }
}
