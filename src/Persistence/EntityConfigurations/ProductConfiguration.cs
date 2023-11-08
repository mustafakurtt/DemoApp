using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.EntityConfigurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products").HasKey("Id");
        builder.Property(u => u.Id).ValueGeneratedOnAdd();
        builder.Property(u => u.Name).HasColumnName("Name").IsRequired();
        builder.Property(u => u.Price).HasColumnName("Price").IsRequired();
        builder.Property(u => u.CategoryId).HasColumnName("CategoryId").IsRequired();
        builder.Property(u => u.SupplierId).HasColumnName("SupplierId").IsRequired();
        builder.Property(uoc => uoc.CreatedDate).HasColumnName("CreatedDate").IsRequired();
        builder.Property(uoc => uoc.UpdatedDate).HasColumnName("UpdatedDate");
        builder.Property(uoc => uoc.DeletedDate).HasColumnName("DeletedDate");
        builder.HasOne(b => b.Category);
        builder.HasOne(b => b.Supplier);
        builder.HasQueryFilter(oa => !oa.DeletedDate.HasValue);
    }
}