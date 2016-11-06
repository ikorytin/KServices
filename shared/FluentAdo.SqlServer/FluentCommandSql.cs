using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

using NLog;

namespace FluentAdo.SqlServer
{
    public partial class FluentCommand<T> : IDisposable
    {
        #region Constants and Fields

        protected readonly DbCommand Command;

        protected ResultMapDelegate Mapper;

        /// <summary>
        /// The _logger.
        /// </summary>
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

        #endregion

        #region Constructors and Destructors

        public FluentCommand()
        {
            Command = CreateCommand();
            Command.Connection = CreateConnection();
        }

        public FluentCommand(string commandText, DbConnection connection)
        {
            Command = CreateCommand();
            Command.Connection = connection;
            Command.CommandText = commandText;
        }

        public FluentCommand(string commandText)
            : this()
        {
            Command.CommandText = commandText;
        }

        #endregion

        #region Delegates

        public delegate T ResultMapDelegate(DataReader reader);

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the result in a DataReader
        /// </summary>
        /// <returns></returns>
        public DbDataReader AsDataReader()
        {
            _logger.Debug("Executing SQL: {0}", GetDebugString());
            using (Command)
            using (DbDataReader reader = Command.ExecuteReader())
            {
                return reader;
            }
        }

        /// <summary>
        /// Returns the result as a typed IEnumerable
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> AsEnumerable()
        {
            if (Mapper == null)
            {
                throw new NullReferenceException("You must call SetMap to use AsList");
            }

            _logger.Debug("Executing SQL: {0}", GetDebugString());

            using (Command)
            using (DbDataReader reader = Command.ExecuteReader())
            {
                var myReader = new DataReader(reader);
                while (reader.Read())
                {
                    T data = Mapper.Invoke(myReader);
                    yield return data;
                }
            }
        }

        public IEnumerable<T> AsEnumerable(ResultMapDelegate map)
        {
            Mapper = map;
            return AsEnumerable();
        }

        /// <summary>
        /// Returns the result in a typed List
        /// </summary>
        /// <returns></returns>
        public List<T> AsList()
        {
            if (Mapper == null)
            {
                throw new NullReferenceException("You must call SetMap to use AsList");
            }

            _logger.Debug("Executing SQL: {0}", GetDebugString());
            var returnList = new List<T>();
            using (Command)
            using (DbDataReader reader = Command.ExecuteReader())
            {
                var myReader = new DataReader(reader);
                do
                {
                    while (reader.Read())
                    {
                        T data = Mapper.Invoke(myReader);
                        returnList.Add(data);
                    }    
                }
                while (reader.NextResult());                               
            }
            return returnList;
        }

        public IList<T> AsList(ResultMapDelegate map)
        {
            Mapper = map;
            return AsList();
        }

