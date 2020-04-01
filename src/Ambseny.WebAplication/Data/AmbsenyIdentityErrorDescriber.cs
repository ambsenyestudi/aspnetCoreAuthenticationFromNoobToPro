using Microsoft.AspNetCore.Identity;

namespace Ambseny.WebAplication.Data
{
    public enum AmbsenyIdentityErrorDefaults
    {
        None,
        UnableToPersistNewUser,
        UnableToDeleteNewUser,
        NonExistingUser
    };

    public class AmbsenyIdentityErrorDescriber : IdentityErrorDescriber
    {
        public IdentityError UnableToPersistNewUser(string userName) =>
            CreateAmbsenyIdentityError(AmbsenyIdentityErrorDefaults.UnableToPersistNewUser, "Error while trying to persist new user "+userName);
        public IdentityError UnableToDeleteNewUser(string userName) =>
            CreateAmbsenyIdentityError(AmbsenyIdentityErrorDefaults.UnableToDeleteNewUser, "Error while trying to delete new user " + userName);
        public IdentityError NonExistingUser(string userName) =>
            CreateAmbsenyIdentityError(AmbsenyIdentityErrorDefaults.NonExistingUser, $"User {userName} is not registered");
        private IdentityError CreateAmbsenyIdentityError(AmbsenyIdentityErrorDefaults errorCode, string description) =>
             new IdentityError { Code = nameof(errorCode), Description = description };
    }
}
