using FinHub.Mock2.Entities;
using MassTransit.PostgresOutbox.Abstractions;
using MassTransit.PostgresOutbox.Entities;
using MassTransit.PostgresOutbox.Extensions;
using Microsoft.EntityFrameworkCore;

namespace FinHub.Mock2.Context;

//dotnet ef migrations add --project src\Pandatech.ModularMonolith.Mock2\Pandatech.ModularMonolith.Mock2.csproj --context Pandatech.ModularMonolith.Mock2.Context.PostgresContext --configuration Debug --output-dir ./Context/Migrations
public class PostgresContext(DbContextOptions<PostgresContext> options) : DbContext(options)
   , IOutboxDbContext, IInboxDbContext
{
   public DbSet<TransactionEntity> Transactions { get; set; } = null!;
   public DbSet<InboxMessage> InboxMessages { get; set; }
   public DbSet<OutboxMessage> OutboxMessages { get; set; }

   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      base.OnModelCreating(modelBuilder);

      modelBuilder.ApplyConfigurationsFromAssembly(typeof(Mock2Extension).Assembly);

      modelBuilder.ConfigureInboxOutboxEntities();
   }
}
