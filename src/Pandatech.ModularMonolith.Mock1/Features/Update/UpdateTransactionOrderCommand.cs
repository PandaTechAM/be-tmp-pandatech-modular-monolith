using SharedKernel.ValidatorAndMediatR;

namespace Pandatech.ModularMonolith.Mock1.Features.Update;

public class UpdateTransactionOrderCommand : ICommand
{
   public long Id { get; set; }
}