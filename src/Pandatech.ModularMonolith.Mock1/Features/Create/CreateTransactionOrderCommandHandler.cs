using FinHub.Mock1.Context;
using FinHub.Mock1.Entities;
using FinHub.Mock1.Enums;
using FinHub.Mock1.Integration;
using FinHub.SharedKernel.Interfaces;
using MassTransit;
using MassTransit.PostgresOutbox.Extensions;

namespace FinHub.Mock1.Features.Create;

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
