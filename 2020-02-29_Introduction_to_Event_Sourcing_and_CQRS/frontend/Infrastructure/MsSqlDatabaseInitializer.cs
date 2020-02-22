using System;
using System.Data.SqlClient;
using SqlStreamStore;

namespace Infrastructure
{
    public class MsSqlDatabaseInitializer
    {
        private readonly MsSqlStreamStoreV3Settings settings;
        private readonly MsSqlStreamStoreV3 store;
        public string DatabaseName { get; private set; }

        public MsSqlDatabaseInitializer(MsSqlStreamStoreV3Settings settings, MsSqlStreamStoreV3 store)
        {
            this.settings = settings;
            this.store = store;
        }

        public SqlConnection CreateConnection()
            => new SqlConnection(CreateConnectionStringBuilder().ConnectionString);

        public SqlConnectionStringBuilder CreateConnectionStringBuilder()
            => CreateMasterDatabaseConnectionString();

        public void Initialize()
        {
            CreateDatabaseIfNotExists();
            store.CreateSchemaIfNotExists();
        }

        private void CreateDatabaseIfNotExists()
        {
            using(var connection = CreateConnection())
            {
                connection.Open();

                var createCommand = $@"CREATE DATABASE [{DatabaseName}]
                                        ALTER DATABASE [{DatabaseName}] SET SINGLE_USER
                                        ALTER DATABASE [{DatabaseName}] SET COMPATIBILITY_LEVEL=140
                                        ALTER DATABASE [{DatabaseName}] SET MULTI_USER";

                using(var command = new SqlCommand(createCommand, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private SqlConnectionStringBuilder CreateMasterDatabaseConnectionString()
        {
            var builder = new SqlConnectionStringBuilder(settings.ConnectionString);

            DatabaseName = builder.InitialCatalog;

            builder.InitialCatalog = "master";

            return builder;
        }
    }
}