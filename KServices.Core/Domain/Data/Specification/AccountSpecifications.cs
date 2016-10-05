using KServices.Core.Domain.Core.Specification;
using KServices.Core.Domain.Data.Entities;
using MedTeam.Infrastructure.Specification;

namespace KServices.Core.Domain.Data.Specification
{
    public class AccountSpecifications
    {
        public static ISpecification<Account> ByPassword(string passwordHesh)
        {
            return new SingleSpecification<Account>(e => e.PasswordHash == passwordHesh);
        }

        public static ISpecification<Account> ByAccount(string account)
        {
            return new SingleSpecification<Account>(e => e.AccountNumber == account);
        }
    }
}