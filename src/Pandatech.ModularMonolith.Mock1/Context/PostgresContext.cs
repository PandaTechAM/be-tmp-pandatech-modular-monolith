using MassTransit.PostgresOutbox.Abstractions;
using MassTransit.PostgresOutbox.Entities;
using MassTransit.PostgresOutbox.Extensions;
using Microsoft.EntityFrameworkCore;
using Pandatech.ModularMonolith.Mock1.Entities;

namespace Pandatech.ModularMonolith.Mock1.Context;

//dotnet ef migrations add --project src\Pandatech.ModularMonolith.Mock1\Pandatech.ModularMonolith.Mock1.csproj --context Pandatech.ModularMonolith.Mock1.Context.PostgresContext --configuration Debug --output-dir ./Context/Migrations
public class PostgresContext(DbContextOptions<PostgresContext> options) : DbContext(options)
   , IOutboxDbContext, IInboxDbContext
{
   public DbSet<TransactionOrderEntity> TransactionOrders { get; set; } = null!;
   public DbSet<InboxMessage> InboxMessages { get; set; }
   public DbSet<OutboxMessage> OutboxMessages { get; set; }

   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      base.OnModelCreating(modelBuilder);

      modelBuilder.ApplyConfigurationsFromAssembly(typeof(Mock1Extension).Assembly);
      modelBuilder.ConfigureInboxOutboxEntities();
   }
}