using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Microsoft.EntityFrameworkCore.
//using Microsoft.EntityFrameworkCore.
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.EntityFrameworkCore;
using store.data;
using store.core.Entities;
using System.Linq;
using System;
using System.Collections.Generic;
using store.core.Repository;
using store.data.Repository;
using System.Threading;

namespace Store.UnitTest
{
   public abstract class UTBase
    {
        protected AppsContext _context;
        public void CleanData()
        {
            AppsContext _context = GetStoreDBContext();
            _context.Database.EnsureDeleted();

        }

        public AppsContext GetStoreDBContext()
        {
            DbContextOptionsBuilder<AppsContext> builder = new DbContextOptionsBuilder<AppsContext>();
            builder.UseInMemoryDatabase("db_store");
            _context = new AppsContext(builder.Options);
            _context.Database.EnsureCreated();
            return _context;
        }
        public IInvoiceRepository GetInvoiceRepository()
        {
            return new InvoiceRepository(GetStoreDBContext());
        }
        public IItemRepository GetItemRepository()
        {
            return new ItemRepository(GetStoreDBContext());
        }
        public IInvoiceDetailRepository GetInvoiceDetailRepository()
        {
            return new InvoiceDetailRepository(GetStoreDBContext());
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
        public UnitTest1()
        {
            CleanData();
        }
        public void initDataItem()
        {
            AppsContext _Context = GetStoreDBContext();
            List<Item> items = new List<Item>()
            {
                new Item()
                {
                Name = "Beras",
                Price = 10000,
                Code = "B123"
                },
                new Item()
                {
                Name = "Minyak",
                Price = 14000,
                Code = "M140"
                },
                new Item()
                {
                Name = "Gula",
                Price = 12000,
                Code = "G120"
                },
                new Item()
                {
                Name = "Terigu",
                Price = 9000,
                Code = "T090"
                },
            };

            _Context.Items.AddRange(items);
            _Context.SaveChanges();
        }
        [TestMethod]
        public void TestInputItem()
        {
            initDataItem();
            AppsContext _Context = GetStoreDBContext();
 
            //newContext
            Item result = _Context.Items.Where(f => f.Name == "Beras").FirstOrDefault();
            Assert.IsNotNull(result);
            Assert.AreEqual("Beras", result.Name);
        }
        [TestMethod]
        public void TestUpdateItem()
        {

            initDataItem();
            AppsContext _Context = GetStoreDBContext();
            Item updated_item = _Context.Items.Where(f => f.Name == "Beras").FirstOrDefault();

            updated_item.Name = "Minyak";
            updated_item.Price = 14000;
            updated_item.Code = "M140";
            _Context.SaveChanges();

            _Context = GetStoreDBContext();
            Item result = _Context.Items.Where(f => f.Name == "Beras").FirstOrDefault();

            Assert.IsNull(result);
            Assert.AreEqual("Minyak", updated_item.Name);
        }

        [TestMethod]
        public void TestRemoveItem()
        {
            AppsContext _Context = GetStoreDBContext();
            initDataItem();
            Item item = _Context.Items.FirstOrDefault(f => f.Name == "Beras");

            _Context = GetStoreDBContext();
            Item targetDeleteItem = _Context.Items.FirstOrDefault(f => f.Name == "Beras");
            //_Context = GetStoreDBContext();
            _Context.Items.Remove(targetDeleteItem);
            _Context.SaveChanges();

            _Context = GetStoreDBContext();
            List<Item> results = _Context.Items.Where(e => e.Name == "Beras").ToList();
            Item result = _Context.Items.Where(e => e.Name == "Beras").FirstOrDefault();
            Assert.IsNull(result);
            //Assert.IsNull()
            //_Context = GetStoreDBContext();
            Assert.AreEqual(0, results.Count());

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
        public void TestUpdateInvoice()
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
            Invoice targetUpdate = _Context.Invoices.Where(f => f.InvoiceNo == 2703).FirstOrDefault();
            targetUpdate.InvoiceNo = 1234;
            targetUpdate.InvoiceDate = DateTime.Now;
            _Context.SaveChanges();

            _Context = GetStoreDBContext();
            Invoice invoices = _Context.Invoices.Where(f => f.InvoiceNo == 1234).FirstOrDefault();
            Assert.IsNotNull(invoices);
            //Console.WriteLine(invoices.Count());
            _Context = GetStoreDBContext();
            Assert.AreEqual(2703, invoice.InvoiceNo);
        }

        [TestMethod]
        public void TestUpdateInvoiceCara2()
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
            invoice = new Invoice()
            {
                Id = 1,
                InvoiceDate = System.DateTime.Now,
                InvoiceNo = 1234,
                // InvoiceDetails = null
            };
            Invoice targetUpdatedInvoice = _Context.Invoices.Where(f => f.InvoiceNo == 2703).FirstOrDefault();
            _Context.Entry<Invoice>(targetUpdatedInvoice).CurrentValues.SetValues(invoice); 
            _Context.SaveChanges();

            _Context = GetStoreDBContext();
            Invoice updatedInvoice = _Context.Invoices.Where(f => f.InvoiceNo == 1234).FirstOrDefault();
            Assert.IsNotNull(updatedInvoice);
            _Context = GetStoreDBContext();
            Assert.AreEqual(1234, invoice.InvoiceNo);
        }
        [TestMethod]
        public void TestUpdateInvoiceCara3()
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
            Invoice invoiceUpdate = new Invoice()
            {
                Id = 1,
                InvoiceDate = System.DateTime.Now,
                InvoiceNo = 1234,
                // InvoiceDetails = null
            };
            //Invoice targetUpdatedInvoice = _Context.Invoices.Where(f => f.InvoiceNo == 2703).FirstOrDefault();
            //_Context.Entry<Invoice>(targetUpdatedInvoice).CurrentValues.SetValues(invoice);
            _Context = GetStoreDBContext();
            _Context.Update(invoiceUpdate);
            _Context.SaveChanges();

