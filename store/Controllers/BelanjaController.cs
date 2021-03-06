using Microsoft.AspNetCore.Mvc;
using store.core.Entities;
using store.data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using AutoMapper;
using System.Threading;
using store.Profiles.DTO;
using System.Linq;

namespace testing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BelanjaController : ControllerBase
    {
        private readonly AppsContext _context;
        private readonly IMapper _mapper;
        public BelanjaController(AppsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [Route("addItem")]
        public void initItem()
        {

            Item item = new Item()
            {
                Name = "Baju",
                Price = 10000,
                Code = "B100"
            };
            _context.Items.Add(item);
            item = new Item()
            {
                Name = "Celana",
                Price = 15000,
                Code = "B150"
            };
            _context.Items.Add(item);
            _context.SaveChanges();
            //return Ok(_context.Items.ToList());
        }
        [HttpGet]
        [Route("initItem")]
        public ActionResult<List<Item>> initItemData()
        {
            initItem();
            List<Item> items = _context.Items.ToList();
            return Ok(items);
        }

        [HttpGet]
        [Route("Invoice")]
        public async Task<ActionResult<InvoiceDTO>> Get(CancellationToken cancellationToken = default)
        {
            Item item = new Item()
            {
                Name = "Baju",
                Price = 10000
            };
            _context.Items.Add(item);

            // {
            Invoice invoice = new Invoice();
            invoice.InvoiceNo = 2703;
            invoice.InvoiceDate = System.DateTime.Now;
            _context.Invoices.Add(invoice);
            // _context.Items.Add(item);
            // };
            InvoiceDetail inputInvoiceDetail = new InvoiceDetail()
            {
                ItemId = item.Id,
                Item = item,
                InvoiceID = invoice.Id,
                Invoice = invoice,
                Price = item.Price,
                Qty = 1,
            };
            // invoice.addInvoiceDetailFull(inputInvoiceDetail);
            invoice.addInvoiceDetail(100, item.Id);
            // _context.Invoices.Add(invoice);
            _context.SaveChanges();
            await _context.SaveChangesAsync(cancellationToken);
            InvoiceDTO invoiceDTO = _mapper.Map<Invoice, InvoiceDTO>(invoice);

            return Ok(invoiceDTO);
        }


        [HttpPost]
        public async Task<ActionResult<InvoiceDTO>> SubmitData([FromBody] List<InvoiceDetail> data)
        {
            if (data.Count() < 1)
            {
                return BadRequest();
            }
            Invoice invoice = new Invoice()
            {
                InvoiceDate = System.DateTime.Now,
                InvoiceNo = 2703,
                // InvoiceDetails = null
            };
            // object obj = data.ToObject<object>();
            // Console.WriteLine(data);
            // Console.WriteLine(data[0].id);
            // Console.WriteLine(data.GetType());
            _context.Invoices.Add(invoice);
            for (int i = 0; i < data.Count; i++)
            {
                // var item = _context.Items.Find(data[i].Id);
                var item = _context.Items.FirstOrDefaultAsync(f => f.Id == data[i].ItemId).Result;
                // Console.WriteLine(data[i].id.ToString());
                var inputInvoiceDetail = new InvoiceDetail()
                {
                    ItemId = data[i].ItemId,
                    //Item = item,
                    InvoiceID = invoice.Id,
                    //Invoice = invoice,
                    Price = data[i].Price,
                    Qty = data[i].Qty,
                };

                invoice.addInvoiceDetail(data[i].Qty, item.Id);
            }
            // _context.Invoices.Add(invoice);
            // _context.SaveChanges();
            await _context.SaveChangesAsync();
            InvoiceDTO invoiceDTO = _mapper.Map<Invoice, InvoiceDTO>(invoice);
            // var hasil = "done";
            // var hasil = await _context.InvoiceDetails.CountAsync();
            // var hasil = _context.InvoiceDetails.Include(f => f.Invoice).Include(it => it.Item).FirstOrDefaultAsync(f => f.InvoiceID == invoice.Id);
            // var hasil = _context.Invoices.Include(inv=>inv.InvoiceDetail).Include(it=>it.)
            // var hasil = _context.InvoiceDetails.FirstOrDefaultAsync(f => f.InvoiceID == invoice.Id).Result;
            // var hasil = await _context.Invoices.Include(ivd => ivd.InvoiceDetails).AllAsync;
            // _context.Invoices.Include(f=>f.)
            // var items = new Item()
            // {

            //     Name = data.Name,
            //     Price = data.Price
            // };
            // _context.Items.Add(items);
            // _context.SaveChanges();

            return Ok(invoiceDTO);
        }

        [HttpGet]
        public ActionResult<List<InvoiceDTO>> getInvoice()
        {

            // var invoice = await _context.Invoices.Include(inv => inv.InvoiceDetails).ToListAsync();
            var invoice =  _context.Invoices.Include(inv => inv.InvoiceDetails)
                            .ThenInclude(inv=>inv.Item)
                            .Select(_mapper.Map<Invoice, InvoiceDTO>);
            // InvoiceDTO invoiceDTO = _mapper.Map<Invoice, InvoiceDTO>(invoice);
            // InvoiceDTO invoiceDTO = _mapper.Map<Invoice, InvoiceDTO>(invoice);
            // var invoice = _context.Invoices.Select(_mapper.Map<InvoiceDTO>).ToList();
            // InvoiceDTO invoiceDTO = await _mapper.Map<Invoice, InvoiceDTO>(invoice);
            return Ok(invoice);
            //[HttpPost]
        }


        [HttpGet("{id:int}")]
        public ActionResult getInvoiceById([FromRoute] int id)
        {

            // var invoice = await _context.Invoices.Include(inv => inv.InvoiceDetails).ToListAsync();
            var invoice = _context.Invoices.Include(inv => inv.InvoiceDetails)
                            .ThenInclude(inv => inv.Item)
                            .Select(_mapper.Map<Invoice, InvoiceDTO>).FirstOrDefault(f=>f.Id == id);
            // InvoiceDTO invoiceDTO = _mapper.Map<Invoice, InvoiceDTO>(invoice);
            // InvoiceDTO invoiceDTO = _mapper.Map<Invoice, InvoiceDTO>(invoice);
            // var invoice = _context.Invoices.Select(_mapper.Map<InvoiceDTO>).ToList();
            // InvoiceDTO invoiceDTO = await _mapper.Map<Invoice, InvoiceDTO>(invoice);
            return Ok(invoice);
            //[HttpPost]
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Invoice>> UpdateDataTransaksi([FromRoute] int id, [FromBody] List<InvoiceDetail> data)
        {
            Invoice invoice = _context.Invoices.FirstOrDefault(f => f.Id == id);
            List<InvoiceDetail> transaksiExist = _context.InvoiceDetails.Where(f => f.InvoiceID == id).ToList();

            foreach (var newItem in data)
            {
                //newItem.InvoiceID = id;
                if (transaksiExist.Any(f => f.Id == newItem.Id))
                {
                    InvoiceDetail transaksi = transaksiExist.FirstOrDefault(f => f.Id == newItem.Id);
                    InvoiceDetail NewTransaksi = new InvoiceDetail()
                    {
                        Id = newItem.Id,
                        ItemId = newItem.ItemId,
                        InvoiceID = newItem.InvoiceID,
                        Price = newItem.Price,
                        Qty = newItem.Qty,
                    };
                    _context.Entry<InvoiceDetail>(transaksi).CurrentValues.SetValues(NewTransaksi);
                    transaksiExist.Remove(transaksi);
                }
                else
                {
                    //var addedTransaction = newItem;
                    InvoiceDetail NewTransaksi = new InvoiceDetail()
                    {
                        ItemId = newItem.ItemId,
                        InvoiceID = newItem.InvoiceID,
                        Price = newItem.Price,
                        Qty = newItem.Qty,
                    };
                    _context.InvoiceDetails.Add(NewTransaksi);
                };
            }
            if (transaksiExist.Count() > 0)
            {
                foreach (var deletedTransaksi in transaksiExist)
                {
                    InvoiceDetail targetDeletedTransaksi = new InvoiceDetail()
                    {
                        
                        Id = deletedTransaksi.Id,
                        ItemId = deletedTransaksi.ItemId,
                        InvoiceID = deletedTransaksi.InvoiceID,
                        Price = deletedTransaksi.Price,
                        Qty = deletedTransaksi.Qty,
                    };
                    _context.InvoiceDetails.Remove(targetDeletedTransaksi);
                }
            }
            invoice.InvoiceDate = DateTime.Now;
            //_context.Entry<Invoice>(invoice).CurrentValues.SetValues(data);
            _context.SaveChanges();

            //var hasil = _context.Invoices.Include(inv => inv.InvoiceDetails)
            //               .ThenInclude(inv => inv.Item)
            //               .Select(_mapper.Map<Invoice, InvoiceDTO>).FirstOrDefault(f => f.Id == id);


            // InvoiceDTO invoiceDTO = _mapper.Map<Invoice, InvoiceDTO>(invoice);
            // InvoiceDTO invoiceDTO = _mapper.Map<Invoice, InvoiceDTO>(invoice);
            // var invoice = _context.Invoices.Select(_mapper.Map<InvoiceDTO>).ToList();
            // InvoiceDTO invoiceDTO = await _mapper.Map<Invoice, InvoiceDTO>(invoice);
            return Ok("Berhasil diupdate");
        }

    }

    //public class gettingItem
    //{

    //    public int id { get; set; }
    //    public string name { get; set; }
    //    public double price { get; set; }
    //    public int qty { get; set; }


    //}
}
