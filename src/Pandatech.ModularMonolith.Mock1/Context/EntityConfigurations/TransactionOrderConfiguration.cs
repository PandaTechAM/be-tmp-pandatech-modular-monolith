using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pandatech.ModularMonolith.Mock1.Entities;

namespace Pandatech.ModularMonolith.Mock1.Context.EntityConfigurations;

public class TransactionOrderConfiguration : IEntityTypeConfiguration<TransactionOrderEntity>
{
   public void Configure(EntityTypeBuilder<TransactionOrderEntity> builder)
   {
      builder.HasKey(b => b.Id);

      builder.Property(b => b.UserId).IsRequired();
      builder.Property(b => b.Status).IsRequired();
      builder.Property(b => b.Amount).IsRequired();
      builder.Property(b => b.Narrative);
      builder.Property(b => b.CreatedAt).IsRequired();
      builder.Property(b => b.UpdatedAt).IsRequired();
   }
}
