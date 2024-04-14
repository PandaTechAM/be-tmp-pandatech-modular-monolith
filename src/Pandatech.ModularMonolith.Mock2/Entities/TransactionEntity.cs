namespace FinHub.Mock2.Entities;

public class TransactionEntity
{
   public long Id { get; set; }
   public long TransactionOrderId { get; set; }
   public long UserId { get; set; }
   public decimal Amount { get; set; }
   public string Narrative { get; set; } = null!;
   public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
   public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
