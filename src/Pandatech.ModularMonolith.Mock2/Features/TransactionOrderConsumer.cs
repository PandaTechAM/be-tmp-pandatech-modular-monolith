using FinHub.Mock1.Integration;
using FinHub.Mock2.Context;
using MassTransit.PostgresOutbox.Abstractions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace FinHub.Mock2.Features;

public class TransactionOrderConsumer(ISender sender, IServiceScopeFactory serviceScopeFactory)
   : InboxConsumer<TransactionOrderCreatedEvent, PostgresContext>(serviceScopeFactory)
{
   public override async Task Consume(TransactionOrderCreatedEvent message)
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
