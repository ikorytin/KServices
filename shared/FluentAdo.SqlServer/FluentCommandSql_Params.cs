using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace FluentAdo.SqlServer
{
    public partial class FluentCommand<T>
    {
        private const string TOTAL_ROW_COUNT_PARAM = "TotalRowCount";

        #region Public Methods

        public FluentCommand<T> AddPaging(int pageIndex, int pageSize)
        {
            int firstRowToShow = ((pageIndex - 1) * pageSize) + 1;
            int lastRowToShow = firstRowToShow + pageSize - 1;            

            return AddInt("FirstRowToShow", firstRowToShow)
            .AddInt("LastRowToShow", lastRowToShow)
            .AddInt(TOTAL_ROW_COUNT_PARAM, ParameterDirection.Output);
        }

        public int GetTotalCount()
        {
            return Convert.ToInt32(GetParamValue(TOTAL_ROW_COUNT_PARAM));
        }

        public FluentCommand<T> Add<TV>(string name, TV value)
        {
            var param = new SqlParameter(name, value);
            Command.Parameters.Add(param);
            return this;
        }

        public FluentCommand<T> AddBoolean(string name, ParameterDirection direction = ParameterDirection.Input)
        {
            CreateParam(name, SqlDbType.Bit, direction);
            return this;
        }

        public FluentCommand<T> AddBoolean(string name, bool value)
        {
            CreateParam(name, SqlDbType.Bit, value);
            return this;
        }

        public FluentCommand<T> AddDateTime(string name)
        {
            CreateParam(name, SqlDbType.DateTime);
            return this;
        }

        public FluentCommand<T> AddDateTime(string name, DateTime value)
        {
            CreateParam(name, SqlDbType.DateTime, value);
            return this;
        }

        public FluentCommand<T> AddDateTime(string name, DateTime? value)
        {
            CreateParam(name, SqlDbType.DateTime, value);
            return this;
        }

        public FluentCommand<T> AddDateTimeOffset(string name)
        {
            CreateParam(name, SqlDbType.DateTimeOffset);
            return this;
        }

        public FluentCommand<T> AddDateTimeOffset(string name, DateTimeOffset value)
        {
            CreateParam(name, SqlDbType.DateTimeOffset, value);
            return this;
        }

        public FluentCommand<T> AddDateTimeOffset(string name, DateTimeOffset? value)
        {
            CreateParam(name, SqlDbType.DateTimeOffset, value);
            return this;
        }

        public FluentCommand<T> AddDateTime2(string name, DateTime value)
        {
            CreateParam(name, SqlDbType.DateTime2, value);
            return this;
        }

        public FluentCommand<T> AddDateTime2(string name, DateTime? value)
        {
            CreateParam(name, SqlDbType.DateTime2, value);
            return this;
        }

        public FluentCommand<T> AddTime(string name)
        {
            CreateParam(name, SqlDbType.Time);
            return this;
        }

        public FluentCommand<T> AddTime(string name, TimeSpan value)
        {
            CreateParam(name, SqlDbType.Time, value);
            return this;
        }

        public FluentCommand<T> AddTime(string name, TimeSpan? value)
        {
            CreateParam(name, SqlDbType.Time, value);
            return this;
        }

        public FluentCommand<T> AddDecimal(string name)
        {
            CreateParam(name, SqlDbType.Decimal);
            return this;
        }

        public FluentCommand<T> AddDecimal(string name, decimal value)
        {
            CreateParam(name, SqlDbType.Decimal, value);
            return this;
        }

        public FluentCommand<T> AddDecimal(string name, decimal? value)
        {
            CreateParam(name, SqlDbType.Decimal, value);
            return this;
        }

        public FluentCommand<T> AddGuid(string name)
        {
            CreateParam(name, SqlDbType.UniqueIdentifier);
            return this;
        }

        public FluentCommand<T> AddGuid(string name, Guid value)
        {
            DbParameter param = CreateParam(name, SqlDbType.UniqueIdentifier);
            param.Value = SetGuidParamValue(value);
            return this;
        }

        public FluentCommand<T> AddGuidId(string name)
        {
            DbParameter param = CreateParam(name, SqlDbType.UniqueIdentifier);
            param.Value = SetGuidParamValue(Guid.NewGuid());
            return this;
        }

        public FluentCommand<T> AddInt(string name, ParameterDirection direction = ParameterDirection.Input)
        {
            CreateParam(name, SqlDbType.Int, direction);
            return this;
        }

        public FluentCommand<T> AddInt(string name, int value)
        {
            CreateParam(name, SqlDbType.Int, value);
            return this;
        }

        public FluentCommand<T> AddInt(string name, int? value)
        {
            CreateParam(name, SqlDbType.Int, value);
            return this;
        }

        public FluentCommand<T> AddLong(string name, long value)
        {
            CreateParam(name, SqlDbType.BigInt, value);
            return this;
        }

        public FluentCommand<T> AddLong(string name, ParameterDirection direction = ParameterDirection.Input)
        {
            CreateParam(name, SqlDbType.BigInt, direction);
            return this;
        }

        public FluentCommand<T> AddDouble(string name)
        {
            CreateParam(name, SqlDbType.Float);
            return this;
        }

        public FluentCommand<T> AddDouble(string name, double value)
        {
            CreateParam(name, SqlDbType.Float, value);
            return this;
        }

        public FluentCommand<T> AddDouble(string name, double? value)
        {
            CreateParam(name, SqlDbType.Float, value);
            return this;
        }

        public FluentCommand<T> AddString(string name)
        {
            CreateParam(name, SqlDbType.NVarChar);
            return this;
        }

        public FluentCommand<T> AddString(string name, int size, string value)
        {
            CreateParam(name, SqlDbType.NVarChar, size);
            return this;
        }        
        
        public FluentCommand<T> AddString(string name, string value)
        {
            CreateParam(name, SqlDbType.NVarChar, value);
            return this;
        }

        public FluentCommand<T> AddString(string name, int size, ParameterDirection direction = ParameterDirection.Input)
        {            
            DbParameter param = CreateParam(name, SqlDbType.NVarChar, direction);
            param.Size = size;
            return this;
        }

        public FluentCommand<T> AddTable(string name, DataTable value)
        {
            CreateParam(name, SqlDbType.Structured, value);
            return this;
        }

        public DbParameter CreateParam(string name, SqlDbType dataType, object value)
        {
            var param = new SqlParameter(name, dataType) { Value = SetParamValue(value) };
            Command.Parameters.Add(param);
            return param;
        }
        
        public DbParameter CreateParam(string name, SqlDbType dataType, ParameterDirection direction = ParameterDirection.Input)
        {
            var param = new SqlParameter(name, dataType) { Direction = direction };
            Command.Parameters.Add(param);
            return param;
        }

        public object GetParamValue(string paramName)
        {
            return Command.Parameters[paramName].Value;
        }

        public FluentCommand<T> SetDirection(ParameterDirection direction)
        {
            if (Command.Parameters.Count == 0)
            {
                return this;
            }

            DbParameter parameter = Command.Parameters[Command.Parameters.Count - 1];
            parameter.Direction = direction;

            return this;
        }

        #endregion
    }
}