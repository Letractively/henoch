using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DataResource.Metadata;
using Rhino.Mocks;

namespace DataResource
{
    public class MyAccess
    {

        /// <summary>
        /// Deze is om BO te vullen
        /// </summary>
        /// <returns></returns>
        public IMetaDataSchema[] GetDummyDataSet()
        {
            MockRepository mocks = new MockRepository();            
            List<IMetaDataSchema> list = new List<IMetaDataSchema>();

            //Make 10 tables/Business Objects.
            for (int i = 0; i < 1000; i++)
            {
                IMetaDataSchema table = mocks.Stub<IMetaDataSchema>();
                table.TableName = "table" + i;                
                list.Add(table);
            }

            return list.ToArray<IMetaDataSchema>();
        }

        /// <summary>
        /// To be run in a 32-bit application pool.
        /// </summary>
        public int DoOleDbAction(string connectionString)
        {
            System.Data.OleDb.OleDbConnection DBConnection;
            System.Data.OleDb.OleDbDataAdapter DataAdaptor;
            DataSet DataSet = new DataSet();
            int result = 0;
            using (DBConnection = new System.Data.OleDb.OleDbConnection(connectionString))
            {
                DataAdaptor = new System.Data.OleDb.OleDbDataAdapter("Select * from Orders", DBConnection);
                DBConnection.Open();
                DataAdaptor.Fill(DataSet);
                result = DataSet.Tables[0].Rows.Count;
                DBConnection.Close();
            }

            return result;
        }
    }

}
