using System.Collections.Generic;

namespace Ambseny.WebAplication
{
    public enum AmbsenyIdentityErrorDefaults
    {
        None, 
        ExistingName,
        UnableToPersistNewUser,
        NonExistingUser
    }
    public static class AmbsenyAuthenticationDefaults
    {
        public static readonly Dictionary<AmbsenyIdentityErrorDefaults, string> IdentityErrorDefaults = new Dictionary<AmbsenyIdentityErrorDefaults, string>
        {
            [AmbsenyIdentityErrorDefaults.ExistingName] = "Name {0} already taken",
            [AmbsenyIdentityErrorDefaults.UnableToPersistNewUser] = "Error while trying to persist new user",
            [AmbsenyIdentityErrorDefaults.NonExistingUser] = "User {0} is not registered"
        };
    }
}
