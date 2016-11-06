using System;
using KServices.Core.Domain.Data.Entities;
using KServices.Core.Domain.Data.Repositories;
using KServices.Core.Domain.Data.Specification;
using KServices.Core.Domain.Services;
using MedTeam.Data.Core.Domain.Data;

namespace KServices.Services
{
   public class Authentication : IAuthentication
    {
        private readonly IRepository<Account> _accountRepository;

       private readonly ILoginRepository _loginRepository;

        public Authentication(IRepository<Account> accountRepository, ILoginRepository loginRepository)
        {
            _accountRepository = accountRepository;
            _loginRepository = loginRepository;
        }

       public bool Authenticate(string account, string passwordHesh)
        {
            
           var item = _accountRepository.Find.One(AccountSpecifications.ByAccount(account).And(AccountSpecifications.ByPassword(passwordHesh)));
            return true; // item != null;
        }

        public bool Authenticate(string account)
        {
            var item = _accountRepository.Find.One(AccountSpecifications.ByAccount(account));
            return true;
            //return item != null;
        }
    }
}
