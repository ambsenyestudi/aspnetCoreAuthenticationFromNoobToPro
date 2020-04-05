using Ambseny.WebAplication.Models.Options;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ambseny.WebAplication.IdentityServer.Stores
{
    public class CustomClientStore : IClientStore
    {
        public List<Client> AllClients { get; }

        private readonly string mvcClientUri;

        public CustomClientStore(IOptions<IdentityClientOptions> options)
        {
            AllClients = CreateAllClients();
            mvcClientUri = options.Value.MVCClientUri;
        }



        public Task<Client> FindClientByIdAsync(string clientId)
        {
            return Task.FromResult(AllClients.FirstOrDefault(c => c.ClientId == clientId));
        }

        private List<Client> CreateAllClients()=>
            new List<Client>
            {
                CreateClientWithToken("client_id_mvc",
                    GrantTypes.Code,
                    new List<string>{ IdentityServerConstants.StandardScopes.OpenId, "ApiOne" },
                    new List<Secret>{ new Secret("client_secret_mvc".ToSha256()) },
                    new List<string>{ $"{mvcClientUri}/signin-oidc" })
            };

        private Client CreateClient(string id, ICollection<string> allowedGrantTypes, ICollection<string> allowedScopes, bool isRequireConsent = false) =>
            new Client
            {
                AllowedGrantTypes = allowedGrantTypes,
                AllowedScopes = allowedScopes,
                ClientId = id,
                RequireConsent = isRequireConsent
            };

        private Client CreateClientWithToken(string id, ICollection<string> allowedGrantTypes, ICollection<string> allowedScopes, ICollection<Secret> clientSecrets, ICollection<string> redirectUriCollection, bool isRequireConsent = false)
        {
            var client = CreateClient(id, allowedGrantTypes, allowedScopes, isRequireConsent);
            client.AlwaysIncludeUserClaimsInIdToken = true;
            client.AllowOfflineAccess = true;
            client.ClientSecrets = clientSecrets;
            client.RedirectUris = redirectUriCollection;
            return client;
        }
        
    }
}
