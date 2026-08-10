using System.Threading.Tasks;
using Testcontainers.MsSql;
using Xunit;

namespace MyNewProjectName.IntegrationTests.Fixtures;

public sealed class SqlServerDatabaseFixture : IAsyncLifetime
{
    private MsSqlContainer Container { get; set; } = null!;
    public string ConnectionString => Container.GetConnectionString();

    public async Task InitializeAsync()
    {
        Container = new MsSqlBuilder("mcr.microsoft.com/mssql/server:2022-latest")
            .WithPassword("Secret123!")
            .Build();

        await Container.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await Container.DisposeAsync();
    }
}