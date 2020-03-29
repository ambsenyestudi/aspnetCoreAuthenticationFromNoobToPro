namespace Ambseny.WebAplication.Models.Users
{
    public enum EasyUserIdenityType
    {
        None, Default, Multiple
    }
    public class EasyUserIdentity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Identity { get; set; }
        public string Claim { get; set; }
    }
}