            _Context = GetStoreDBContext();
            Invoice updatedInvoice = _Context.Invoices.Where(f => f.InvoiceNo == 1234).FirstOrDefault();
            Assert.IsNotNull(updatedInvoice);
            _Context = GetStoreDBContext();
            Invoice targetUpdatedInvoice = _Context.Invoices.Where(f => f.InvoiceNo == 2703).FirstOrDefault();
            Assert.IsNull(targetUpdatedInvoice);
        }



        [TestMethod]
        public void TestRemoveInvoice()
        {
            AppsContext _Context = GetStoreDBContext();
            initDataItem();
            Item item = _Context.Items.FirstOrDefault(f => f.Name == "Beras");

            Invoice invoice = new Invoice()
            {
                InvoiceDate = System.DateTime.Now,
                InvoiceNo = 2703
            };
            _Context.Invoices.Add(invoice);

            invoice.addInvoiceDetail(1, item);
            invoice.addInvoiceDetail(5, item);
            _Context.SaveChanges();
            _Context = GetStoreDBContext();
            Invoice result = _Context.Invoices.Include(inv => inv.InvoiceDetails).ThenInclude(inv => inv.Item).FirstOrDefault();

            Assert.IsNotNull(result);
            Assert.AreEqual(2, invoice.InvoiceDetails.Count());

            _Context = GetStoreDBContext();
            AppsContext _DeleteContext = GetStoreDBContext();
            Invoice invoiceHapus = _Context.Invoices.FirstOrDefault(f => f.InvoiceNo == 2703);
            foreach (InvoiceDetail ivd in invoice.InvoiceDetails)
            {
                _DeleteContext = GetStoreDBContext();
                InvoiceDetail targetHapus = _DeleteContext.InvoiceDetails.FirstOrDefault(f => f.Id == ivd.Id);
                Console.WriteLine(ivd);
                //invoiceHapus.removeInvoiceDetail(targetHapus);
                //_Context.SaveChanges();
            }
            _Context.SaveChanges();
            _Context = GetStoreDBContext();
            Invoice hapusInvoice = _Context.Invoices.Where(f => f.InvoiceNo == 2703).FirstOrDefault();
            _Context.Invoices.Remove(hapusInvoice);
            _Context.SaveChanges();
            Assert.AreEqual(0, _Context.Invoices.Count());

        }

