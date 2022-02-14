using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using store.core.Entities;
// using Microsoft.EntityFrameworkCore.R
// using Microsoft.Entity

namespace store.core.Configuration
{
    public class InvoiceDetailConfiguration : IEntityTypeConfiguration<InvoiceDetail>
    {
        public void Configure(EntityTypeBuilder<InvoiceDetail> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.HasOne(e => e.Invoice).WithMany(f => f.InvoiceDetails).HasForeignKey(e => e.InvoiceID).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.Item).WithMany().HasForeignKey(e => e.ItemId).OnDelete(DeleteBehavior.Restrict);
            // builder.Property(e=>e.Id)
            // builder.Property(e => e.Id).UseIdentityColumn();
            // builder.HasMany(e => e.InvoiceID).WithOne(d => d.InvoiceDetail).HasForeignKey(e => e.InvoiceID);
            // builder.HasMany(e=>e.Item).WithOne(d=>d.Id)
            // builder.HasOne(e => e.Invoice).WithMany(d => d.InvoiceDetail).HasForeignKey(e => e.InvoiceID);
            // builder.HasOne(e => e.Item).WithMany(d => d.InvoiceDetail).HasForeignKey(e => e.ItemId);
            //throw new System.NotImplementedException();
        }
    }
}
