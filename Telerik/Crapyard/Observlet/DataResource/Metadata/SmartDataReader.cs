using System;
using System.Data.SqlClient;
using System.Diagnostics;

namespace DataResource.Metadata
{
    public class SmartDataReader : ISmartDataReader
    {
        private DateTime defaultDate;
        private SqlDataReader reader;

        public SmartDataReader(SqlDataReader  reader)
        {
            defaultDate = DateTime.MinValue;
            this.reader = reader;
        }

        public decimal GetDecimal(string column)
        {
            return GetDecimal(column, 0);
        }

        public decimal GetDecimal(string column, decimal defaultValue)
        {
            decimal data = (reader.IsDBNull(reader.GetOrdinal(column)))
                               ? defaultValue : (decimal)reader[column];
            return data;
        }

        public int GetInt32(string column)
        {
            return GetInt32(column, 0);
        }

        public int GetInt32(string column, int defaultValue)
        {
            int data = (reader.IsDBNull(reader.GetOrdinal(column)))
                           ? defaultValue : (int)reader[column];
            return data;
        }

        public long GetInt64(string column)
        {
            return GetInt64(column, 0);
        }

        public long GetInt64(string column, long defaultValue)
        {
            long data = (reader.IsDBNull(reader.GetOrdinal(column)))
                            ? defaultValue : (long)reader[column];
            return data;
        }

        public short GetInt16(string column)
        {
            return GetInt16(column, 0);
        }

        public short GetInt16(string column, short defaultValue)
        {
            short data = (reader.IsDBNull(reader.GetOrdinal(column)))
                             ? defaultValue : (short)reader[column];
            return data;
        }

        public float GetFloat(string column)
        {
            return GetFloat(column, 0);
        }

        // Nullable overload
        public Nullable<short> GetNullableInt16(string column)
        {
            if (reader.IsDBNull(reader.GetOrdinal(column)))
                return null;
            return short.Parse(reader[column].ToString());
        }

        public Nullable<int> GetNullableInt32(string column)
        {
            if (reader.IsDBNull(reader.GetOrdinal(column)))
                return null;
            return int.Parse(reader[column].ToString());
        }

        public Nullable<long> GetNullableInt64(string column)
        {
            if (reader.IsDBNull(reader.GetOrdinal(column)))
                return null;
            return long.Parse(reader[column].ToString());
        }

        public float GetFloat(string column, float defaultValue)
        {
            float data = (reader.IsDBNull(reader.GetOrdinal(column)))
                             ? defaultValue : (float)reader[column];
            return data;
        }

        public double GetDouble(string column)
        {
            return GetDouble(column, 0);
        }

        // Nullable overload
        public Nullable<double> GetNullableDouble(string column)
        {
            if (reader.IsDBNull(reader.GetOrdinal(column)))
                return null;
            return (double)reader[column];
        }

        // double.Parse(val, CultureInfo.CreateSpecificCulture("en-US")
        public double GetDouble(string column, double defaultValue)
        {
            double data = (reader.IsDBNull(reader.GetOrdinal(column)))
                              ? defaultValue : (double)reader[column];
            return data;
        }

        public bool GetBoolean(string column)
        {
            return GetBoolean(column, false);
        }

        public bool GetBoolean(string column, bool defaultValue)
        {
            bool data = (reader.IsDBNull(reader.GetOrdinal(column)))
                            ? defaultValue : (bool)reader[column];
            return data;
        }

        public string GetString(string column)
        {
            return GetString(column, null);
        }

        public string GetString(string column, string defaultValue)
        {
            string data = null;                             // 2009-3-12 Columnnaam kan mogelijk niet voorkomen!
            int ordinal = reader.GetOrdinal(column);
            if (ordinal > -1)
            {
                data = (reader.IsDBNull(ordinal))
                           ? defaultValue : reader[column].ToString();
            }
            return data;
        }

        public DateTime GetDateTime(string column)
        {
            return GetDateTime(column, defaultDate);
        }

        //
        // Nullable overloads
        //

        public Nullable<bool> GetNullableBoolean(string column)
        {
            if (reader.IsDBNull(reader.GetOrdinal(column)))
                return null;
            return bool.Parse(reader[column].ToString());
        }

        public Nullable<DateTime> GetNullableDateTime(string column)
        {
            if (reader.IsDBNull(reader.GetOrdinal(column)))
                return null;
            DateTime datetime = (DateTime)reader[column];
            return datetime;
        }

        public DateTime GetDateTime(string column, DateTime defaultValue)
        {
            DateTime data = (reader.IsDBNull(reader.GetOrdinal(column)))
                                ? defaultValue : (DateTime)reader[column];
            return data;
        }

        public bool Read()
        {
            return reader.Read();
        }

        public Object GetObject(string column)
        {
            //string nullValue = "null";
            int ordinal = 0;
            Object returnObject;

            try
            {
                ordinal = reader.GetOrdinal(column);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
            finally
            {
                if (ordinal == -1 || reader.IsDBNull(ordinal))
                    returnObject = null;
                else
                    returnObject = reader[column];
            }

            return returnObject;
        }

    }
}