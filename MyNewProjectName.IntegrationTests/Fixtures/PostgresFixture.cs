using System.Threading.Tasks;
using Testcontainers.PostgreSql;
using Xunit;

namespace MyNewProjectName.IntegrationTests.Fixtures;

public sealed class PostgresDatabaseFixture : IAsyncLifetime
{
    private PostgreSqlContainer Container { get; set; } = null!;
    public string ConnectionString => Container.GetConnectionString();

    public async Task InitializeAsync()
    {
        Container = new PostgreSqlBuilder("postgres:16-alpine")
            .WithDatabase("psgDB")
            .WithUsername("psg")
            .WithPassword("psg")
            .Build();

        await Container.StartAsync();
    }
    
    public async Task DisposeAsync()
    {
        await Container.DisposeAsync();
    }
}