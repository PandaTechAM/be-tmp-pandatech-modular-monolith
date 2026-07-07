using System.Data.Common;
using Microsoft.AspNetCore.Mvc.Testing;
using Npgsql;
using Pandatech.ModularMonolith.ApiGateway;
using Respawn;
using Testcontainers.PostgreSql;
using Testcontainers.RabbitMq;
using Testcontainers.Redis;

namespace Pandatech.ModularMonolith.E2ETests.Configurations;

public class ApiFactory : WebApplicationFactory<AssemblyReference>, IAsyncLifetime
{
    // Images match docker-compose.yml so tests run against the same infra versions as the app.
    private readonly PostgreSqlContainer _postgresContainer = new PostgreSqlBuilder("postgres:latest")
        .Build();

    private readonly RabbitMqContainer _rabbitMqContainer = new RabbitMqBuilder("rabbitmq:4-management-alpine")
        .Build();

    private readonly RedisContainer _redisContainer = new RedisBuilder("redis:latest")
        .Build();

    private DbConnection _dbConnection = default!;
    private Respawner _respawner = default!;

    public HttpClient HttpClient { get; private set; } = default!;

    public async ValueTask InitializeAsync()
    {
        await Task.WhenAll(
            _postgresContainer.StartAsync(),
            _rabbitMqContainer.StartAsync(),
            _redisContainer.StartAsync());

        SetEnvironments();

        // Building the client runs the real app pipeline (migrations + seeding).
        HttpClient = CreateClient();

        await InitializeRespawner();
    }

    public override async ValueTask DisposeAsync()
    {
        await Task.WhenAll(
            _postgresContainer.DisposeAsync()
                .AsTask(),
            _rabbitMqContainer.DisposeAsync()
                .AsTask(),
            _redisContainer.DisposeAsync()
                .AsTask());

        await base.DisposeAsync();
    }

    public async Task ResetStateAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }

    private async Task InitializeRespawner()
    {
        // Test data written via the modules' endpoints lands in the Mock1 database, so reset that one.
        _dbConnection = new NpgsqlConnection(DatabaseFor(_postgresContainer.GetConnectionString(), "mock1"));

        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection,
            new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                SchemasToInclude = ["public"]
            });
    }

    // Local environment: SharedKernel skips PandaVault, so appsettings.Local.json supplies the AES
    // key + faked SMS/email; the harness only overrides the infra connection strings with the
    // Testcontainers endpoints. Each module owns its own database on the shared Postgres server
    // (the modules define colliding outbox/inbox tables, so they cannot share one database); EF
    // Core's Migrate() creates each database on first boot.
    private void SetEnvironments()
    {
        var postgres = _postgresContainer.GetConnectionString();

        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Local");
        Environment.SetEnvironmentVariable("ConnectionStrings:Postgres", postgres);
        Environment.SetEnvironmentVariable("ConnectionStrings:Postgres.Mock1", DatabaseFor(postgres, "mock1"));
        Environment.SetEnvironmentVariable("ConnectionStrings:Postgres.Mock2", DatabaseFor(postgres, "mock2"));
        Environment.SetEnvironmentVariable("ConnectionStrings:Postgres.Scheduler", DatabaseFor(postgres, "scheduler"));
        Environment.SetEnvironmentVariable("ConnectionStrings:Redis", _redisContainer.GetConnectionString());
        Environment.SetEnvironmentVariable("ConnectionStrings:RabbitMq", _rabbitMqContainer.GetConnectionString());
    }

    private static string DatabaseFor(string connectionString, string database)
    {
        return new NpgsqlConnectionStringBuilder(connectionString)
        {
            Database = database
        }.ConnectionString;
    }
}
