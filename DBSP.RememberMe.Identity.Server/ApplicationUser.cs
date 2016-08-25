using Microsoft.AspNet.Identity.EntityFramework;

namespace DBSP.RememberMe.Identity.Server
{
  public class ApplicationUser : IdentityUser
  {
    public bool IsActive { get; set; }
    public string Password { get; set; }
  }
}

