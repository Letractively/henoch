using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using Beheer.BusinessObjects.Dictionary;
using Dictionary.BusinessObjects;
using System.Configuration;

namespace Repository
{
    /// <summary>
    /// Voor simpele CRUD.
    /// </summary>
    public class MyAccess<TConnection> : IMyAccess<TConnection>
        where TConnection : DbConnection //ICloneable, new()
    {
        //TODO:vervang connection string
        private static TConnection m_Connection;
        private string _connstring = @"Provider=SQLOLEDB;Data Source=SYGNION-D9E6E77\SYGNION;Persist Security Info=True;Password=imagine;User ID=sa;Initial Catalog=sygniondb";
        private string _providerName;

        #region IMyAccess<TConnection> Members

        /// <summary>
        /// Given a provider name and connection string,
        /// create the DbProviderFactory and DbConnection.
        /// Returns a DbConnection on success; null on failure.
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public TConnection CreateDbConnection(string providerName, string connectionString)
        {
            m_Connection = CreateDatabaseConnection(providerName, connectionString);
            return m_Connection;
        }

        /// <summary>
        /// Given a provider name and connection string,
        /// create the DbProviderFactory and DbConnection.
        /// Returns a DbConnection on success; null on failure.
        /// </summary>
        /// <param name="providerName"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static TConnection CreateDatabaseConnection(string providerName, string connectionString)
        {
            // Assume failure.
            TConnection connection = null;

            // Create the DbProviderFactory and DbConnection.
            if (connectionString != null)
            {
                try
                {
                    DbProviderFactory factory =
                    DbProviderFactories.GetFactory(providerName);

                    connection = (TConnection) factory.CreateConnection();
                    connection.ConnectionString = connectionString;
                }
                catch (Exception ex)
                {
                    // Set the connection to null if it was created.
                    if (connection != null)
                    {
                        connection = null;
                    }
                    Console.WriteLine(ex.Message);
                }
            }
            // Return the connection.
            return connection;
        }

        /// <summary>
        /// Retrieve a connection string by specifying the providerName.
        /// Assumes one connection string per provider in the config file.
        /// </summary>
        /// <param name="providerName"></param>
        /// <returns></returns>
        public string GetConnectionStringByProvider(string providerName)
        {
            return GetConnStringByProvider(providerName);
        }

        /// <summary>
        /// Retrieve a connection string by specifying the providerName.
        /// Assumes one connection string per provider in the config file.
        /// </summary>
        /// <param name="providerName"></param>
        /// <returns></returns>
        public static string GetConnStringByProvider(string providerName)
        {
            // Return null on failure.
            string returnValue = null;

            // Get the collection of connection strings.
            ConnectionStringSettingsCollection settings =
            ConfigurationManager.ConnectionStrings;

            // Walk through the collection and return the first
            // connection string matching the providerName.
            if (settings != null)
            {
                foreach (ConnectionStringSettings cs in settings)
                {
                    if (cs.ProviderName == providerName)
                        returnValue = cs.ConnectionString;
                    break;
                }
            }
            return returnValue;
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
            using (var conn = m_Connection)
            {
                conn.ConnectionString = _connstring;
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.Connection = conn;
                cmd.CommandText = qry;
                cmd.CommandType = CommandType.Text;

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

        public DataSet GetBusinessObjectDataset(string qry, BeheerContextEntity beheerObject)
        {
            using (var conn = m_Connection)
            {
                // Create the DbProviderFactory and DbConnection.
                DbProviderFactory factory =
                DbProviderFactories.GetFactory(_providerName);

                conn.ConnectionString = _connstring;
                var myDs = new DataSet();
                conn.Open();
                var cmd = CreateCommand(qry, conn);

                DbDataAdapter dataAdapter = factory.CreateDataAdapter();
                dataAdapter.SelectCommand = cmd ;
                dataAdapter.Fill(myDs);

                return myDs;
            }
        }

        private static DbCommand CreateCommand(string qry, TConnection conn)
        {
            var cmd = conn.CreateCommand();
            cmd.Connection = conn;
            cmd.CommandText = qry;
            cmd.CommandType = CommandType.Text;
            return cmd;
        }

        public IList<BeheerContextEntity> GetBusinessObjects(string tableName, string kolomName)
        {
            var beheerContextEntities = new List<BeheerContextEntity>();
            using (var conn = m_Connection)
            {
                conn.ConnectionString = _connstring;
                conn.Open();
                var cmd = conn.CreateCommand();
                cmd.Connection = conn;
                cmd.CommandText = "select " + kolomName + " from " + tableName +
                                  " order by " + kolomName;
                cmd.CommandType = CommandType.Text;
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


        /// <summary>
        /// Na Update wordt het aantal bijgewerkte rijen teruggegeven.
        /// </summary>
        /// <param name="oldBusinessObject"></param>
        /// <param name="newBusinessObject"></param>
        /// <returns></returns>
        public int Update(BeheerContextEntity oldBusinessObject, BeheerContextEntity newBusinessObject)
        {
            int rowsAffected;
            using (var conn = m_Connection)
            {
                conn.ConnectionString = _connstring;
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
                        var cmd = CreateCommand(sqlUpdate, conn);
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
            using (var conn = m_Connection)
            {
                conn.ConnectionString = _connstring;
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
                    var cmd = CreateCommand(sqlUpdate, conn);
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
            using (var conn = m_Connection)
            {
                conn.ConnectionString = _connstring;
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
                    var cmd = CreateCommand(sqlUpdate, conn);
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
            using (var conn = m_Connection)
            {
                conn.ConnectionString = _connstring;
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
                    var cmd = CreateCommand(sqlUpdate, conn);
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