using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using Beheer.BusinessObjects.Dictionary;
using OraAccessLayer.Data.Core;
using Oracle.DataAccess.Client;

namespace OraAccessLayer
{
    /// <summary>
    /// Voor simpele CRUD.
    /// </summary>
    public class OracleAccess
    {
        private string _oradb = "Data Source=odomeinen;User Id=so_dom;Password=supermx;";
        private OracleConnection m_Connection;

        public OracleConnection GetOraConnection()
        {
            using (OracleConnection conn = new OracleConnection(_oradb))
            {
                conn.Open();
                OracleCommand oracleCommand = new OracleCommand {Connection = conn};
                return oracleCommand.Connection;
            }
            
        }
        public DataSet SelectOracleSrvRows(DataSet dataset, string query)
        {
            using (var conn = new OracleConnection(_oradb))
            {
                var adapter = new OracleDataAdapter {SelectCommand = new OracleCommand(query, conn)};
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

            var ds = GetDaBusinessObject(qry,
            new BeheerContextEntity
            {
                DataKeyValue = stringValue,
                DataKeyName = kolomName,
                Tablename = tableName
            });
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
            using (var conn = new OracleConnection(_oradb))
            {
                conn.Open();                
                var cmd = new OracleCommand
                {
                    Connection = conn,
                    CommandText = qry,
                    CommandType = CommandType.Text
                };
                cmd.Connection = conn;
                var reader = cmd.ExecuteReader();

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
            using (var conn = new OracleConnection(_oradb))
            {
                DataSet myDs = new DataSet();
                conn.Open();
                var cmd = new OracleCommand
                {
                    Connection = conn,
                    CommandText = qry,
                    CommandType = CommandType.Text
                };
                cmd.Connection = conn;
                OracleDataAdapter dataAdapter = new OracleDataAdapter(cmd);
                dataAdapter.Fill(myDs);

                return myDs;
            }
        }
        public IList<BeheerContextEntity> GetBusinessObjects(string tableName, string kolomName)
        {
            var theTables = new List<BeheerContextEntity>();            
            using (OracleConnection conn = new OracleConnection(_oradb))
            {
                conn.Open();
                var cmd = new OracleCommand
                              {
                                  Connection = conn,
                                  CommandText = "select "+ kolomName + " from " + tableName + 
                                  " order by " + kolomName,
                                  CommandType = CommandType.Text
                              };
                cmd.Connection = conn;
                var reader = cmd.ExecuteReader();

                SmartDataReader smartReader = new SmartDataReader(reader);
                int i = 0;
                while (smartReader.Read())
                {

                    BeheerContextEntity aTable = new BeheerContextEntity
                    {
                        Id = i++,
                        DataKeyValue = smartReader.GetString(kolomName, ""),
                        Tablename = tableName,
                        DataKeyName = kolomName
                    };
                    theTables.Add(aTable);
                }

                return theTables;
            }
        }

        public bool SetOraConnection()
        {
            _oradb = "Data Source=odomeinen;User Id=so_dom;Password=supermx;";
            using (m_Connection = new OracleConnection(_oradb))
            {
                m_Connection.Open();
                new OracleCommand { Connection = m_Connection };
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
            int rowsAffected = 0;
            using (OracleConnection conn = new OracleConnection(_oradb))
            {
                string sqlUpdate = "update {0} set {1} ='{2}' where {1} = '{3}'";
                string tableName = oldBusinessObject.Tablename;
                string columnName = oldBusinessObject.DataKeyName;
                sqlUpdate = string.Format(sqlUpdate,
                    tableName,
                    columnName,
                    newBusinessObject.DataKeyValue,
                    oldBusinessObject.DataKeyValue);
                conn.Open();

                var trans = conn.BeginTransaction();
                try
                {
                    var cmd = new OracleCommand
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
        /// Na insert wordt het aantal toegevoegde rijen teruggegeven.
        /// </summary>
        /// <param name="newBusinessObject"></param>
        /// <returns></returns>
        public int Insert(BeheerContextEntity newBusinessObject)
        {
            int rowsAffected = 0;
            using (OracleConnection conn = new OracleConnection(_oradb))
            {
                conn.Open();
                var trans = conn.BeginTransaction();
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
                    var cmd = new OracleCommand
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
            int rowsAffected = 0;
            using (OracleConnection conn = new OracleConnection(_oradb))
            {
                conn.Open();
                var trans = conn.BeginTransaction();
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
                    var cmd = new OracleCommand
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
            int rowsaffected = 0;
            using (OracleConnection conn = new OracleConnection(_oradb))
            {
                conn.Open();
                var trans = conn.BeginTransaction();

                try
                {
                    string sqlUpdate =
                @"
                        DELETE FROM {0}
                        WHERE {1} = '{2}'
                    ";
                    sqlUpdate = string.Format(sqlUpdate, businessObject.Tablename,
                        businessObject.DataKeyName, businessObject.DataKeyValue);
                    var cmd = new OracleCommand
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
    }
}
