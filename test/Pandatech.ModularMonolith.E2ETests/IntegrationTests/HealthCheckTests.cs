using Pandatech.ModularMonolith.E2ETests.Configurations;

namespace Pandatech.ModularMonolith.E2ETests.IntegrationTests;

[Collection("Shared Postgres")]
public class HealthCheckTests(ApiFactory factory) : IAsyncLifetime
{
    private readonly HttpClient _client = factory.HttpClient;
    private readonly Func<Task> _resetState = factory.ResetStateAsync;

    public ValueTask InitializeAsync()
    {
        return ValueTask.CompletedTask;
    }

    public async ValueTask DisposeAsync()
    {
        await _resetState();
    }

    [Fact]
    public async Task HealthCheck_Should_Return_Successful_Response()
    {
        var response = await _client.GetAsync("/above-board/health");

        Assert.True(response.IsSuccessStatusCode);
    }
}
