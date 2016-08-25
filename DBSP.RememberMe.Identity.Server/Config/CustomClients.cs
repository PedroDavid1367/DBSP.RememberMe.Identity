using IdentityServer3.Core.Models;
using System.Collections.Generic;

namespace DBSP.RememberMe.Identity.Server.Config
{
  public static class CustomClients
  {
    public static IEnumerable<Client> Get()
    {
      return new[]
      {
        new Client
        {
          ClientId = "tripgalleryclientcredentials",
          ClientName = "Trip Gallery (Client Credentials)",
          Flow = Flows.ClientCredentials,
          AllowAccessToAllScopes = true,

          ClientSecrets = new List<Secret>()
          {
            new Secret(DBSP.RememberMe.Identity.Constants.TripGalleryClientSecret.Sha256())
          }
        },
        new Client
        {
          ClientId = "tripgalleryauthcode",
          ClientName = "Trip Gallery (Authorization Code)",
          Flow = Flows.AuthorizationCode,
          AllowAccessToAllScopes = true,

          // redirect = URI of our callback controller in the MVC application
          RedirectUris = new List<string>
          {
            DBSP.RememberMe.Identity.Constants.TripGalleryMVCSTSCallback
          },

          ClientSecrets = new List<Secret>()
          {
            new Secret(DBSP.RememberMe.Identity.Constants.TripGalleryClientSecret.Sha256())
          }
        },
        new Client
        {
          ClientId = "tripgalleryimplicit",
          ClientName = "Trip Gallery (Implicit)",
          Flow = Flows.Implicit,
          AllowAccessToAllScopes = true,
          IdentityTokenLifetime = 10,
          AccessTokenLifetime = 120,
          RequireConsent = false,

          // redirect = URI of the Angular application
          RedirectUris = new List<string>
          {
            DBSP.RememberMe.Identity.Constants.TripGalleryAngular + "callback.html",
            // for silent refresh
            DBSP.RememberMe.Identity.Constants.TripGalleryAngular + "silentrefreshframe.html"
          },
          PostLogoutRedirectUris = new List<string>()
          {
            DBSP.RememberMe.Identity.Constants.TripGalleryAngular + "index.html"
          }
        },
        new Client
        {
          ClientId = "tripgalleryimplicitAngular2",
          ClientName = "Trip Gallery (Implicit - Angular2)",
          Flow = Flows.Implicit,
          AllowAccessToAllScopes = true,
          IdentityTokenLifetime = 10,
          AccessTokenLifetime = 120,
          RequireConsent = false,

          RedirectUris = new List<string>
          {
            "http://localhost:8080/callback",
            // For silent refresh.
            "http://localhost:8080/silent-refresh",
          },

          // PostLogoutRedirectUris should work when using Https

          //PostLogoutRedirectUris = new List<string>()
          //{
          //  "http://localhost:8080/home"
          //}
        },
        new Client
        {
          ClientId = "tripgalleryropc",
          ClientName = "Trip Gallery (Resource Owner Password Credentials)",
          Flow = Flows.ResourceOwner,
          AllowAccessToAllScopes = true,

          ClientSecrets = new List<Secret>()
          {
            new Secret(DBSP.RememberMe.Identity.Constants.TripGalleryClientSecret.Sha256())
          }
        },
        new Client
        {
          ClientId = "tripgalleryhybrid",
          ClientName = "Trip Gallery (Hybrid)",
          Flow = Flows.Hybrid,
          AllowAccessToAllScopes = true,
          IdentityTokenLifetime = 10,
          AccessTokenLifetime = 120,
          RequireConsent = false,
                                   
          // redirect = URI of the MVC application
          RedirectUris = new List<string>
          {
            DBSP.RememberMe.Identity.Constants.TripGalleryMVC
          },
          PostLogoutRedirectUris = new List<string>()
          {
            DBSP.RememberMe.Identity.Constants.TripGalleryMVC
          },
          ClientSecrets = new List<Secret>()
          {
            new Secret(DBSP.RememberMe.Identity.Constants.TripGalleryClientSecret.Sha256())
          }
        }
      };
    }
  }
}
