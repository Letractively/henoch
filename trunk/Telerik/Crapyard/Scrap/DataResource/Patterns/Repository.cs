using System.Data.EntityClient;
using System.Data.SqlClient;

namespace DataResource.Patterns
{
    /// <summary>
    /// For Data Access Layer
    /// </summary>
    public class Repository
    {
        // Specify the provider name, server and database.
        const string providerName = "System.Data.SqlClient";
        const string serverName = @"VIRTUALXP-61514\MYENTERPRISE";
        const string databaseName = "AMS_DM";
        const string user = "sa";
        const string pwd = "sa123!";

        private EntityConnectionStringBuilder GetEntityBuilder()
        {
            SqlConnectionStringBuilder sqlBuilder =
                new SqlConnectionStringBuilder();

            // Set the properties for the data source.
            sqlBuilder.DataSource = serverName;
            sqlBuilder.InitialCatalog = databaseName;
            sqlBuilder.IntegratedSecurity = true;
            sqlBuilder.UserID = user;
            sqlBuilder.Password = pwd;

            // Build the SqlConnection connection string.
            string providerString = sqlBuilder.ToString();

            // Initialize the EntityConnectionStringBuilder.
            EntityConnectionStringBuilder entityBuilder =
                new EntityConnectionStringBuilder();

            //Set the provider name.
            entityBuilder.Provider = providerName;

            // Set the provider-specific connection string.
            entityBuilder.ProviderConnectionString = providerString;

            // Set the Metadata location.
            entityBuilder.Metadata = @"res://*/AMSTeamManagement.csdl|
                            res://*/AMSTeamManagement.ssdl|
                            res://*/AMSTeamManagement.msl";
            return entityBuilder;
        }
    }
}
