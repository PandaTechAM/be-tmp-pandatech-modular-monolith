using MassTransit.PostgresOutbox.Abstractions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Pandatech.ModularMonolith.Mock1.Integration;
using Pandatech.ModularMonolith.Mock2.Context;

namespace Pandatech.ModularMonolith.Mock2.Features;

public class TransactionOrderConsumer(ISender sender, IServiceScopeFactory serviceScopeFactory)
   : InboxConsumer<TransactionOrderCreatedEvent, PostgresContext>(serviceScopeFactory)
{
   protected override async Task Consume(TransactionOrderCreatedEvent message)
   {
      var command = new CreateTransactionV1Command
      {
         TransactionOrderId = message.Id,
         UserId = message.UserId,
         Amount = message.Amount,
         Narrative = message.Narrative
      };

      await sender.Send(command);
   }
}