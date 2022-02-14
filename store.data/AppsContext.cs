using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using store.core.Entities;
using store.core.Configuration;

namespace store.data
{
    public class AppsContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }
        public AppsContext(DbContextOptions<AppsContext> opt) : base(opt) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration<Item>(new ItemConfiguration());
            modelBuilder.ApplyConfiguration<Invoice>(new InvoiceConfiguration());
            modelBuilder.ApplyConfiguration<InvoiceDetail>(new InvoiceDetailConfiguration());
        }
    }
}