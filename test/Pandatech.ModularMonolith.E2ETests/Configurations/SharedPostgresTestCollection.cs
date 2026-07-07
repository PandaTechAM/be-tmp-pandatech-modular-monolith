namespace Pandatech.ModularMonolith.E2ETests.Configurations;

[CollectionDefinition("Shared Postgres")]
public class SharedPostgresTestCollection : ICollectionFixture<ApiFactory>;
