using FinHub.Mock1.Enums;

namespace FinHub.Mock1.Entities;

public class TransactionOrderEntity
{
   public long Id { get; set; }
   public long UserId { get; set; }
   public Status Status { get; set; } = Status.Enqueued;
   public decimal Amount { get; set; }
   public string Narrative { get; set; } = null!;
   public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
   public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
