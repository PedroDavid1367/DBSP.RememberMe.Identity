using IdentityServer3.Core.Models;
using System.Collections.Generic;

namespace DBSP.RememberMe.Identity.Server.Config
{
  public static class CustomScopes
  {
    public static IEnumerable<Scope> Get()
    {
      return new List<Scope>
      {
        StandardScopes.OpenId,
        StandardScopes.ProfileAlwaysInclude,
        StandardScopes.Address,
        new Scope
        {
          Name = "gallerymanagement",
          DisplayName = "Gallery Management",
          Description = "Allow the application to manage galleries on your behalf.",
          Type = ScopeType.Resource,
          Claims = new List<ScopeClaim>()
          {
            new ScopeClaim("role", false)
          },
        },
        new Scope
        {
          Name = "roles",
          DisplayName = "Role(s)",
          Description = "Allow the application to see your role(s).",
          Type = ScopeType.Identity,
          Claims = new List<ScopeClaim>()
          {
            new ScopeClaim("role", true)
          }
        },
        StandardScopes.OfflineAccess
      };
    }
  }
}
