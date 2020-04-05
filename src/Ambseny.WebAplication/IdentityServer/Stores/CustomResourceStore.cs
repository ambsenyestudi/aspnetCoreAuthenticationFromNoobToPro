using IdentityServer4.Models;
using IdentityServer4.Stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ambseny.WebAplication.IdentityServer.Stores
{
    public class CustomResourceStore : IResourceStore
    {
        private readonly List<ApiResource> apiResourceCollection;
        private readonly List<IdentityResource> identityResourceCollection;

        public CustomResourceStore()
        {
            apiResourceCollection = ProduceApiResources();
            identityResourceCollection = ProduceIdentityResources();
        }

        public Task<ApiResource> FindApiResourceAsync(string name) =>
            Task.FromResult(
                apiResourceCollection.FirstOrDefault(x => x.Name == name)
                );


        public Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames) =>
            Task.FromResult(
                apiResourceCollection
                    .Where(x => x.Scopes
                        .Where(s=>scopeNames.Contains(s.Name)).Any())
                );

        public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames) =>
            Task.FromResult(identityResourceCollection.Where(x => scopeNames.Contains(x.Name)));
        /*        
        todo check it
        https://github.com/IdentityServer/IdentityServer4/blob/master/src/EntityFramework.Storage/src/Stores/ResourceStore.cs
        */
        public Task<Resources> GetAllResourcesAsync() =>
            Task.FromResult(
                new Resources { ApiResources = apiResourceCollection, IdentityResources = identityResourceCollection }
                );

        private List<ApiResource> ProduceApiResources()=> 
            new List<ApiResource> {
                new ApiResource("ApiOne"),
                new ApiResource("ApiTwo", new string[]{"rc.api.grandma"})
            };

        private List<IdentityResource> ProduceIdentityResources() => new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResource
                {
                    Name = "rc.scope",
                    UserClaims =
                    {
                        "rc.grandma"
                    }
                }
            };
    }
}
