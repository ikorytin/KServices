using System;
using System.Collections.Generic;
using KServices.Core.Domain.Data.Entities;

namespace KServices.Core.Domain.Data.Repositories
{
    public interface IEntityRepository
    {
        List<Entity> Get(string account);

        List<PersonShortInfo> GetConactPerson(int accountId);
    }
}