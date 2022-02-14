using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using store.core.Entities;
// using Microsoft.EntityFrameworkCore.R
// using Microsoft.Entity

namespace store.core.Configuration
{
    public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
    {
        public void Configure(EntityTypeBuilder<Invoice> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            // builder.HasMany(inv => inv.InvoiceDetail).WithOne(inv => inv.Invoice).HasForeignKey(f => f.InvoiceID);
            // builder.Has
            // builder.Property(e=>e.Id)
            // builder.Property(e => e.Id).UseIdentityColumn();
            // builder.HasMany(e => e.InvoiceDetail).WithOne(d => d.Invoice).HasForeignKey(e => e.InvoiceID);
            //throw new System.NotImplementedException();
        }
    }
}
