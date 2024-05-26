using System.Data;
using EFCore.PostgresExtensions.Extensions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Pandatech.ModularMonolith.Mock1.Context;
using Pandatech.ModularMonolith.Mock1.Enums;
using Pandatech.ModularMonolith.SharedKernel.Interfaces;

namespace Pandatech.ModularMonolith.Mock1.Features.Update;

internal class UpdateTransactionOrderCommandHandler(PostgresContext postgresContext, IPublishEndpoint bus)
   : ICommandHandler<UpdateTransactionOrderCommand>
{
   public async Task Handle(UpdateTransactionOrderCommand request, CancellationToken cancellationToken)
   {
      using var transactionScope =
         await postgresContext.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, cancellationToken);

      try
      {
         var order = await postgresContext.TransactionOrders
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
