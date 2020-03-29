using Ambseny.WebAplication.Data;
using Ambseny.WebAplication.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ambseny.WebAplication.Services.Users
{
    public class UsersService : IUsersService
    {
        private readonly EasyUserDbContext dbContext;

        public UsersService(EasyUserDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public UserDetail GetUser(string id)
        {
            var matches = dbContext.Users.Where(x => x.Id == id);
            if(matches.Any())
            {
                var result = ToUserDetail(matches.First());
                return result;
            }
            return default(UserDetail);
        }
        public IEnumerable<EasyUserIdentity> GetUserIdentities() =>
            dbContext.Users.AsEnumerable().Select(x => ToUserIdentity(x));

        private EasyUserIdentity ToUserIdentity(EasyUser user) =>
            new EasyUserIdentity
            {
                Id = user.Id,
                Name = user.Name,
                Identity = GetIdentityFromId(user.Id),
                Claim = "Todo"
            };

        private UserDetail ToUserDetail(EasyUser user) =>
             new UserDetail
             {
                 Id = user.Id,
                 Name = user.Name,
                 Identity = GetIdentityFromId(user.Id)
             };
        private string GetIdentityFromId(string id)
        {
            var matchingClaims = dbContext.UserClaims.Where(x => x.UserId == id);
            if(matchingClaims.Any())
            {
                var claimsList = matchingClaims.ToList();
                if(claimsList.Count > 1)
                {
                    return EasyUserIdenityType.Multiple.ToString();
                }
                return EasyUserIdenityType.Default.ToString();
            }
            return EasyUserIdenityType.None.ToString();
        }

        
    }
}
