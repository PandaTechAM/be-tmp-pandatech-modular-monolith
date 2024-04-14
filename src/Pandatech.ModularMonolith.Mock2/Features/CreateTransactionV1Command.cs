using FinHub.SharedKernel.Interfaces;

namespace FinHub.Mock2.Features;

public class CreateTransactionV1Command : ICommand
{
   public long TransactionOrderId { get; set; }
   public long UserId { get; set; }
   public decimal Amount { get; set; }
   public string Narrative { get; set; }
}
