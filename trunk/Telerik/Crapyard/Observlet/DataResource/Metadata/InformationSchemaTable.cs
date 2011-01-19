using System.Collections.Generic;

namespace DataResource.Metadata
{
    public interface IMetaDataSchema
    {
        string TableName { get; set; }
        Dictionary<string, MetaDataColumn> Columns { get; set; }
        Dictionary<string, object> Rows { get; set; }
    }

    public class MetaDataSchema : IMetaDataSchema
    {
        public MetaDataSchema(ISmartDataReader reader)
        {
            TableName = reader.GetString("table_name", "");
            Columns = new Dictionary<string, MetaDataColumn>();
            Rows = new Dictionary<string, object>();
        }
        public string TableName { get; set; }
        public Dictionary<string, MetaDataColumn> Columns { get; set; }
        public Dictionary<string, object> Rows { get; set; }
    }
}