        [TestMethod]
        public void TestInputInvoiceDetail()
        {
            AppsContext _Context = GetStoreDBContext();
            initDataItem();
            Item item = _Context.Items.FirstOrDefault(f => f.Name == "Beras");

            Invoice invoice = new Invoice()
            {
                InvoiceDate = System.DateTime.Now,
                InvoiceNo = 2703
            };
            _Context.Invoices.Add(invoice);

            invoice.addInvoiceDetail(1, item);
            invoice.addInvoiceDetail(5, item);
            _Context.SaveChanges();
            _Context = GetStoreDBContext();
            Invoice result = _Context.Invoices.Include(inv => inv.InvoiceDetails).ThenInclude(inv => inv.Item).FirstOrDefault();

            Assert.IsNotNull(result);
            Assert.AreEqual(2,invoice.InvoiceDetails.Count());

        }

        [TestMethod]
        public void TestUpdateInvoiceDetail()
        {
            AppsContext _Context = GetStoreDBContext();
            initDataItem();
            Item item = _Context.Items.FirstOrDefault(f => f.Name == "Beras");

            Invoice invoice = new Invoice()
            {
                InvoiceDate = System.DateTime.Now,
                InvoiceNo = 2703
            };
            _Context.Invoices.Add(invoice);

            invoice.addInvoiceDetail(1, item);
            invoice.addInvoiceDetail(5, item);
            _Context.SaveChanges();

            _Context = GetStoreDBContext();
            Invoice result = _Context.Invoices.Include(inv => inv.InvoiceDetails).ThenInclude(inv => inv.Item).FirstOrDefault();
            InvoiceDetail targetUpdate =  result.InvoiceDetails.FirstOrDefault(f => f.Qty == 1);
            targetUpdate.Qty = 25;
            _Context.SaveChanges();


            //result
            _Context = GetStoreDBContext();
            Invoice updatedInvoiceDetail = _Context.Invoices.Include(inv => inv.InvoiceDetails).ThenInclude(inv => inv.Item).FirstOrDefault();
            
            Assert.AreEqual(1, updatedInvoiceDetail.InvoiceDetails.Where(f => f.Qty == 25).Count());
            //Assert.AreNotEqual()
            Assert.AreEqual(0, updatedInvoiceDetail.InvoiceDetails.Where(f => f.Qty == 1).Count());
            Assert.AreNotEqual(1, updatedInvoiceDetail.InvoiceDetails.Where(f => f.Qty == 1).Count());

        }


        [TestMethod]
        public void TestRemoveInvoiceDetail()
        {
            
            AppsContext _Context = GetStoreDBContext();
            initDataItem();
            Item item = _Context.Items.FirstOrDefault(f => f.Name == "Beras");

            Invoice invoice = new Invoice()
            {
                InvoiceDate = System.DateTime.Now,
                InvoiceNo = 2703
            };
            _Context.Invoices.Add(invoice);

            invoice.addInvoiceDetail(1, item); //masukkan quantity dan object item pertama
            invoice.addInvoiceDetail(5, item); //masukkan quantity dan object item kedua
            _Context.SaveChanges();
            InvoiceDetail target = invoice.InvoiceDetails.Where(f => f.Id == 1).FirstOrDefault();
            invoice.removeInvoiceDetail(target); // hapus pada entity invoice dengan object detailInvoice adalah target

            _Context.SaveChanges();
            _Context = GetStoreDBContext();
            InvoiceDetail objectTerhapus = invoice.InvoiceDetails.Where(f => f.Id == 1).FirstOrDefault();
            Assert.IsNull(objectTerhapus);
            Assert.AreEqual(1, invoice.InvoiceDetails.Count()); //setelah hapus 1, maka sisa 1, jika diisi 2 maka salah, jika isi 1 maka benar
        }

