using FluentAdo.SqlServer;
using KServices.Core.Domain.Data.Entities;
using KServices.Core.Domain.Data.Repositories;

namespace KServices.Data.Repository.SqlRepository
{
    public class PersonRepository : BaseRepository, IPersonRepository
    {
        public Person Get(string userName)
        {
            const string sql = @"select * 
                                from 
                                where username = @username and password = @password";

            using (FluentCommand<Person> command = CreateSql<Person>(sql))
            {
                return command
                    .AddString("userName", userName)
                    .SetMap(MapPerson()).AsSingle();
            }
        }

        private static FluentCommand<Person>.ResultMapDelegate MapPerson()
        {
            return x => new Person
            {
                Id = x.GetInt("Id"),
                FirstName = x.GetStringNullable("FirstName"),
                MiddleName = x.GetStringNullable("MiddleName"),
                LastName = x.GetStringNullable("LastName"),
                MobilePhone1 = x.GetStringNullable("MobilePhone1"),
                MobilePhone2 = x.GetStringNullable("MobilePhone2"),
                MobilePhone3 = x.GetStringNullable("MobilePhone3"),
                HomePhone = x.GetStringNullable("HomePhone")
            };
        }
    }
}