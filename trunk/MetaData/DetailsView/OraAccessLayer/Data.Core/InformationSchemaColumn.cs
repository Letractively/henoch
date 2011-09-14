namespace OraAccessLayer.Data.Core
{
    public class MetaDataColumn
    {

        public MetaDataColumn(ISmartDataReader reader)
        {
            //table_name, column_name, is_nullable, data_type, character_maximum_length , udt_name
            TableName = reader.GetString("table_name", "");
            ColumnName = reader.GetString("column_name", "");
            IsNullable = reader.GetString("is_nullable", "");
            DataType = reader.GetString("data_type", "");
            MaxCharLen = reader.GetNullableInt32("character_maximum_length");
            UdtName = reader.GetString("udt_name", "");
        }

        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string IsNullable { get; set; }
        /// <summary>
        /// This is the datatype like integer & character varying
        /// </summary>
        public string DataType { get; set; }
        public int? MaxCharLen { get; set; }
        /// <summary>
        /// This is the variable type like varchar & int4
        /// </summary>
        public string UdtName { get; set; }
    }
}