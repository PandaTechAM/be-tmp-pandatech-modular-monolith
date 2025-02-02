using Pandatech.ModularMonolith.Mock2.Context;
using Pandatech.ModularMonolith.Mock2.Entities;
using ResponseCrafter.HttpExceptions;
using SharedKernel.ValidatorAndMediatR;

namespace Pandatech.ModularMonolith.Mock2.Features;

public class CreateTransactionV1CommandHandler(Mock2Context mock2Context)
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

      mock2Context.Transactions.Add(transaction);

      await mock2Context.SaveChangesAsync(cancellationToken);
   }
}