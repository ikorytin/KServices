using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KServices.Core.Domain.Data.Entities;
using KServices.Models;
using Microsoft.AspNet.Identity;

namespace KServices.Services.User
{
    public class AccountStore : IUserStore<Account, int>
    {
        static readonly List<Account> users = new List<Account>();

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public Task CreateAsync(Account user)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateAsync(Account user)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteAsync(Account user)
        {
            throw new System.NotImplementedException();
        }

        public Task<Account> FindByIdAsync(int userId)
        {
            return Task.Run(() => new Account());  
        }

        public Task<Account> FindByNameAsync(string userName)
        {
            return Task<Account>.Factory.StartNew(() => Users.FirstOrDefault(u => u.UserName == userName));
        }
    }
}