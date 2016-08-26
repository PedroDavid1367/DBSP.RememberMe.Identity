using DBSP.RememberMe.Identity.Server.Config;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services;
using IdentityServer3.Core.Services.Default;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Owin;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace DBSP.RememberMe.Identity.Server
{
  public class Startup
  {
    public void Configuration(IAppBuilder app)
    {
      // Configure the db context and user manager to use a single instance per request
      app.CreatePerOwinContext(ApplicationDbContext.Create);
      app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

      app.Map("/identity", idsrvApp =>
      {
        var corsPolicyService = new DefaultCorsPolicyService()
        {
          AllowAll = true
        };

        var defaultViewServiceOptions = new DefaultViewServiceOptions();
        defaultViewServiceOptions.CacheViews = false;

        var idServerServiceFactory = new IdentityServerServiceFactory()
                              .UseInMemoryClients(CustomClients.Get())
                              .UseInMemoryScopes(CustomScopes.Get());
        //.UseInMemoryUsers(CustomUsers.Get());

        idServerServiceFactory.CorsPolicyService = new
                  Registration<IdentityServer3.Core.Services.ICorsPolicyService>(corsPolicyService);

        idServerServiceFactory.ConfigureDefaultViewService(defaultViewServiceOptions);

        idServerServiceFactory.Register(new Registration<ApplicationDbContext>());
        idServerServiceFactory.Register(new Registration<UserStore<ApplicationUser>>(resolver =>
        {
          return new UserStore<ApplicationUser>(resolver.Resolve<ApplicationDbContext>());
        }));
        idServerServiceFactory.Register(new Registration<UserManager<ApplicationUser>>(resolver =>
        {
          return new ApplicationUserManager(resolver.Resolve<UserStore<ApplicationUser>>());
        }));

        idServerServiceFactory.UserService = new Registration<IUserService, CustomUserService>();

        var options = new IdentityServerOptions
        {
          Factory = idServerServiceFactory,

          // Just for Angular 2 App testing.
          RequireSsl = false,

          SiteName = "TripCompany Security Token Service",
          SigningCertificate = LoadCertificate(),
          IssuerUri = DBSP.RememberMe.Identity.Constants.TripGalleryIssuerUri,
          PublicOrigin = DBSP.RememberMe.Identity.Constants.TripGallerySTSOrigin,
          AuthenticationOptions = new AuthenticationOptions()
          {
            EnablePostSignOutAutoRedirect = true,
            LoginPageLinks = new List<LoginPageLink>()
            {
              new LoginPageLink()
              {
                Type= "createaccount",
                Text = "Create a new account",
                Href = "~/createuseraccount"
              }
            }
          },
          CspOptions = new CspOptions()
          {
            Enabled = false
            // once available, leave Enabled at true and use:
            // FrameSrc = "https://localhost:44318 https://localhost:44316"
            // or
            // FrameSrc = "*" for all URI's.
          }
        };
        idsrvApp.UseIdentityServer(options);
      });
    }

    private X509Certificate2 LoadCertificate()
    {
      return new X509Certificate2(
          string.Format(@"{0}\certificates\idsrv3test.pfx",
          AppDomain.CurrentDomain.BaseDirectory), "idsrv3test");
    }
  }
}