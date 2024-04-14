namespace FinHub.Mock1.Integration;

public record TransactionOrderCreatedEvent(long Id, long UserId, decimal Amount, string Narrative);
