using Data;
using Data.OWIN;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace WebUI.App_Start
{
    public class IdentityConfig
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext(() => new ShoppingCartContext());
            app.CreatePerOwinContext<AppUserManager>(AppUserManager.Create);

            app.CreatePerOwinContext<RoleManager<AppRole>>((options, context) =>
                new RoleManager<AppRole>(
                    new RoleStore<AppRole>(ShoppingCartContext.Create())));

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
            });

            AddUserRoles();
        }

        private void AddUserRoles()
        {
            ShoppingCartContext context = new ShoppingCartContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<AppUser>(new UserStore<AppUser>(context));

            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                var passwordHash = new PasswordHasher();
                string password = passwordHash.HashPassword("password123A");
                var user = new AppUser();
                //THIS is a serious BUG in Identity 2 username must be equal to Email
                user.UserName = "Admin@Admin.com";
                user.Email = "Admin@Admin.com";
                string userPWD = "password123A";

                var chkUser = UserManager.Create(user, userPWD);

                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");
                }
            }

            if (!roleManager.RoleExists("User"))
            {
                var urole = new IdentityRole();
                urole.Name = "User";
                roleManager.Create(urole);
            }
        }
    }
}