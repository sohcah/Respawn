using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using NPoco;
using Testcontainers.MsSql;
using Xunit;

namespace Respawn.DatabaseTests;

// Testcontainers.Xunit references the .NET xUnit execution assembly, which cannot
// be loaded by the .NET Framework runner. Use the container directly here.
public class SqlServerFixture : IAsyncLifetime
{
    private readonly MsSqlContainer _container = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-CU23-ubuntu-22.04").Build();

    public Task InitializeAsync() => _container.StartAsync();

    public Task DisposeAsync() => _container.DisposeAsync().AsTask();

    public async Task<IDatabase> CreateDatabaseAsync([CallerMemberName] string dbName = "")
    {
        using var connection = new SqlConnection(_container.GetConnectionString());
        await connection.OpenAsync();
        using var command = connection.CreateCommand();
        command.CommandText = $"CREATE DATABASE [{dbName}]";
        await command.ExecuteNonQueryAsync();

        var connectionString = new SqlConnectionStringBuilder(_container.GetConnectionString())
        {
            InitialCatalog = dbName
        }.ConnectionString;
        var database = new Database(connectionString, DatabaseType.SqlServer2012, SqlClientFactory.Instance);
        return database.OpenSharedConnection();
    }
}
