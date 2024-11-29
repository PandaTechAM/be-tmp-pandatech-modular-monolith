using SharedKernel.ValidatorAndMediatR;

namespace Pandatech.ModularMonolith.Mock2.Features;

public class CreateTransactionV1Command : ICommand
{
   public long TransactionOrderId { get; set; }
   public long UserId { get; set; }
   public decimal Amount { get; set; }
   public required string Narrative { get; set; }
}