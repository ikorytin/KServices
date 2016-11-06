using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using FluentAdo.SqlServer;

namespace KServices.Data.Repository.SqlRepository
{
    public class BaseRepository 
    {
        private readonly string _connectionString;

        public BaseRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DBDTConnectionString"].ConnectionString;
        }

        public BaseRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private DbConnection CreateConnection()
        {
            SqlConnection sqlConnection = new SqlConnection(_connectionString);
            sqlConnection.Open();

            return sqlConnection;
        }

        protected FluentCommand<T> CreateCommand<T>(string commandText, CommandType commandType = CommandType.StoredProcedure)
        {
            return new FluentCommand<T>(commandText, CreateConnection()).SetCommandType(commandType);
        }

        protected FluentCommand<T> CreateSql<T>(string commandText, CommandType commandType = CommandType.Text)
        {
            return new FluentCommand<T>(commandText, CreateConnection()).SetCommandType(commandType);
        }
    }
}