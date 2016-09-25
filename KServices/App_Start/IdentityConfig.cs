using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using KServices.Models;

namespace KServices
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public override Task<ApplicationUser> FindAsync(string userName, string password)
        {
            Task<ApplicationUser> taskInvoke = Task<ApplicationUser>.Factory.StartNew(() =>
            {
                //PasswordVerificationResult result = this.PasswordHasher.VerifyHashedPassword(userName, password);
                //if (result == PasswordVerificationResult.SuccessRehashNeeded)
                //{
                return new ApplicationUser
                {
                    Email = "Test@mail.ru",
                    PasswordHash = "0YLQtdGB0YI=",
                    UserName = userName
                    ,
                    SecurityStamp = Guid.NewGuid().ToString()

                }; //Store.FindByNameAsync(userName).Result;
                //}

                return null;
            });

            return taskInvoke;
        }

        //public override Task<ClaimsIdentity> CreateIdentityAsync(ApplicationUser user, string authenticationType)
        //{
        //    ClaimsIdentity identity = new ClaimsIdentity();
        //    return Task.FromResult(identity);
        //}

        //public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        //{
        //    var manager = new ApplicationUserManager(new AccountStore<ApplicationUser>());

        //    var dataProtectionProvider = options.DataProtectionProvider;
        //    if (dataProtectionProvider != null)
        //    {
        //        manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
        //    }
        //    return manager;
        //}
    }
}
