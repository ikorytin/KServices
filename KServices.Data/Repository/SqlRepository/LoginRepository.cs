using FluentAdo.SqlServer;
using KServices.Core.Domain.Data.Repositories;

namespace KServices.Data.Repository.SqlRepository
{
    public class LoginRepository : BaseRepository, ILoginRepository
    {
        public string Login(string userName, string password)
        {
            const string sql = @"select * 
                                from 
                                where username = @username and password = @password";

            using (FluentCommand<string> command = CreateSql<string>(sql))
            {
                return command
                    .AddString("userName", userName)
                    .AddString("password ", password)
                    .SetMap(x => x.GetString("test")).AsSingle();
            }
        }
    }
}