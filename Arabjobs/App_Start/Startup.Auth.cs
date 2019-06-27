using System;
using Arabjobs.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace Arabjobs
{
    public partial class Startup
    {
        private ApplicationDbContext db= new ApplicationDbContext();
        // For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            CreateDefaultRolesAndUsers();
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });            
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            //app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});


        }

        public void CreateDefaultRolesAndUsers()
        {   //RoleManager allow us to mange the roles thatis in identityrole(ده بيمثل جدول ال role in database)  (Rolestore)ويقوم بالتخزين علي مستوي الداتابيز باستخدام  
            var roleManger =new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));//IdentityRole ده المتخصص ف جدول الرولز ف الداتابيز
            var userManger = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));//ApplicationUserده المتخصص ف جدول اليوزر ف الداتابيز
            IdentityRole role =new IdentityRole();
            if (!roleManger.RoleExists("Admins"))
            {
                role.Name = "Admins";
                roleManger.Create(role);
                ApplicationUser user = new ApplicationUser();
                user.UserName = "Abdallah";
                user.Email = "abdallahkhateb66@gmail.com";
                var check = userManger.Create(user,"asd01277670167");//عمليك الكريشن 
                if (check.Succeeded)// عمليك التحقق من الكريشن اذا تم بنجاح نضيف اليوزر الى الروول
                {
                    userManger.AddToRoles(user.Id, "Admins");//عملية اضافة اليوزر الى الرول في جدول الداتابيز )(many to many rel table)تخ
                }



            }
           
        }
    }
}