        [TestMethod]
        public void UpdateInvoiceDetail()
        {
            AppsContext _Context = GetStoreDBContext();
            initDataItem();
            Invoice invoice = new Invoice()
            {
                InvoiceDate = System.DateTime.Now,
                InvoiceNo = 1234
            };
            Item item = _Context.Items.FirstOrDefault(f => f.Name == "Beras");
            //invoice.addInvoiceDetail(5, item);
            invoice.addInvoiceDetailFull(new InvoiceDetail()
            {
                ItemId = item.Id,
             
                InvoiceID = invoice.Id,
                Qty = 5,
                Price = item.Price * 5,
            });
            item = _Context.Items.FirstOrDefault(f => f.Name == "Minyak");
            //invoice.addInvoiceDetail(2, item);
            invoice.addInvoiceDetailFull(new InvoiceDetail()
            {
                ItemId = item.Id,
                InvoiceID = invoice.Id,
                Qty = 5,
                Price = item.Price * 5,
            });

            _Context.Invoices.Add(invoice);
            _Context.SaveChanges();


            //_Context = GetStoreDBContext();
        //Buat seakan2 new Request
            Invoice newRequest = new Invoice()
            {
                Id = 1,
                InvoiceDate = System.DateTime.Now,
                InvoiceNo = 4321
            };

            //Hapus Beras
            //item = _Context.Items.FirstOrDefault(f => f.Name == "Beras");
            //invoice.addInvoiceDetail(5, item);
            //newRequest.addInvoiceDetailFull(new InvoiceDetail()
            //{
            //    Id = invoice.InvoiceDetails[0].Id,
            //    ItemId = item.Id,
            //    InvoiceID = newRequest.Id,
            //    Qty = 4,
            //    Price = item.Price * 4,
            //});

            //ubah minyak jadi 4
            item = _Context.Items.FirstOrDefault(f => f.Name == "Minyak");
            //newRequest.addInvoiceDetail(4, item);

            newRequest.addInvoiceDetailFull(new InvoiceDetail()
            {
                Id = invoice.InvoiceDetails[1].Id,
                ItemId = item.Id,
                InvoiceID = newRequest.Id,
                Qty = 4,
                Price = item.Price * 4,
            });

            //tambah barang Terigu dan gula
            item = _Context.Items.FirstOrDefault(f => f.Name == "Gula");
            //newRequest.addInvoiceDetail(2, item);
            newRequest.addInvoiceDetailFull(new InvoiceDetail()
            {
                ItemId = item.Id,
                InvoiceID = newRequest.Id,
                Qty = 2,
                Price = item.Price * 2,
            });

            item = _Context.Items.FirstOrDefault(f => f.Name == "Terigu");
            //newRequest.addInvoiceDetail(5, item);
            newRequest.addInvoiceDetailFull(new InvoiceDetail()
            {
                ItemId = item.Id,
                InvoiceID = newRequest.Id,
                Qty = 5,
                Price = item.Price * 5,
            });

            _Context = GetStoreDBContext();
            invoice = _Context.Invoices.FirstOrDefault(f => f.Id == 1);
            //_Context = GetStoreDBContext();
            //var transaksiExist = _Context.InvoiceDetails.Where(f => f.InvoiceID == 1).ToList();
            List<InvoiceDetail> transaksiExist = _Context.InvoiceDetails.Where(f => f.InvoiceID == 1).ToList();
            //transaksi
            //Console.WriteLine(newRequest.InvoiceDetails);
            foreach(var newItem in newRequest.InvoiceDetails)
            {
                //newItem.InvoiceID = 1;
                if (transaksiExist.Any(f=>f.Id == newItem.Id))
                {
                    InvoiceDetail transaksi = transaksiExist.First(f => f.Id == newItem.Id);
                    _Context.Entry<InvoiceDetail>(transaksi).CurrentValues.SetValues(newItem);
                    transaksiExist.Remove(transaksi);
                }
                else
                {
                    var addedTransaction =  newItem;
                    _Context.InvoiceDetails.Add(newItem);

                    //invoice.addInvoiceDetailFull(new InvoiceDetail()
                    //{
                        
                    //    ItemId = newItem.Id,
                    //    InvoiceID = newItem.Invoice.Id,
                    //    Qty = newItem.Qty,
                    //    Price = newItem.Price * newItem.Qty
                    //});
                    
                };
            }
            if(transaksiExist.Count() > 0)
            {
                foreach(var deletedTransaksi in transaksiExist)
                {
                    _Context.InvoiceDetails.Remove(deletedTransaksi);
                }
            }
            _Context.Entry<Invoice>(invoice).CurrentValues.SetValues(newRequest);
            _Context.SaveChanges();

            _Context = GetStoreDBContext();
            invoice = _Context.Invoices.Include(f => f.InvoiceDetails).ThenInclude(f => f.Item).FirstOrDefault(f => f.Id == 1);
            Assert.AreEqual(3, invoice.InvoiceDetails.Count());
            Assert.IsNotNull(invoice.InvoiceDetails.FirstOrDefault(f => f.Item.Name == "Minyak"));
            Assert.IsNotNull(invoice.InvoiceDetails.FirstOrDefault(f => f.Item.Name == "Terigu"));
            Assert.IsNotNull(invoice.InvoiceDetails.FirstOrDefault(f => f.Item.Name == "Gula"));
            //Assert.IsNotNull(invoice.InvoiceDetails.FirstOrDefault(f => f.Item.Name == "Beras"));
            Assert.IsNull(invoice.InvoiceDetails.FirstOrDefault(f => f.Item.Name == "Beras")); 

        }

