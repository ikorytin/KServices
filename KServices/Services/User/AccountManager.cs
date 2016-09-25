using KServices.Core.Domain.Data.Entities;
using KServices.Models;
using Microsoft.AspNet.Identity;

namespace KServices.Services.User
{
    public class AccountManager : UserManager<AccountIdentity, int>
    {
        public AccountManager(IUserStore<AccountIdentity, int> store) 
            : base(store)
        {
            UserValidator = new UserValidator<Account, int>(this);
            PasswordValidator = new PasswordValidator();
        }
    }
}