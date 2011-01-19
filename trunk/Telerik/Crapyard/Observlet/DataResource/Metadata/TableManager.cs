using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DataResource.Metadata
{
    class TableManager
    {
        private string m_connstring;

        #region Table Manager

        public IMetaDataSchema[] GetTablesInformationSchema()
        {
            var theTables = new List<IMetaDataSchema>();

            SqlConnection conn = null;
            SqlCommand cmd = null;
            try
            {
                conn = new SqlConnection(m_connstring);
                conn.Open();

                string sql = @"SELECT table_name
                                FROM INFORMATION_SCHEMA.tables
                                where table_schema = 'public'
                                and table_type = 'BASE TABLE' Order BY table_name";

                cmd = new SqlCommand(sql, conn);
                cmd.CommandType = CommandType.Text;

                SqlDataReader reader = cmd.ExecuteReader();

                SmartDataReader smartReader = new SmartDataReader(reader);
                while (smartReader.Read())
                {
                    IMetaDataSchema aTable = new MetaDataSchema(smartReader);
                    theTables.Add(aTable);
                }
                return theTables.ToArray();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();

                if (conn != null)
                    conn.Dispose();
            }
        }
        /// <summary>
        /// Get the columns of table [tableName].
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public IMetaDataSchema GetColumnsInformationSchema(IMetaDataSchema aTable)
        {

            SqlConnection conn = null;
            SqlCommand cmd = null;
            try
            {
                conn = new SqlConnection(m_connstring);
                conn.Open();

                string sql = @"SELECT 
                                    table_name, 
                                    column_name, 
                                    is_nullable, 
                                    data_type, 
                                    character_maximum_length , 
                                    udt_name
                                FROM INFORMATION_SCHEMA.Columns
                                WHERE table_name = @tableName";

                cmd = new SqlCommand(sql, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@tableName", aTable.TableName);

                SqlDataReader reader = cmd.ExecuteReader();

                ISmartDataReader smartReader = new SmartDataReader(reader);
                while (smartReader.Read())
                {
                    MetaDataColumn column = new MetaDataColumn(smartReader);
                    aTable.Columns.Add(column.ColumnName, column);
                }
                return aTable;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();

                if (conn != null)
                    conn.Dispose();
            }
        }

        public static DataSet GetTableView(string tableName, string connString)
        {

            DataSet myDS = new DataSet();
            SqlConnection conn = null;
            SqlCommand cmd = null;
            try
            {
                if (!string.IsNullOrEmpty(tableName))
                {
                    conn = new SqlConnection(connString);
                    conn.Open();

                    string sql = String.Format(@"
                                SELECT 
                                    *
                                FROM {0}
                                ", tableName);

                    cmd = new SqlCommand(sql, conn);
                    cmd.CommandType = CommandType.Text;

                    SqlDataReader reader = cmd.ExecuteReader();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(myDS);
                }
                else
                    myDS.Tables.Add();

                return myDS;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();

                if (conn != null)
                    conn.Dispose();
            }
        }

        public static DataSet GetTablesInformationSchema(string connString)
        {
            DataSet myDS = new DataSet();
            SqlConnection conn = null;
            SqlCommand cmd = null;
            try
            {

                conn = new SqlConnection(connString);
                conn.Open();

                string sql = @"SELECT table_name
                                FROM INFORMATION_SCHEMA.tables
                                where table_schema = 'public'
                                and table_type = 'BASE TABLE' Order BY table_name";

                cmd = new SqlCommand(sql, conn);
                cmd.CommandType = CommandType.Text;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(myDS);

                return myDS;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();

                if (conn != null)
                    conn.Dispose();
            }

        }
        #endregion
    }
}
