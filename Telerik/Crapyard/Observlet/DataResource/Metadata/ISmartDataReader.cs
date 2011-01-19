using System;

namespace DataResource.Metadata
{
    public interface ISmartDataReader
    {
        decimal GetDecimal(string column);
        decimal GetDecimal(string column, decimal defaultValue);
        int GetInt32(string column);
        int GetInt32(string column, int defaultValue);
        long GetInt64(string column);
        long GetInt64(string column, long defaultValue);
        short GetInt16(string column);
        short GetInt16(string column, short defaultValue);
        float GetFloat(string column);
        short? GetNullableInt16(string column);
        int? GetNullableInt32(string column);
        long? GetNullableInt64(string column);
        float GetFloat(string column, float defaultValue);
        double GetDouble(string column);
        double? GetNullableDouble(string column);
        double GetDouble(string column, double defaultValue);
        bool GetBoolean(string column);
        bool GetBoolean(string column, bool defaultValue);
        string GetString(string column);
        string GetString(string column, string defaultValue);
        DateTime GetDateTime(string column);
        bool? GetNullableBoolean(string column);
        DateTime? GetNullableDateTime(string column);
        DateTime GetDateTime(string column, DateTime defaultValue);
        bool Read();
        Object GetObject(string column);
    }
}