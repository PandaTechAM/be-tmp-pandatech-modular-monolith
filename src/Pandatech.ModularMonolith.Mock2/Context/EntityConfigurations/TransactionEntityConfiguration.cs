using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pandatech.ModularMonolith.Mock2.Entities;

namespace Pandatech.ModularMonolith.Mock2.Context.EntityConfigurations;

public class TransactionEntityConfiguration : IEntityTypeConfiguration<TransactionEntity>
{
   public void Configure(EntityTypeBuilder<TransactionEntity> builder)
   {
      builder.HasKey(b => b.Id);

      builder.Property(b => b.TransactionOrderId).IsRequired();
      builder.Property(b => b.UserId).IsRequired();
      builder.Property(b => b.Amount).IsRequired();
      builder.Property(b => b.Narrative);
      builder.Property(b => b.CreatedAt).IsRequired();
      builder.Property(b => b.UpdatedAt).IsRequired();
   }
}
