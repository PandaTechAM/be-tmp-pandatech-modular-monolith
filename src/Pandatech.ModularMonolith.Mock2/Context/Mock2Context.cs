using MassTransit.PostgresOutbox.Abstractions;
using MassTransit.PostgresOutbox.Entities;
using MassTransit.PostgresOutbox.Extensions;
using Microsoft.EntityFrameworkCore;
using Pandatech.ModularMonolith.Mock2.Entities;

namespace Pandatech.ModularMonolith.Mock2.Context;

public class Mock2Context(DbContextOptions<Mock2Context> options) : DbContext(options)
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