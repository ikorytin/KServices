using System.Collections.Generic;
using FluentAdo.SqlServer;
using KServices.Core.Domain.Data.Entities;
using KServices.Core.Domain.Data.Repositories;

namespace KServices.Data.Repository.SqlRepository
{
    public class EntityRepository : BaseRepository, IEntityRepository
    {
        public List<Entity> Get(string account)
        {
            const string sql = @"select * 
                                from 
                                where username = @username and password = @password";

            using (FluentCommand<Entity> command = CreateSql<Entity>(sql))
            {
                var items = command
                    .AddString("account", account)
                    .SetMap(MapEntity()).AsList();

                foreach (var entity in items)
                {
                    entity.Owners = GetConactPerson(entity.Id);
                }

                return items;
            }
        }

        public List<PersonShortInfo> GetConactPerson(int accountId)
        {
            const string sql = @"select * 
                                from 
                                where username = @username and password = @password";

            using (FluentCommand<PersonShortInfo> command = CreateSql<PersonShortInfo>(sql))
            {
                var items = command
                    .AddInt("account", accountId)
                    .SetMap(MapPersonShortInfo()).AsList();

                return items;
            }
        }

        private static FluentCommand<PersonShortInfo>.ResultMapDelegate MapPersonShortInfo()
        {
            return x => new PersonShortInfo
            {
                FullName = x.GetStringNullable("FullName"),
                CellPhone1 = x.GetStringNullable("CellPhone1"),
                CellPhone2 = x.GetStringNullable("CellPhone2"),
                CellPhone3 = x.GetStringNullable("CellPhone3"),
                CellPhone4 = x.GetStringNullable("CellPhone4")
            };
        }

        private static FluentCommand<Entity>.ResultMapDelegate MapEntity()
        {
            return x => new Entity
            {
                Id = x.GetInt("Id"),
                Address = x.GetStringNullable("Address"),
                ContactDate = x.GetDateTime("ContactDate"),
                Type = x.GetStringNullable("Type"),
                СontractNumber = x.GetStringNullable("СontractNumber")
            };
        }
    }
}