using DBSP.RememberMe.Identity.Server.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace DBSP.RememberMe.Identity.Server.Controllers
{
  public class CreateUserAccountController : Controller
  {
    private ApplicationUserManager _userManager;

    public CreateUserAccountController() { }

    public CreateUserAccountController(ApplicationUserManager userManager)
    {
      UserManager = userManager;
    }

    public ApplicationUserManager UserManager
    {
      get
      {
        return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
      }
      private set
      {
        _userManager = value;
      }
    }

    // GET: CreateUserAccount
    [HttpGet]
    public ActionResult Index(string signin)
    {
      return View(new CreateUserAccountModel());
    }

    [HttpPost]
    public ActionResult Index(string signin, CreateUserAccountModel model)
    {
      if (ModelState.IsValid)
      {
        // create a user
        var password = HashHelper.Sha512(model.UserName + model.Password);

        var user = new ApplicationUser
        {
          UserName = model.UserName,
          IsActive = true,
          Password = password,
          Email = model.Email
        };
        var createdUser = UserManager.Create(user, model.Password);

        if (createdUser.Succeeded)
        {
          var userRetrieved = UserManager.Users
            .FirstOrDefault(u => u.UserName == model.UserName && u.Password == password);

          if (userRetrieved != null)
          {
            UserManager.AddClaim(userRetrieved.Id,
              new Claim(IdentityServer3.Core.Constants.ClaimTypes.GivenName, model.FirstName));
            UserManager.AddClaim(userRetrieved.Id,
              new Claim(IdentityServer3.Core.Constants.ClaimTypes.FamilyName, model.LastName));
            UserManager.AddClaim(userRetrieved.Id,
              new Claim("role", model.Role));
            UserManager.AddClaim(userRetrieved.Id,
              new Claim("address", model.Address));
            UserManager.AddClaim(userRetrieved.Id,
              new Claim("email", model.Email));
          }
        }
        // redirect to the login page, passing in 
        // the signin parameter
        return Redirect("~/identity/" + IdentityServer3.Core.Constants.RoutePaths.Login + "?signin=" + signin);
      }
      return View();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && _userManager != null)
      {
        _userManager.Dispose();
        _userManager = null;
      }

      base.Dispose(disposing);
    }
  }
}