using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Beheer.BusinessObjects.Dictionary;

namespace OraAccessLayer
{
    public interface IMyAccess<TConnection>
     where TConnection : DbConnection, ICloneable, new()
    {
        int Delete(BeheerContextEntity businessObject);
        IBeheerContextEntity GetBusinessObject(string tableName, string kolomName, string stringValue);
        IBeheerContextEntity GetBusinessObject(string qry, BeheerContextEntity beheerObject);
        IBeheerContextEntity GetBusinessObject(string tableName, string kolomName, ValueType value);
        IList<BeheerContextEntity> GetBusinessObjects(string tableName, string kolomName);
        DataSet GetDaBusinessObject(string qry, BeheerContextEntity beheerObject);
        DbConnection GetOraConnection();
        int Insert(BeheerContextEntity newBusinessObject);
        int Insert(ParentKeyEntity masterForeignKey, BeheerContextEntity newBusinessObject);
        DataSet SelectOracleSrvRows(DataSet dataset, string query);
        bool SetOraConnection();
        int Update(BeheerContextEntity oldBusinessObject, BeheerContextEntity newBusinessObject);
    }
}
