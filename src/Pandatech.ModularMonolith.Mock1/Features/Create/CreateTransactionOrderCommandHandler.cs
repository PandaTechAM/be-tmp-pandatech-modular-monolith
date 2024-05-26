using MassTransit;
using MassTransit.PostgresOutbox.Extensions;
using Pandatech.ModularMonolith.Mock1.Context;
using Pandatech.ModularMonolith.Mock1.Entities;
using Pandatech.ModularMonolith.Mock1.Enums;
using Pandatech.ModularMonolith.Mock1.Integration;
using Pandatech.ModularMonolith.SharedKernel.Interfaces;

namespace Pandatech.ModularMonolith.Mock1.Features.Create;

public class CreateTransactionOrderCommandHandler(PostgresContext postgresContext, IPublishEndpoint publishEndpoint)
   : ICommandHandler<CreateTransactionOrderCommand>
{
   public async Task Handle(CreateTransactionOrderCommand request, CancellationToken cancellationToken)
   {
      var transaction = new TransactionOrderEntity
      {
         UserId = 1, Status = Status.Enqueued, Amount = 239.43m, Narrative = "null"
      };

      postgresContext.TransactionOrders.Add(transaction);

      var transactionOrderEvent = new TransactionOrderCreatedEvent(transaction.Id, transaction.UserId,
         transaction.Amount, transaction.Narrative);

      postgresContext.AddToOutbox(transactionOrderEvent);

      await postgresContext.SaveChangesAsync(cancellationToken);
   }
}
