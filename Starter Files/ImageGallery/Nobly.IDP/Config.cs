using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;
using System.Security.Claims;

namespace Nobly.IDP
{
    public static class Config
    {
        // test users
        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
               new TestUser
               {
                   SubjectId = "d860efca-22d9-47fd-8249-791ba61b07c7",
                   Username = "Frank",
                   Password = "password",

                   Claims = new List<Claim>
                   {
                       new Claim("given_name", "Frank"),
                       new Claim("family_name", "Underwood"),
                       new Claim("address", "Main road 1"),
                       new Claim("subscriptionlevel", "FreeUser"),
                       new Claim("country", "nl")
                   }
               },
               new TestUser
               {
                   SubjectId = "b7539694-97e7-4dfe-84da-b4256e1ff5c7",
                   Username = "Claire",
                   Password = "password",

                   Claims = new List<Claim>
                   {
                       new Claim("given_name", "Claire"),
                       new Claim("family_name", "Underwood"),
                       new Claim("address", "Big street 2"),
                       new Claim("subscriptionlevel", "PayingUser"),
                       new Claim("country", "be")
                   }
               }
            };
        }

        // identity-related resources (scopes)
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                new IdentityResource(
                  "roles", //Scope name
                "Your role(s)",  //Display name
                new List<string>() {"role"} //List of claims that must be returned when app asks for roles scope
                ),
                new IdentityResource(
                    "country",
                    "The country you're living in",
                    new List<string>() { "country" }),
                new IdentityResource(
                    "subscriptionlevel",
                    "Your subscription level",
                    new List<string>() { "subscriptionlevel" })
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("imagegalleryapi", "Image Gallery API",
                new List<string>() { "role" } )
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>()
            {
                new Client
                {
                    ClientName = "Image Gallery",
                    ClientId = "imagegalleryclient",
                    AllowedGrantTypes = GrantTypes.Hybrid,
                    //IdentityTokenLifetime = ... In seconds, default 5min 600
                    //AuthorizationCodeLifetime = ... Not a token per say, 
                    //it is exchange for one or more tokens when the token endpoint is called, default is 600

                    AccessTokenLifetime = 120, //In seconds, default value is 1h
                    AllowOfflineAccess = true,
                    //AbsoluteRefreshTokenLifetime = ...
                    //RefreshTokenExpiration = TokenExpiration.Sliding //Once a new token is requested, it's lifetime is renewed.
                    
                    UpdateAccessTokenClaimsOnRefresh = true, // By default claims in access token stay the same when refreshing them. 
                    //By setting this to true, we do refresh the claims.
                    RedirectUris = new List<string>()
                    {
                        "https://localhost:44318/signin-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Address,
                        "roles",
                        "imagegalleryapi",
                        "country",
                        "subscriptionlevel"
                    },
                    PostLogoutRedirectUris = new List<string>()
                    {
                        "https://localhost:44318/signout-callback-oidc"
                    },
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    }
                }
            };
        }
    }
}
