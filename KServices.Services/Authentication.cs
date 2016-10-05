using System;
using KServices.Core.Domain.Data.Entities;
using KServices.Core.Domain.Data.Specification;
using KServices.Core.Domain.Services;
using MedTeam.Data.Core.Domain.Data;

namespace KServices.Services
{
    class Authentication : IAuthentication
    {
        private readonly IRepository<Account> _accountRepository;

        public Authentication(IRepository<Account> accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public bool Authenticate(string account, string passwordHesh)
        {
           var item = _accountRepository.Find.One(AccountSpecifications.ByAccount(account).And(AccountSpecifications.ByPassword(passwordHesh)));
            return item != null;
        }

        public bool Authenticate(string account)
        {
            var item = _accountRepository.Find.One(AccountSpecifications.ByAccount(account));
            return item != null;
        }
    }
}
