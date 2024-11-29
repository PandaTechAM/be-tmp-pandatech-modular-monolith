using System.Data;
using EFCore.PostgresExtensions.Extensions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Pandatech.ModularMonolith.Mock1.Context;
using Pandatech.ModularMonolith.Mock1.Enums;
using SharedKernel.ValidatorAndMediatR;

namespace Pandatech.ModularMonolith.Mock1.Features.Update;

internal class UpdateTransactionOrderCommandHandler(Mock1Context mock1Context, IPublishEndpoint bus)
   : ICommandHandler<UpdateTransactionOrderCommand>
{
   public async Task Handle(UpdateTransactionOrderCommand request, CancellationToken cancellationToken)
   {
      using var transactionScope =
         await mock1Context.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);

      try
      {
         var order = await mock1Context.TransactionOrders
                                       .ForUpdate()
                                       .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
         if (order != null)
         {
            order.Status = Status.Processing;
         }
         // some update stuff...                          

         await transactionScope.CommitAsync(cancellationToken);
      }
      catch (Exception)
      {
         //todo: log
         transactionScope.Rollback();
         throw;
      }
   }
}