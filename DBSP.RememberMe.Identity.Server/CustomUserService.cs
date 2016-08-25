using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services.Default;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer3.Core.Extensions;
using System.Security.Claims;
using Microsoft.AspNet.Identity;

namespace DBSP.RememberMe.Identity.Server
{
  public class CustomUserService : UserServiceBase, IDisposable
  {
    private readonly UserManager<ApplicationUser> _userManager;

    public CustomUserService(UserManager<ApplicationUser> userManager)
    {
      _userManager = userManager;
    }

    public override Task AuthenticateLocalAsync(IdentityServer3.Core.Models.LocalAuthenticationContext context)
    {
      //var user = userRepository.GetUser(context.UserName, context.Password);

      // 1Q7v1oV7OXIn9Q1kgXayb2KYTjzO64mfxWV2N5K6szjmfNf77NPmyWCD5ZMALkj1VD/VKv8yOZvszDjrE7WXDg==
      var encryptedPassword = HashHelper.Sha512(context.UserName + context.Password);

      var user = _userManager.Users
        .FirstOrDefault(u => u.UserName == context.UserName &&
                             u.Password == encryptedPassword);

      if (user == null)
      {
        context.AuthenticateResult = new AuthenticateResult("Invalid credentials");
        return Task.FromResult(0);
      }

      context.AuthenticateResult = new AuthenticateResult(
          //user.Subject,
          user.Id,
          //user.UserClaims.First(c => c.ClaimType == Constants.ClaimTypes.GivenName).ClaimValue);
          user.Claims.First(c => c.ClaimType == IdentityServer3.Core.Constants.ClaimTypes.GivenName).ClaimValue);

      return Task.FromResult(0);
    }

    public override Task IsActiveAsync(IdentityServer3.Core.Models.IsActiveContext context)
    {
      if (context.Subject == null)
      {
        throw new ArgumentNullException("subject");
      }

      var subjectId = context.Subject.GetSubjectId();
      var user = _userManager.Users.FirstOrDefault(u => u.Id == subjectId);

      // set whether or not the user is active
      context.IsActive = (user != null) && user.IsActive;

      return Task.FromResult(0);
    }

    public override Task GetProfileDataAsync(IdentityServer3.Core.Models.ProfileDataRequestContext context)
    {
      // find the user
      var subjectId = context.Subject.GetSubjectId();
      var user = _userManager.Users.FirstOrDefault(u => u.Id == subjectId);

      // add subject as claim
      var claims = new List<Claim>
      {
        new Claim(IdentityServer3.Core.Constants.ClaimTypes.Subject, user.Id),
      };

      // add the other UserClaims
      //claims.AddRange(user.UserClaims.Select<UserClaim, Claim>(uc => new Claim(uc.ClaimType, uc.ClaimValue)));
      claims.AddRange(user.Claims.Select(uc => new Claim(uc.ClaimType, uc.ClaimValue)));

      // only return the requested claims
      if (!context.AllClaimsRequested)
      {
        claims = claims.Where(x => context.RequestedClaimTypes.Contains(x.Type)).ToList();
      }

      // set the issued claims - these are the ones that were requested, if available
      context.IssuedClaims = claims;

      return Task.FromResult(0);
    }

    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
      if (disposing && _userManager != null)
      {
        _userManager.Dispose();
      }
    }
  }
}
