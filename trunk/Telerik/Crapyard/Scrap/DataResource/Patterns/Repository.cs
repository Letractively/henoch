using System;
using System.Collections.Generic;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Linq;
namespace DataResource.Patterns
{
    /// <summary>
    /// For Data Access Layer
    /// </summary>
    public class Repository
    {
        // Specify the provider name, server and database.
        const string providerName = "System.Data.SqlClient";
        const string serverName = @"M17-WINDOWS7\SqlExpress";
        const string databaseName = "AMS_DM";
        const string user = "sa";
        const string pwd = "!";

        private EntityConnectionStringBuilder GetEntityBuilder()
        {
            SqlConnectionStringBuilder sqlBuilder =
                new SqlConnectionStringBuilder();

            // Set the properties for the data source.
            sqlBuilder.DataSource = serverName;
            sqlBuilder.InitialCatalog = databaseName;
            sqlBuilder.IntegratedSecurity = true;
            //sqlBuilder.UserID = user;
            //sqlBuilder.Password = pwd;

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
            entityBuilder.Metadata = @"res://*/MyEntities.csdl|
                            res://*/MyEntities.ssdl|
                            res://*/MyEntities.msl";
            return entityBuilder;
        }

        public void AddTeam(string name, string email, bool isExclusive, bool isActive)
        {

            // Initialize the connection string builder for the
            // underlying provider.
            EntityConnectionStringBuilder entityBuilder = GetEntityBuilder();

            using (EntityConnection conn =
                    new EntityConnection(entityBuilder.ToString()))
            {
                using (AMS_DMEntities entities = new AMS_DMEntities(conn))
                {
                    Console.WriteLine("connection Ok.");

                    Team team = new Team();
                    team.Name = name;
                    team.Email = email;
                    team.IsExclusive = isExclusive;
                    team.IsActive = isActive;

                    entities.AddToTeams(team);
                    entities.SaveChanges();
                }
            }
        }



        public Team GetTeam(string name)
        {
            Team team = null;
            EntityConnectionStringBuilder entityBuilder = GetEntityBuilder();

            using (EntityConnection conn =
                    new EntityConnection(entityBuilder.ToString()))
            {
                using (AMS_DMEntities entities = new AMS_DMEntities(conn))
                {
                    team = entities.Teams.Where(n => n.Name == name).FirstOrDefault();
                }
            }
            return team;
        }

        public IList<Team> GetTeams(string name)
        {
            IList<Team> teams = new List<Team>();
            EntityConnectionStringBuilder entityBuilder = GetEntityBuilder();

            using (EntityConnection conn =
                    new EntityConnection(entityBuilder.ToString()))
            {
                using (AMS_DMEntities entities = new AMS_DMEntities(conn))
                {
                    teams = entities.Teams.Where(n => n.Name == name).ToList();
                }
            }
            return teams;
        }

        public void DeleteTeams()
        {
            IList<Team> teams = new List<Team>();
            EntityConnectionStringBuilder entityBuilder = GetEntityBuilder();

            using (EntityConnection conn =
                    new EntityConnection(entityBuilder.ToString()))
            {
                using (AMS_DMEntities entities = new AMS_DMEntities(conn))
                {
                    teams = entities.Teams.ToList();
                    for (int i = 0; i < teams.Count; i++)
                    {
                        entities.Teams.DeleteObject(teams[i]);
                    }
                    entities.SaveChanges();
                }
            }
        }
    }
}
