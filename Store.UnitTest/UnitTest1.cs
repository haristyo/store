using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Microsoft.EntityFrameworkCore.
//using Microsoft.EntityFrameworkCore.
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.EntityFrameworkCore;
using store.data;
using store.core.Entities;
using System.Linq;

namespace Store.UnitTest
{
   public abstract class UTBase
    {
        private AppsContext _context;

        public AppsContext GetStoreDBContext()
        {
            DbContextOptionsBuilder<AppsContext> builder = new DbContextOptionsBuilder<AppsContext>();
            builder.UseInMemoryDatabase("db_store");
            _context = new AppsContext(builder.Options);
            return _context;
        }
    }

    [TestClass]
    public class UnitTest1 : UTBase
    {
    //private readonly AppsContext _context;
        //public UnitTest1()
        //{
        //    _context = context;
        //}
        [TestMethod]
        public void TestInputItem()
        {
            AppsContext _Context = GetStoreDBContext();
            Item item = new Item()
            {
                Name = "Beras",
                Price = 10000,
                Code = "B123"
            };
            _Context.Items.Add(item);
            _Context.SaveChanges();

            _Context = GetStoreDBContext();
            //newContext
            Item result = _Context.Items.Where(f => f.Name == "Beras").FirstOrDefault();
            Assert.IsNotNull(result);
            Assert.AreEqual("Beras", item.Name);
        }
        [TestMethod]
        public void TestInputInvoice()
        {
            AppsContext _Context = GetStoreDBContext();
            Invoice invoice = new Invoice()
            {
                InvoiceDate = System.DateTime.Now,
                InvoiceNo = 2703,
                // InvoiceDetails = null
            };
            _Context.Invoices.Add(invoice);
            _Context.SaveChanges();

            _Context = GetStoreDBContext();
            //newContext
            Invoice result = _Context.Invoices.Where(f => f.InvoiceNo == 2703).FirstOrDefault();
            Assert.IsNotNull(result);
            Assert.AreEqual(2703, invoice.InvoiceNo);
        }
        [TestMethod]
        public void TestInputInvoiceDetail()
        {
            AppsContext _Context = GetStoreDBContext();
            Item item = new Item()
            {
                Name = "Beras",
                Price = 10000,
                Code = "B123"
            };
            _Context.Items.Add(item);

            Invoice invoice = new Invoice()
            {
                InvoiceDate = System.DateTime.Now,
                InvoiceNo = 2703,
                // InvoiceDetails = null
            };
            _Context.Invoices.Add(invoice);

            invoice.addInvoiceDetail(1, item);
            _Context.SaveChanges();
            _Context = GetStoreDBContext();
            Invoice result = _Context.Invoices.Include(inv => inv.InvoiceDetails).ThenInclude(inv => inv.Item).FirstOrDefault();

            Assert.IsNotNull(result);
            Assert.AreEqual(1,invoice.InvoiceDetails.Count());

        }
    }
}
