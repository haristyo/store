using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using store.core.Entities;
// using Microsoft.EntityFrameworkCore.R
// using Microsoft.Entity

namespace store.core.Configuration
{
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            // builder.HasMany(inv => inv.InvoiceDetail).WithOne(d => d.Item).HasForeignKey("ItemId");
            // builder.HasMany(inv => inv.InvoiceDetail).WithOne(it => it.Item).HasForeignKey("ItemId");
            // builder.Property(e=>e.Id)
            // builder.Property(e => e.Id).UseIdentityColumn();
            // builder.HasMany(e => e.InvoiceDetail).WithOne(d => d.Item).HasForeignKey(e => e.ItemId);
            // builder.HasMany(e => e.InvoiceDetail).WithOne(d => d.Item).HasForeignKey(e => e.ItemId);
            //throw new System.NotImplementedException();
        }
    }
}
