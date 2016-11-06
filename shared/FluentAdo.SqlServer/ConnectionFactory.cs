
using System.Configuration;
using System.Data.SqlClient;

namespace FluentAdo.SqlServer
{
    /// <summary>
    /// This is a template for creating a connection factor.  It is not 
    /// required, but it does make your life easier if you do.
    /// </summary>
    public static class ConnectionFactory
    {
        public static SqlConnection GetConnection()
        {
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString);
            sqlConnection.Open();
            return sqlConnection;
        }
    }
}