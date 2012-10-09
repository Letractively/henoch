using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Dictionary.BusinessObjects;

namespace Repository
{
    public interface IMyAccess<TConnection>
     where TConnection : DbConnection  //*ICloneable*/ new()
    {
        TConnection CreateDbConnection(string providerName, string connectionString);
        string GetConnectionStringByProvider(string providerName);

        int Delete(BeheerContextEntity businessObject);
        IBeheerContextEntity GetBusinessObject(string tableName, string kolomName, string stringValue);
        IBeheerContextEntity GetBusinessObject(string qry, BeheerContextEntity beheerObject);
        IBeheerContextEntity GetBusinessObject(string tableName, string kolomName, ValueType value);
        IList<BeheerContextEntity> GetBusinessObjects(string tableName, string kolomName);
        DataSet GetBusinessObjectDataset(string qry, BeheerContextEntity beheerObject);
        int Insert(BeheerContextEntity newBusinessObject);
        int Insert(ParentKeyEntity masterForeignKey, BeheerContextEntity newBusinessObject);
       
        int Update(BeheerContextEntity oldBusinessObject, BeheerContextEntity newBusinessObject);
    }
}
