namespace DBSP.RememberMe.Identity.Server.Migrations
{
  using Microsoft.AspNet.Identity;
  using Microsoft.AspNet.Identity.EntityFramework;
  using System.Data.Entity.Migrations;
  using System.Linq;
  using System.Security.Claims;
  internal sealed class Configuration : DbMigrationsConfiguration<DBSP.RememberMe.Identity.Server.ApplicationDbContext>
  {
    public Configuration()
    {
      AutomaticMigrationsEnabled = false;
    }

    protected override void Seed(ApplicationDbContext context)
    {
      var store = new UserStore<ApplicationUser>(context);
      //var manager = new UserManager<ApplicationUser>(store);
      var manager = new ApplicationUserManager(store);

      if (!context.Users.Any(u => u.UserName == "Kevin"))
      {
        // Using a custom encrypted password.
        var password = HashHelper.Sha512("Kevin" + "secret");

        var user = new ApplicationUser
        {
          UserName = "Kevin",
          IsActive = true,
          Id = "b05d3546-6ca8-4d32-b95c-77e94d705ddf",
          Password = password
        };
        manager.Create(user, "secret");
      }

      if (context.Users.Any(u => u.UserName == "Kevin"))
      {
        var user = manager.Users.FirstOrDefault(u => u.UserName == "Kevin");

        if (!user.Claims.Any(c => c.ClaimType == "given_name"))
        {
          manager.AddClaim(user.Id, new Claim("given_name", "Kevin"));
        }

        if (!user.Claims.Any(c => c.ClaimType == "family_name"))
        {
          manager.AddClaim(user.Id, new Claim("family_name", "Dockx"));
        }

        if (!user.Claims.Any(c => c.ClaimType == "address"))
        {
          manager.AddClaim(user.Id, new Claim("address", "1, Main Street, Antwerp, Belgium"));
        }

        if (!user.Claims.Any(c => c.ClaimType == "role"))
        {
          manager.AddClaim(user.Id, new Claim("role", "PayingUser"));
        }

        if (!user.Claims.Any(c => c.ClaimType == "email"))
        {
          manager.AddClaim(user.Id, new Claim("email", "kevin.dockx@gmail.com"));
        }
      }
    }
  }
}
