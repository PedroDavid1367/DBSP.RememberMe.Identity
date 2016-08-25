using IdentityServer3.Core.Services.InMemory;
using System.Collections.Generic;
using System.Security.Claims;

namespace DBSP.RememberMe.Identity.Server.Config
{
  public static class CustomUsers
  {
    public static List<InMemoryUser> Get()
    {
      return new List<InMemoryUser>()
      {
        new InMemoryUser
        {
          Username = "Kevin",
          Password = "secret",
          Subject = "b05d3546-6ca8-4d32-b95c-77e94d705ddf",
          Claims = new[]
          {
            new Claim(IdentityServer3.Core.Constants.ClaimTypes.GivenName, "Kevin"),
            new Claim(IdentityServer3.Core.Constants.ClaimTypes.FamilyName, "Dockx"),
            new Claim(IdentityServer3.Core.Constants.ClaimTypes.Address, "1, Main Street, Antwerp, Belgium"),
            new Claim("role", "PayingUser")
          }
        },
        new InMemoryUser
        {
          Username = "Sven",
          Password = "secret",
          Subject = "bb61e881-3a49-42a7-8b62-c13dbe102018",
          Claims = new[]
          {
            new Claim(IdentityServer3.Core.Constants.ClaimTypes.GivenName, "Sven"),
            new Claim(IdentityServer3.Core.Constants.ClaimTypes.FamilyName, "Vercauteren"),
            new Claim(IdentityServer3.Core.Constants.ClaimTypes.Address, "2, Main Road, Antwerp, Belgium"),
            new Claim("role", "FreeUser")
          }
        }
      };
    }
  }
}
