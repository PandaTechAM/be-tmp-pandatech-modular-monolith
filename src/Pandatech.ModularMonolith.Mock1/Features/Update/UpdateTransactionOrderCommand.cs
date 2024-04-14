using FinHub.SharedKernel.Interfaces;

namespace FinHub.Mock1.Features.Update;

public class UpdateTransactionOrderCommand : ICommand
{
   public long Id { get; set; }
}
