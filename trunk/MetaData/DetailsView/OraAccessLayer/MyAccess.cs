using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using Beheer.BusinessObjects.Dictionary;
using OraAccessLayer.Data.Core;

namespace OraAccessLayer
{
    /// <summary>
    /// Voor simpele CRUD.
    /// </summary>
    public class MyAccess<TConnection, TCommand, TAdapter> : IMyAccess<TConnection>
        where TConnection : DbConnection, ICloneable, new()
        where TCommand : DbCommand, ICloneable, new()
        where TAdapter : DbDataAdapter, IDataAdapter, IDbDataAdapter, new()
    {
        //TODO:vervang connection string
        private TConnection m_Connection;
        private string m_Oradb = "Data Source=odomeinen;User Id=so_dom;Password=supermx;";

        #region IMyAccess<TConnection> Members

        public DbConnection GetOraConnection()
        {
            using (var conn = new TConnection())
            {
                conn.ConnectionString = m_Oradb;
                conn.Open();
                var command = new TCommand {Connection = conn};
                return command.Connection;
            }
        }

        public DataSet SelectOracleSrvRows(DataSet dataset, string query)
        {
            using (var conn = new TConnection())
            {
                conn.ConnectionString = m_Oradb;
                var adapter = new TAdapter();
                var command = new TCommand {CommandText = query, Connection = conn};
                adapter.SelectCommand = command;
                adapter.Fill(dataset);
            }
            return dataset;
        }

        public IBeheerContextEntity GetBusinessObject(string tableName, string kolomName, ValueType value)
        {
            string qry = @"select id, {0} from {1} where {0} = {2}";
            qry = string.Format(CultureInfo.InvariantCulture, qry, kolomName, tableName, value);


            return GetBusinessObject(qry,
                                     new BeheerContextEntity
                                         {
                                             DataKeyValue = value.ToString(),
                                             DataKeyName = kolomName,
                                             Tablename = tableName
                                         });
        }

        public IBeheerContextEntity GetBusinessObject(string tableName, string kolomName, string stringValue)
        {
            string qry = @"select id, {0} from {1} where {0} = '{2}'";
            qry = string.Format(CultureInfo.InvariantCulture, qry, kolomName, tableName, stringValue);

            return GetBusinessObject(qry,
                                     new BeheerContextEntity
                                         {
                                             DataKeyValue = stringValue,
                                             DataKeyName = kolomName,
                                             Tablename = tableName
                                         });
        }

        public IBeheerContextEntity GetBusinessObject(string qry, BeheerContextEntity beheerObject)
        {
            using (var conn = new TConnection())
            {
                conn.ConnectionString = m_Oradb;
                conn.Open();
                var cmd = new TCommand
                              {
                                  Connection = conn,
                                  CommandText = qry,
                                  CommandType = CommandType.Text
                              };
                cmd.Connection = conn;
                DbDataReader reader = cmd.ExecuteReader();

                var smartReader = new SmartDataReader(reader);

                BeheerContextEntity businessObject = null;
                while (smartReader.Read())
                {
                    businessObject = new BeheerContextEntity
                                         {
                                             Id = smartReader.GetInt32("id"),
                                             DataKeyValue = smartReader.GetString(beheerObject.DataKeyName, ""),
                                             Tablename = beheerObject.Tablename,
                                             DataKeyName = beheerObject.DataKeyName
                                         };
                }
                return businessObject;
            }
        }

        public DataSet GetDaBusinessObject(string qry, BeheerContextEntity beheerObject)
        {
            using (var conn = new TConnection())
            {
                conn.ConnectionString = m_Oradb;
                var myDs = new DataSet();
                conn.Open();
                var cmd = new TCommand
                              {
                                  Connection = conn,
                                  CommandText = qry,
                                  CommandType = CommandType.Text
                              };
                cmd.Connection = conn;
                var dataAdapter = new TAdapter {SelectCommand = cmd};
                dataAdapter.Fill(myDs);

                return myDs;
            }
        }

        public IList<BeheerContextEntity> GetBusinessObjects(string tableName, string kolomName)
        {
            var beheerContextEntities = new List<BeheerContextEntity>();
            using (var conn = new TConnection())
            {
                conn.ConnectionString = m_Oradb;
                conn.Open();
                var cmd = new TCommand
                              {
                                  Connection = conn,
                                  CommandText = "select " + kolomName + " from " + tableName +
                                                " order by " + kolomName,
                                  CommandType = CommandType.Text
                              };
                cmd.Connection = conn;
                DbDataReader reader = cmd.ExecuteReader();

                var smartReader = new SmartDataReader(reader);
                int i = 0;
                while (smartReader.Read())
                {
                    var aTable = new BeheerContextEntity
                                     {
                                         Id = i++,
                                         DataKeyValue = smartReader.GetString(kolomName, ""),
                                         Tablename = tableName,
                                         DataKeyName = kolomName
                                     };
                    beheerContextEntities.Add(aTable);
                }

                return beheerContextEntities;
            }
        }

        public bool SetOraConnection()
        {
            m_Oradb = "Data Source=odomeinen;User Id=so_dom;Password=supermx;";
            using (m_Connection = new TConnection())
            {
                m_Connection.ConnectionString = m_Oradb;
                m_Connection.Open();
                new TCommand {Connection = m_Connection};
            }
            return true;
        }

        /// <summary>
        /// Na Update wordt het aantal bijgewerkte rijen teruggegeven.
        /// </summary>
        /// <param name="oldBusinessObject"></param>
        /// <param name="newBusinessObject"></param>
        /// <returns></returns>
        public int Update(BeheerContextEntity oldBusinessObject, BeheerContextEntity newBusinessObject)
        {
            int rowsAffected;
            using (var conn = new TConnection())
            {
                conn.ConnectionString = m_Oradb;
                string sqlUpdate = "update {0} set {1} ='{2}' where {1} = '{3}'";
                string tableName = oldBusinessObject.Tablename;
                string columnName = oldBusinessObject.DataKeyName;
                sqlUpdate = string.Format(sqlUpdate,
                                          tableName,
                                          columnName,
                                          newBusinessObject.DataKeyValue,
                                          oldBusinessObject.DataKeyValue);
                conn.Open();

                using (DbTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        var cmd = new TCommand
                                      {
                                          Connection = conn,
                                          CommandText = sqlUpdate,
                                          CommandType = CommandType.Text
                                      };
                        cmd.Connection = conn;

                        rowsAffected = cmd.ExecuteNonQuery();
                        trans.Commit();
                    }
                    catch (Exception)
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
            return rowsAffected;
        }

        /// <summary>
        /// Na insert wordt het aantal toegevoegde rijen teruggegeven.
        /// </summary>
        /// <param name="newBusinessObject"></param>
        /// <returns></returns>
        public int Insert(BeheerContextEntity newBusinessObject)
        {
            int rowsAffected;
            using (var conn = new TConnection())
            {
                conn.ConnectionString = m_Oradb;
                conn.Open();
                DbTransaction trans = conn.BeginTransaction();
                try
                {
                    string sqlUpdate =
                        @"INSERT INTO {0}
                        (id, {1})
                        VALUES
                        (s_thema.nextval, '{2}')
                    ";
                    sqlUpdate = string.Format(sqlUpdate, newBusinessObject.Tablename,
                                              newBusinessObject.DataKeyName, newBusinessObject.DataKeyValue);
                    var cmd = new TCommand
                                  {
                                      Connection = conn,
                                      CommandText = sqlUpdate,
                                      CommandType = CommandType.Text
                                  };
                    cmd.Connection = conn;

                    rowsAffected = cmd.ExecuteNonQuery();
                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }
            }
            return rowsAffected;
        }

        public int Insert(ParentKeyEntity masterForeignKey, BeheerContextEntity newBusinessObject)
        {
            int rowsAffected;
            using (var conn = new TConnection())
            {
                conn.ConnectionString = m_Oradb;
                conn.Open();
                DbTransaction trans = conn.BeginTransaction();
                try
                {
                    string sqlUpdate =
                        @"INSERT INTO {0}
                        ({1}, {3})
                        VALUES
                        ('{2}','{4}')
                    ";
                    sqlUpdate = string.Format(sqlUpdate,
                                              newBusinessObject.Tablename,
                                              newBusinessObject.DataKeyName,
                                              newBusinessObject.DataKeyValue,
                                              masterForeignKey.DataKeyName,
                                              masterForeignKey.DataKeyValue);
                    var cmd = new TCommand
                                  {
                                      Connection = conn,
                                      CommandText = sqlUpdate,
                                      CommandType = CommandType.Text
                                  };
                    cmd.Connection = conn;

                    rowsAffected = cmd.ExecuteNonQuery();
                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }
            }
            return rowsAffected;
        }

        /// <summary>
        /// Na Delete wordt het aantal verwijderde rijen teruggegeven.
        /// </summary>
        /// <param name="businessObject"></param>
        /// <returns></returns>
        public int Delete(BeheerContextEntity businessObject)
        {
            int rowsaffected;
            using (var conn = new TConnection())
            {
                conn.ConnectionString = m_Oradb;
                conn.Open();
                DbTransaction trans = conn.BeginTransaction();

                try
                {
                    string sqlUpdate =
                        @"
                        DELETE FROM {0}
                        WHERE {1} = '{2}'
                    ";
                    sqlUpdate = string.Format(sqlUpdate, businessObject.Tablename,
                                              businessObject.DataKeyName, businessObject.DataKeyValue);
                    var cmd = new TCommand
                                  {
                                      Connection = conn,
                                      CommandText = sqlUpdate,
                                      CommandType = CommandType.Text
                                  };
                    cmd.Connection = conn;

                    rowsaffected = cmd.ExecuteNonQuery();
                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                    throw;
                }
            }
            return rowsaffected;
        }

        #endregion
    }
}