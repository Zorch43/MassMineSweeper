using Antlr.Runtime.Misc;
using MassMineSweeper.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

[assembly: OwinStartupAttribute(typeof(MassMineSweeper.Startup))]
namespace MassMineSweeper
{
    public partial class Startup
    {
        public static Func<UserManager<Member>> UserManagerFactory { get; private set; }

        public void Configuration(IAppBuilder app)
        {           
            // this is the same as before
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/auth/login")
            });

            // configure the user manager
            UserManagerFactory = () =>
            {
                var usermanager = new UserManager<Member>(
                    new UserStore<Member>(new MassMineSweeperContext()));
                // allow alphanumeric characters in username
                usermanager.UserValidator = new UserValidator<Member>(usermanager)
                {
                    AllowOnlyAlphanumericUserNames = false
                };

                return usermanager;
            };
        }
    }
}
