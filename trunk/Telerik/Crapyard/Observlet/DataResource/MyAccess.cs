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
    }

}
