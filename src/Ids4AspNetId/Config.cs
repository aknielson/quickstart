using IdentityServer4.Models;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer4;
using IdentityServer4.Test;

namespace Ids4AspNetId
{
  public static class Config
  {
    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
      return new List<IdentityResource>
      {
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
      };
    }

    public static IEnumerable<ApiResource> GetApis()
    {
      return new List<ApiResource>
      {
        new ApiResource("api1", "My API")
      };
    }

    public static IEnumerable<Client> GetClients()
    {
      return new[]
      {
        // client credentials flow client
        new Client
        {
          ClientId = "client",
          ClientName = "Client Credentials Client",

          AllowedGrantTypes = GrantTypes.ClientCredentials,
          ClientSecrets = {new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256())},

          AllowedScopes = {"api1"}
        },

        // MVC client using hybrid flow
        new Client
        {
          ClientId = "mvc",
          ClientName = "MVC Client",

          AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
          ClientSecrets = {new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256())},

          RedirectUris = {"http://localhost:5001/signin-oidc"},
          FrontChannelLogoutUri = "http://localhost:5001/signout-oidc",
          PostLogoutRedirectUris = {"http://localhost:5001/signout-callback-oidc"},

          AllowOfflineAccess = true,
          AllowedScopes = {"openid", "profile", "api1"}
        },

        // SPA client using implicit flow
        new Client
        {
          ClientId = "spa",
          ClientName = "SPA Client",
          ClientUri = "http://identityserver.io",

          AllowedGrantTypes = GrantTypes.Implicit,
          AllowAccessTokensViaBrowser = true,

          RedirectUris =
          {
            "http://localhost:5002/index.html",
            "http://localhost:5002/callback.html",
            "http://localhost:5002/silent.html",
            "http://localhost:5002/popup.html",
          },

          PostLogoutRedirectUris = {"http://localhost:5002/index.html"},
          AllowedCorsOrigins = {"http://localhost:5002"},

          AllowedScopes = {"openid", "profile", "api1"}
        }
      };
    }

    //public static List<TestUser> GetUsers()
    //{
    //  return new List<TestUser>
    //  {
    //    new TestUser
    //    {
    //      SubjectId = "1",
    //      Username = "alice",
    //      Password = "password",

    //      Claims = new []
    //      {
    //        new Claim("name", "Alice"),
    //        new Claim("website", "https://alice.com")
    //      }
    //    },
    //    new TestUser
    //    {
    //      SubjectId = "2",
    //      Username = "bob",
    //      Password = "password",

    //      Claims = new []
    //      {
    //        new Claim("name", "Bob"),
    //        new Claim("website", "https://bob.com")
    //      }
    //    }
    //  };
    //}
  }
}
