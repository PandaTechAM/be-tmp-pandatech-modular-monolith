using MassTransit.PostgresOutbox.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using Pandatech.ModularMonolith.Mock1.Integration;
using Pandatech.ModularMonolith.Mock2.Context;

namespace Pandatech.ModularMonolith.Mock2.Features;

public class TransactionOrderConsumer(ISender sender, IServiceProvider serviceScopeFactory)
    : InboxConsumer<TransactionOrderCreatedEvent, Mock2Context>(serviceScopeFactory)
{
    protected override async Task ConsumeAsync(TransactionOrderCreatedEvent message,
        IDbContextTransaction transactionScope, CancellationToken cancellationToken)
    {
        var command = new CreateTransactionV1Command
        {
            TransactionOrderId = message.Id,
            UserId = message.UserId,
            Amount = message.Amount,
            Narrative = message.Narrative
        };

        await sender.Send(command, cancellationToken);
    }
}
