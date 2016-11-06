using KServices.Core.Domain.Data.Entities;

namespace KServices.Core.Domain.Data.Repositories
{
    public interface IPersonRepository
    {
        Person Get(string userName);
    }
}