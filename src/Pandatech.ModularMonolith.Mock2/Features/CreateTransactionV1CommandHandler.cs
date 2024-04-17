using FinHub.Mock2.Context;
using FinHub.Mock2.Entities;
using Pandatech.ModularMonolith.SharedKernel.Interfaces;
using ResponseCrafter.StandardHttpExceptions;

namespace FinHub.Mock2.Features;

public class CreateTransactionV1CommandHandler(PostgresContext postgresContext)
   : ICommandHandler<CreateTransactionV1Command>
{
   public async Task Handle(CreateTransactionV1Command request, CancellationToken cancellationToken)
   {
      var transaction = new TransactionEntity
      {
         TransactionOrderId = request.TransactionOrderId,
         UserId = request.UserId,
         Amount = request.Amount,
         Narrative = request.Narrative
      };

      await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);

      Console.WriteLine(
         "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");

      if (transaction.TransactionOrderId == 13)
      {
         throw new InternalServerErrorException("I don't know what happened");
      }

      postgresContext.Transactions.Add(transaction);

      await postgresContext.SaveChangesAsync(cancellationToken);
   }
}