        [TestMethod]
        public void ClearInvoiceDetail()
        {
            CleanData();
            AppsContext _Context = GetStoreDBContext();
            initDataItem();
            Invoice invoice = new Invoice()
            {
                InvoiceDate = System.DateTime.Now,
                InvoiceNo = 1234
            };
            Item item = _Context.Items.FirstOrDefault(f => f.Name == "Beras");
            //invoice.addInvoiceDetail(5, item);
            invoice.addInvoiceDetailFull(new InvoiceDetail()
            {
                ItemId = item.Id,

                InvoiceID = invoice.Id,
                Qty = 5,
                Price = item.Price * 5,
            });
            item = _Context.Items.FirstOrDefault(f => f.Name == "Minyak");
            //invoice.addInvoiceDetail(2, item);
            invoice.addInvoiceDetailFull(new InvoiceDetail()
            {
                ItemId = item.Id,
                InvoiceID = invoice.Id,
                Qty = 5,
                Price = item.Price * 5,
            });

            _Context.Invoices.Add(invoice);
            _Context.SaveChanges();

            _Context = GetStoreDBContext();
            Invoice invoiceDelete = _Context.Invoices.Include(f=>f.InvoiceDetails).ThenInclude(e=>e.Item).FirstOrDefault(f => f.Id == 1);
            //invoiceDelete.InvoiceDetails
            foreach(var itemDeleted in invoiceDelete.InvoiceDetails)
            {
                _Context.InvoiceDetails.Remove(itemDeleted);
                //invoiceDelete.removeInvoiceDetail(itemDeleted);
                //invoiceDelete.removeInvoiceDetail(itemDeleted);
            }
            //_Context.Invoices.Remove(invoiceDelete);
            //_Context.SaveChanges();

            _Context = GetStoreDBContext();
            Invoice hasilHapus = _Context.Invoices.FirstOrDefault(f => f.Id==1);
            Assert.AreEqual(0, hasilHapus.InvoiceDetails.Count());
            //List _Context
            //_Context.Invoices.Remove(invoiceDelete);
            _Context = GetStoreDBContext();
            Invoice targetDelete = _Context.Invoices.FirstOrDefault(f => f.Id == 1);
            _Context.Invoices.Remove(targetDelete);
            _Context.SaveChanges();
            Assert.IsNull(_Context.Invoices.FirstOrDefault(f=>f.Id==1));

        }







    }
    [TestClass]
    public class UnitTest2 : UTBase
    {
        //AppsContext _Context = GetStoreDBContext();
        public UnitTest2()
        {
            CleanData();
        }
        [TestMethod]
        public void InsertInvoiceWithRepository()
        {
            IInvoiceRepository invoiceRepository = GetInvoiceRepository();
            
            Invoice invoice = new Invoice()
            {
                InvoiceDate = System.DateTime.Now,
                InvoiceNo = 4646,
            };
            invoiceRepository.Insert(invoice);
            _context.SaveChanges();

            invoiceRepository = GetInvoiceRepository();
            invoice = new Invoice()
            {
                InvoiceDate = System.DateTime.Now,
                InvoiceNo = 1234,
            };
            invoiceRepository.Insert(invoice);
            _context.SaveChanges();

            invoiceRepository = GetInvoiceRepository();
            Invoice newInvoice = invoiceRepository.getSingle(invoice.Id).Result;
            Assert.IsNotNull(newInvoice);
            List<Invoice> listInvoice = invoiceRepository.GetList().Result;
            Assert.AreEqual(2, listInvoice.Count());
        }


        [TestMethod]
        public void UpdateInvoiceWithRepository()
        {
            IInvoiceRepository invoiceRepository = GetInvoiceRepository();
            Invoice invoice = new Invoice()
            {
                InvoiceDate = System.DateTime.Now,
                InvoiceNo = 4646,
            };
            invoiceRepository.Insert(invoice);
            _context.SaveChanges();

            Invoice invoiceRequest = new Invoice()
            {
                Id = invoice.Id,
                InvoiceDate = System.DateTime.Now,
                InvoiceNo = 1234,
            };


            invoiceRepository = GetInvoiceRepository();
            Invoice newInvoice = invoiceRepository.getSingle(invoice.Id).Result;
            Assert.IsNotNull(newInvoice);

            invoiceRepository.Update(invoiceRequest, invoiceRequest.Id);
            _context.SaveChanges();

            invoiceRepository = GetInvoiceRepository();
            List<Invoice> listInvoice = invoiceRepository.GetList().Result;
            Assert.AreEqual(1234, listInvoice[0].InvoiceNo);
        }
        [TestMethod]
        public void DeleteInvoiceWithRepository()
        {
            IInvoiceRepository invoiceRepository = GetInvoiceRepository();
            Invoice invoice = new Invoice()
            {
                InvoiceDate = System.DateTime.Now,
                InvoiceNo = 4646,
            };
            invoiceRepository.Insert(invoice);
            _context.SaveChanges();


            invoiceRepository = GetInvoiceRepository();
            Invoice newInvoice = invoiceRepository.getSingle(1).Result;
            Assert.IsNotNull(newInvoice);

            invoiceRepository = GetInvoiceRepository();
            invoiceRepository.Delete(invoice.Id);
            _context.SaveChanges();

            invoiceRepository = GetInvoiceRepository();
            List<Invoice> listInvoice = invoiceRepository.GetList().Result;
            Assert.AreEqual(0, listInvoice.Count());
        }

        [TestMethod]
        public void InsertItemWithRepository()
        {
            IItemRepository itemRepository = GetItemRepository();
            Item item= new Item()
            {
                Name = "Baju",
                Code = "BA123",
                Price = 25000
            };
            itemRepository.Insert(item);
            _context.SaveChanges();

            itemRepository = GetItemRepository();
            item = new Item()
            {
                Name = "Celana",
                Code = "Ce123",
                Price = 25000
            };
            itemRepository.Insert(item);
            _context.SaveChanges();

            itemRepository = GetItemRepository();
            Item newItem = itemRepository.getSingle(item.Id).Result;
            Assert.IsNotNull(newItem);
            List<Item> listItem = itemRepository.GetList().Result;
            Assert.AreEqual(2, listItem.Count());

        }

        [TestMethod]
        public void UpdateItemWithRepository()
        {
            IItemRepository itemRepository = GetItemRepository();
            Item item = new Item()
            {
                Name = "Baju",
                Code = "BA123",
                Price = 25000
            };
            itemRepository.Insert(item);
            _context.SaveChanges();

            Item itemRequest = new Item()
            {
                Id = item.Id,
                Name = "Baju",
                Code = "BA130",
                Price = 30000
            };


            itemRepository = GetItemRepository();
            Item newItem = itemRepository.getSingle(item.Id).Result;
            Assert.IsNotNull(newItem);

            itemRepository.Update(itemRequest, itemRequest.Id);
            _context.SaveChanges();

            itemRepository = GetItemRepository();
            List<Item> listItem = itemRepository.GetList().Result;
            Assert.AreEqual("BA130", listItem[0].Code);
        }

        [TestMethod]
        public void DeleteItemWithRepository()
        {
            IItemRepository itemRepository = GetItemRepository();
            Item item = new Item()
            {
                Name = "Baju",
                Code = "BA123",
                Price = 25000
            };
            itemRepository.Insert(item);
            _context.SaveChanges();

            itemRepository = GetItemRepository();
            Item newItem = itemRepository.getSingle(item.Id).Result;
            Assert.IsNotNull(newItem);

            itemRepository = GetItemRepository();
            itemRepository.Delete(newItem.Id);
            _context.SaveChanges();

            itemRepository = GetItemRepository();
            List<Item> listItem = itemRepository.GetList().Result;
            Assert.AreEqual(0, listItem.Count());
        }





        [TestMethod]
        public void InsertInvoiceDetailWithRepository()
        {
            IItemRepository itemRepository = GetItemRepository();
            Item item = new Item()
            {
                Name = "Baju",
                Code = "BA123",
                Price = 25000
            };
            itemRepository.Insert(item);
            _context.SaveChanges();

            IInvoiceRepository invoiceRepository = GetInvoiceRepository();

            Invoice invoice = new Invoice()
            {
                InvoiceDate = System.DateTime.Now,
                InvoiceNo = 4646,
            };
            invoiceRepository.Insert(invoice);
            _context.SaveChanges();

            itemRepository = GetItemRepository();
            Item targetItem = itemRepository.getSingle(item.Id).Result;

            invoiceRepository = GetInvoiceRepository();
            Invoice targetInvoice = invoiceRepository.getSingle(invoice.Id).Result;

            IInvoiceDetailRepository invoiceDetailRepository = GetInvoiceDetailRepository();
            InvoiceDetail invoiceDetail = new InvoiceDetail()
            {
                InvoiceID = targetInvoice.Id,
                ItemId = targetItem.Id,
                Qty = 5,
                Price = targetItem.Price * 5
            };
            invoiceDetailRepository.Insert(invoiceDetail);
            _context.SaveChanges();


            invoiceDetailRepository = GetInvoiceDetailRepository();
            InvoiceDetail newInvoiceDetail = invoiceDetailRepository.getSingle(invoiceDetail.Id).Result;
            Assert.IsNotNull(newInvoiceDetail);

            List<InvoiceDetail> listInvoiceDetail = invoiceDetailRepository.GetList().Result;
            Assert.AreEqual(1, listInvoiceDetail.Count());

        }

        [TestMethod]
        public void UpdateInvoiceDetailWithRepository()
        {
            IItemRepository itemRepository = GetItemRepository();
            Item item = new Item()
            {
                Name = "Baju",
                Code = "BA123",
                Price = 25000
            };
            itemRepository.Insert(item);
            _context.SaveChanges();

            IInvoiceRepository invoiceRepository = GetInvoiceRepository();

            Invoice invoice = new Invoice()
            {
                InvoiceDate = System.DateTime.Now,
                InvoiceNo = 4646,
            };
            invoiceRepository.Insert(invoice);
            _context.SaveChanges();

            itemRepository = GetItemRepository();
            Item targetItem = itemRepository.getSingle(item.Id).Result;

            invoiceRepository = GetInvoiceRepository();
            Invoice targetInvoice = invoiceRepository.getSingle(invoice.Id).Result;

            IInvoiceDetailRepository invoiceDetailRepository = GetInvoiceDetailRepository();
            InvoiceDetail invoiceDetail = new InvoiceDetail()
            {
                InvoiceID = targetInvoice.Id,
                ItemId = targetItem.Id,
                Qty = 5,
                Price = targetItem.Price * 5
            };
            invoiceDetailRepository.Insert(invoiceDetail);
            _context.SaveChanges();


            invoiceDetailRepository = GetInvoiceDetailRepository();
            InvoiceDetail newInvoiceDetail = invoiceDetailRepository.getSingle(invoiceDetail.Id).Result;
            Assert.IsNotNull(newInvoiceDetail);

            List<InvoiceDetail> listInvoiceDetail = invoiceDetailRepository.GetList().Result;
            Assert.AreEqual(1, listInvoiceDetail.Count());


            InvoiceDetail requestInvoiceDetail = new InvoiceDetail()
            {
                Id = newInvoiceDetail.Id,
                InvoiceID = newInvoiceDetail.Id,
                ItemId = targetItem.Id,
                Qty = 7,
                Price = targetItem.Price * 7
            };
            invoiceDetailRepository = GetInvoiceDetailRepository();
            invoiceDetailRepository.Update(requestInvoiceDetail, requestInvoiceDetail.Id);
            _context.SaveChanges();
            InvoiceDetail updatedInvoiceDetail = invoiceDetailRepository.getSingle(invoiceDetail.Id).Result;
            Assert.AreEqual(7, updatedInvoiceDetail.Qty);


        }

        [TestMethod]
        public void DeleteInvoiceDetailWithRepository()
        {
            IItemRepository itemRepository = GetItemRepository();
            Item item = new Item()
            {
                Name = "Baju",
                Code = "BA123",
                Price = 25000
            };
            itemRepository.Insert(item);
            _context.SaveChanges();

            IInvoiceRepository invoiceRepository = GetInvoiceRepository();

            Invoice invoice = new Invoice()
            {
                InvoiceDate = System.DateTime.Now,
                InvoiceNo = 4646,
            };
            invoiceRepository.Insert(invoice);
            _context.SaveChanges();

            itemRepository = GetItemRepository();
            Item targetItem = itemRepository.getSingle(item.Id).Result;

            invoiceRepository = GetInvoiceRepository();
            Invoice targetInvoice = invoiceRepository.getSingle(invoice.Id).Result;

            IInvoiceDetailRepository invoiceDetailRepository = GetInvoiceDetailRepository();
            InvoiceDetail invoiceDetail = new InvoiceDetail()
            {
                InvoiceID = targetInvoice.Id,
                ItemId = targetItem.Id,
                Qty = 5,
                Price = targetItem.Price * 5
            };
            invoiceDetailRepository.Insert(invoiceDetail);
            _context.SaveChanges();


            invoiceDetailRepository = GetInvoiceDetailRepository();
            InvoiceDetail newInvoiceDetail = invoiceDetailRepository.getSingle(invoiceDetail.Id).Result;
            Assert.IsNotNull(newInvoiceDetail);

            List<InvoiceDetail> listInvoiceDetail = invoiceDetailRepository.GetList().Result;
            Assert.AreEqual(1, listInvoiceDetail.Count());

            invoiceDetailRepository = GetInvoiceDetailRepository();
            invoiceDetailRepository.Delete(invoiceDetail.Id);
            _context.SaveChanges();

            invoiceDetailRepository = GetInvoiceDetailRepository();
            newInvoiceDetail = invoiceDetailRepository.getSingle(invoiceDetail.Id).Result;
            Assert.IsNull(newInvoiceDetail);

            listInvoiceDetail = invoiceDetailRepository.GetList().Result;
            Assert.AreEqual(0, listInvoiceDetail.Count());


        }
    }

}