        /// <summary>
        /// Executes the query as a NonQuery
        /// </summary>
        /// <returns></returns>
        public int AsNonQuery()
        {
            _logger.Debug("Executing SQL: {0}", GetDebugString());            
            using (Command)
            {
                return Command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Executes the query as a NonQuery. This is typically used for UPDATE, INSERT, and DELETE statements.
        /// This version of AsNonQuery does not destroy the command object, so it can be called multiple times.
        /// Each time it is called the parameters are reset from the param list passed in.  The values in the list must be 
        /// added in the same order that the parameters were initialy added.
        /// Use this version of AsScalar in high performance situations.
        /// </summary>
        /// <param name="list">New parameter values.</param>
        /// <returns></returns>
        public int AsNonQuery(params object[] list)
        {
            ResetParams(list);
            _logger.Debug("Executing SQL: {0}", GetDebugString());
            return Command.ExecuteNonQuery();
        }

        /// <summary>
        /// Returns the command results as a Scalar value
        /// </summary>
        /// <returns></returns>
        public T AsScalar()
        {
            _logger.Debug("Executing SQL: {0}", GetDebugString());
            using (Command)
            {
                return (T)Command.ExecuteScalar();
            }
        }

        /// <summary>
        /// Returns the command results as a Scalar value
        /// </summary>
        /// <returns></returns>
        public TScalar AsTypeScalar<TScalar>()
        {
            _logger.Debug("Executing SQL: {0}", GetDebugString());
            using (Command)
            {
                return (TScalar)Command.ExecuteScalar();
            }
        }

        /// <summary>
        /// Executes the query using ExecuteScaler.  
        /// This version of AsScalar does not destroy the command object, so it can be called multiple times.
        /// Each time it is called the parameters are reset from the param list passed in.  The values in the list must be 
        /// added in the same order that the parameters were initialy added.  
        /// Use this version of AsScalar in high performance situations.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public T AsScalar(params object[] list)
        {
            ResetParams(list);
            return (T)Command.ExecuteScalar();
        }

        /// <summary>
        /// Returns the first result
        /// </summary>
        /// <returns></returns>
        public T AsSingle()
        {
            if (Mapper == null)
            {
                throw new NullReferenceException("You must call SetMap to use AsList");
            }

            _logger.Debug("Executing SQL: {0}", GetDebugString());

            using (Command)
            using (DbDataReader reader = Command.ExecuteReader())
            {
                var myReader = new DataReader(reader);
                if (reader.Read())
                {
                    return Mapper.Invoke(myReader);
                }
            }
            return default(T);
        }

        /// <summary>
        /// Executes the command and returns the first (and only the first) result.   
        /// This version of AsSingle does not destroy the command object, so it can be called multiple times.
        /// Each time it is called the parameters are reset from the param list passed in.  The values in the list must be 
        /// added in the same order that the parameters were initialy added.  
        /// Use this version of AsScalar in high performance situations.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public T AsSingle(params object[] list)
        {
            if (Mapper == null)
            {
                throw new NullReferenceException("You must call SetMap to use AsList");
            }

            ResetParams(list);
            _logger.Debug("Executing SQL: {0}", GetDebugString());

            using (DbDataReader reader = Command.ExecuteReader())
            {
                var myReader = new DataReader(reader);
                if (reader.Read())
                {
                    T data = Mapper.Invoke(myReader);
                    return data;
                }
            }
            return default(T);
        }

        public void Dispose()
        {
            Command.Connection.Close();
            Command.Dispose();
        }

        public FluentCommand<T> SetCommandTimeOut(int seconds)
        {
            Command.CommandTimeout = seconds;
            return this;
        }

        public FluentCommand<T> SetCommandType(CommandType commandType)
        {
            Command.CommandType = commandType;
            return this;
        }

        public FluentCommand<T> SetConnection(DbConnection connection)
        {
            Command.Connection = connection;
            return this;
        }

        /// <summary>
        /// Takes the function that maps the DataReader to an actual object
        /// </summary>
        /// <param name="resultMapDelegate"></param>
        /// <returns></returns>
        public FluentCommand<T> SetMap(ResultMapDelegate resultMapDelegate)
        {
            Mapper = resultMapDelegate;
            return this;
        }

        public FluentCommand<T> SetTransaction(DbTransaction transaction)
        {
            Command.Transaction = transaction;
            return this;
        }

        #endregion

        #region Methods

        protected DbCommand CreateCommand()
        {
            var command = new SqlCommand { CommandTimeout = 60 };
            return command;
        }

        protected DbConnection CreateConnection()
        {
            return ConnectionFactory.GetConnection();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected object SetGuidParamValue(Guid value)
        {
            if (value == Guid.Empty)
            {
                return DBNull.Value;
            }
            return value;
        }

        /// <summary>
        /// SetParamValue is used when setting parameter values 
        /// and you want to ensure that a null value is set to DBNull.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected object SetParamValue(object value)
        {
            return value ?? DBNull.Value;
        }

        /// <summary>
        /// Returns all parameters in string format. It's used only for logging
        /// </summary>
        /// <returns></returns>
        private string GetDebugString()
        {            
            if (Command.CommandType == CommandType.Text)
            {
                string text = Command.CommandText;                
                foreach (DbParameter parameter in Command.Parameters)
                {
                    parameter.ParameterName = parameter.ParameterName.Replace("@", string.Empty);
                    text = text.Replace(string.Format("@{0}", parameter.ParameterName), string.Format("'{0}'", parameter.Value));
                }

                return text;
            }

            var sb = new StringBuilder();            
            if (Command.Parameters.Count > 0)
            {
                foreach (DbParameter parameter in Command.Parameters)
                {
                    parameter.ParameterName = parameter.ParameterName.Replace("@", string.Empty);
                    sb.AppendFormat("@{0}='{1}', ", parameter.ParameterName, parameter.Value);
                }

                sb.Remove(sb.Length - 2, 2);
            }

            return string.Format("{0} {1}", Command.CommandText, sb);
        }

        /// <summary>
        /// ResetParams is to be used with AsNonQuery and AsScalar to reset the command parameter values.
        /// The values must be passed in the same order that they were created.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private void ResetParams(params object[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                Command.Parameters[i].Value = SetParamValue(list[i]);
            }
        }

        #endregion
    }
}