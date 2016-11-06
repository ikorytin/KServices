using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;

namespace FluentAdo.SqlServer
{
    public class DataReader : IEnumerable
    {
        #region Constants and Fields

        private readonly Dictionary<string, int> _columnOrdinals = new Dictionary<string, int>();

        private readonly DbDataReader _reader;

        #endregion

        #region Constructors and Destructors

        public DataReader(DbDataReader reader)
        {
            _reader = reader;
        }

        #endregion

        #region Public Methods

        public bool GetBoolean(string name)
        {
            int iCol = GetColumn(name);
            return _reader.GetBoolean(iCol);
        }

        public bool GetBoolean(string name, bool defaultValue)
        {
            return GetBooleanNullable(name) ?? defaultValue;
        }

        public bool? GetBooleanNullable(string name)
        {
            int iCol = GetColumn(name);
            if (!_reader.IsDBNull(iCol))
            {
                return _reader.GetBoolean(iCol);
            }

            return null;
        }

        public int GetColumn(string name)
        {
            if (!_columnOrdinals.ContainsKey(name))
            {
                int i = _reader.GetOrdinal(name);
                _columnOrdinals.Add(name, i);
            }

            return _columnOrdinals[name];
        }

        public DateTime GetDateTime(string name)
        {
            int icol = GetColumn(name);
            return _reader.GetDateTime(icol);
        }

        public DateTime? GetDateTimeNullable(string name)
        {
            int iCol = GetColumn(name);
            if (!_reader.IsDBNull(iCol))
            {
                return _reader.GetDateTime(iCol);
            }

            return null;
        }

        public decimal GetDecimal(string name)
        {
            int iCol = GetColumn(name);
            return _reader.GetDecimal(iCol);
        }

        public decimal GetDecimal(string name, decimal defaultValue)
        {
            return GetDecimalNullable(name) ?? defaultValue;
        }

        public decimal? GetDecimalNullable(string name)
        {
            int iCol = GetColumn(name);
            if (!_reader.IsDBNull(iCol))
            {
                return _reader.GetDecimal(iCol);
            }

            return null;
        }

        public IEnumerator GetEnumerator()
        {
            return _reader.GetEnumerator();
        }

        public Guid GetGuid(string name)
        {
            int iCol = GetColumn(name);
            return _reader.GetGuid(iCol);
        }

        public Guid? GetGuidNullable(string name)
        {
            int iCol = GetColumn(name);
            if (!_reader.IsDBNull(iCol))
            {
                return _reader.GetGuid(iCol);
            }

            return null;
        }

        public int GetInt(string name)
        {
            int iCol = GetColumn(name);
            return _reader.GetInt32(iCol);
        }

        public int GetInt(string name, int defaultValue)
        {
            return GetIntNullable(name) ?? defaultValue;
        }

        public int? GetIntNullable(string name)
        {
            int iCol = GetColumn(name);
            if (!_reader.IsDBNull(iCol))
            {
                return _reader.GetInt32(iCol);
            }

            return null;
        }

        public double GetDouble(string name)
        {
            int iCol = GetColumn(name);
            return _reader.GetDouble(iCol);
        }

        public double GetDouble(string name, int defaultValue)
        {
            return GetDoubleNullable(name) ?? defaultValue;
        }

        public double? GetDoubleNullable(string name)
        {
            int iCol = GetColumn(name);
            if (!_reader.IsDBNull(iCol))
            {
                return _reader.GetDouble(iCol);
            }

            return null;
        }

        public string GetString(string name)
        {
            int iCol = GetColumn(name);
            return _reader.GetString(iCol).TrimEnd();
        }

        public string GetStringNullable(string name)
        {
            int iCol = GetColumn(name);
            if (!_reader.IsDBNull(iCol))
            {
                return _reader.GetString(iCol).TrimEnd();
            }

            return string.Empty;
        }

        public TimeSpan GetTimeSpan(string name)
        {
            int iCol = GetColumn(name);
            return (TimeSpan)_reader.GetValue(iCol);
        }

        public TimeSpan? GetTimeSpanNullable(string name)
        {
            int iCol = GetColumn(name);
            if (!_reader.IsDBNull(iCol))
            {
                return (TimeSpan)_reader.GetValue(iCol);
            }

            return null;
        }

        public T GetValue<T>(string name)
        {
            return (T)_reader.GetValue(GetColumn(name));
        }

        public bool HasColumn(string name)
        {
            for (int i = 0; i < _reader.FieldCount; i++)
            {
                if (_reader.GetName(i).Equals(name, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}