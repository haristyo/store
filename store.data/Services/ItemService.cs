﻿using store.core;
using store.core.Entities;
using store.core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace store.data.Services
{
    public abstract class Service<T> : IService where T : class
    {
        private Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>> ();
        public Dictionary<string, List<string>> GetError()
        {
            return _errors;
        }

        public bool GetServiceState()
        {
            return _errors.Count == 0;
        }
        protected void ClearError()
        {
            _errors.Clear();
        }
        protected void AddError(string key, string error)
        {
            if (_errors.ContainsKey(key)==false)
            {
                //jika tidak ada dictionary, buat dulu
                _errors.Add(key, new List<string>());
            }
            //jika dictionary sudah ada
            _errors[key].Add(error);

            //cara get list berdasarkan key
            //List<string> x = _errors[key];
        }
        protected void AddError(string error)
        {
            if (_errors.ContainsKey("_ERROR_") == false)
            {
                //jika tidak ada dictionary, buat dulu
                _errors.Add("_ERROR_", new List<string>());
            }
            //jika dictionary sudah ada
            _errors["_ERROR_"].Add(error);
        }
        protected abstract bool ProcessData(T target);
        protected abstract T BindToObject(Dictionary<string,object> map);
        public void SyncData()
        {
            Dictionary<string, object> hasilbacafile = new Dictionary<string, object>();
            T data = BindToObject(hasilbacafile);
            ProcessData(data);
        }

    }
    public class ItemService : Service<Item>, IItemService
    {
        protected ITokoUnitOfWork _tokoUnitOfWork;
        public ItemService(ITokoUnitOfWork tokoUnitOfWork)
        {
            _tokoUnitOfWork = tokoUnitOfWork;
        }
        public async Task<bool> Delete(int id, CancellationToken cancellationToken = default)
        {
            //throw new NotImplementedException();
            Item existingItemId = _tokoUnitOfWork.Item.getSingle(id).Result;
            if (existingItemId.Id != id)
            {
                return false;
            }
            await _tokoUnitOfWork.Item.Delete(id, cancellationToken);
            await _tokoUnitOfWork.SaveChangeAsync(cancellationToken);
            return true;
        }

        public async Task<List<Item>> GetList(CancellationToken cancellationToken = default)
        {
            //throw new NotImplementedException();
            return await _tokoUnitOfWork.Item.GetList(cancellationToken);   
        }

        public async Task<Item> getSingle(int id, CancellationToken cancellationToken = default)
        {
            Item existingItem = await _tokoUnitOfWork.Item.getSingle(id,cancellationToken);
            return existingItem ??= null;
        }

        public async Task<Item> Insert(Item item, CancellationToken cancellationToken = default)
        {
            if (Validate(item) == false)
            {
                return null;
            }
            await _tokoUnitOfWork.Item.Insert(item, cancellationToken);
            await _tokoUnitOfWork.SaveChangeAsync(cancellationToken);
            return item;
        }

        public async Task<Item> Update(Item item, int id, CancellationToken cancellationToken = default)
        {
            //throw new NotImplementedException();
            if (Validate(item) == false )
            {
                return null;
            }
            int existingItemId = _tokoUnitOfWork.Item.getSingle(id).Result.Id;
            if(existingItemId != id)
            {
                return null;
            }

            //await _tokoUnitOfWork.Item.Insert(item, cancellationToken);
            await _tokoUnitOfWork.Item.Update(item, id, cancellationToken);
            await _tokoUnitOfWork.SaveChangeAsync(cancellationToken);
            return item;
        }

        public bool Validate(Item item)
        {
            if (string.IsNullOrEmpty(item.Name))
            {
                AddError("Nama","Nama Harus Diisi.");
            }
            if (string.IsNullOrEmpty(item.Code))
            {
                AddError("Code","Nama Harus Diisi.");
            }
            if(0 >= item.Price){
                AddError("Harga", "Harga harus Lebih Besar daripada 0");
            }
            return GetServiceState();
        }
        protected override Item BindToObject(Dictionary<string, object> map)
        {
            Item item = new Item();
            item.Id = Convert.ToInt32(map["id"]);
            return item;
        }

        protected override bool ProcessData(Item item)
        {
            return true;
        }
    }


    public class InvoiceService : Service<Invoice>, IInvoiceService
    {
        protected ITokoUnitOfWork _tokoUnitOfWork;
        public InvoiceService(ITokoUnitOfWork tokoUnitOfWork)
        {
            _tokoUnitOfWork = tokoUnitOfWork;
        }
        public async Task<bool> Delete(int id, CancellationToken cancellationToken = default)
        {
            //throw new NotImplementedException();
            //Invoice existingInvoiceId = _tokoUnitOfWork.Invoice.getSingle(id).Result;
            //if (existingInvoiceId.Id != id)
            //{
            //    return false;
            //}
            //await _tokoUnitOfWork.Invoice.Delete(id, cancellationToken);
            //await _tokoUnitOfWork.SaveChangeAsync(cancellationToken);
            //return true;




            Invoice invoiceExist = await _tokoUnitOfWork.Invoice.getSingle(id);
            List<InvoiceDetail> transaksiExist = invoiceExist.InvoiceDetails.ToList();
            //List<InvoiceDetail> transaksi= invoiceExist.InvoiceDetails;
            //IQueryable<Invoice> queryableInvoiceExist = invoiceExist.
            //List<InvoiceDetail> x = await _tokoUnitOfWork.InvoiceDetail;
            //List<InvoiceDetail> transaksiExist = invoiceExist.InvoiceDetails[0];

            if (ValidateUpdate(invoiceExist, id) == false)
            {
                return false;
            }

            if (transaksiExist.Count() > 0)
            {
                foreach (var deletedTransaksi in transaksiExist)
                {
                    await _tokoUnitOfWork.InvoiceDetail.Delete(deletedTransaksi.Id, cancellationToken);
                }
            }
            await _tokoUnitOfWork.Invoice.Delete(id, cancellationToken);
            await _tokoUnitOfWork.SaveChangeAsync(cancellationToken);
            return true;
        }

        public async Task<List<Invoice>> GetList(CancellationToken cancellationToken = default)
        {
            //throw new NotImplementedException();
            List<Invoice> existingInvoiceList = await _tokoUnitOfWork.Invoice.GetList(cancellationToken);
            return existingInvoiceList ??= null;
        }

        public async Task<Invoice> getSingle(int id, CancellationToken cancellationToken = default)
        {
            Invoice existingInvoice = await _tokoUnitOfWork.Invoice.getSingle(id, cancellationToken);
            return existingInvoice ??= null;
        }

        public async Task<Invoice> Insert(Invoice invoice, CancellationToken cancellationToken = default)
        {
            if (ValidateInsert(invoice) == false)
            {
                return null;
            }
            await _tokoUnitOfWork.Invoice.Insert(invoice, cancellationToken);
            //foreach (InvoiceDetail newitem in invoice.InvoiceDetails)
            //{
            //    await _tokoUnitOfWork.InvoiceDetail.Insert(newitem, cancellationToken);
            //}
            //for(int i = 0; i< invoice.InvoiceDetails.Count; i++)
            //{
            //    await _tokoUnitOfWork.InvoiceDetail.Insert(invoice.InvoiceDetails[i], cancellationToken);
            //}


            await _tokoUnitOfWork.SaveChangeAsync(cancellationToken);
            return invoice;
        }

        public async Task<Invoice> Update(Invoice invoice, int id, CancellationToken cancellationToken = default)
        {


            
            Invoice invoiceExist = await _tokoUnitOfWork.Invoice.getSingle(id);
            List<InvoiceDetail> transaksiExist = invoiceExist.InvoiceDetails.ToList();
            //List<InvoiceDetail> transaksi= invoiceExist.InvoiceDetails;
            //IQueryable<Invoice> queryableInvoiceExist = invoiceExist.
            //List<InvoiceDetail> x = await _tokoUnitOfWork.InvoiceDetail;
            //List<InvoiceDetail> transaksiExist = invoiceExist.InvoiceDetails[0];


            foreach (var newItem in invoice.InvoiceDetails)
            {
                //newItem.InvoiceID = 1;
                if (transaksiExist.Any(f => f.Id == newItem.Id))
                {
                    InvoiceDetail transaksi = transaksiExist.First(f => f.Id == newItem.Id);
                    await _tokoUnitOfWork.InvoiceDetail.Update(newItem, newItem.Id);
                    transaksiExist.Remove(transaksi);
                }
                else
                {
                    await _tokoUnitOfWork.InvoiceDetail.Insert(newItem);
                };
            }
            if (transaksiExist.Count() > 0)
            {
                foreach (var deletedTransaksi in transaksiExist)
                {
                    await _tokoUnitOfWork.InvoiceDetail.Delete(deletedTransaksi.Id);
                }
            }
            await _tokoUnitOfWork.Invoice.Update(invoice, id, cancellationToken);
            await _tokoUnitOfWork.SaveChangeAsync(cancellationToken);
            return invoice;
        }

        public bool ValidateBase(Invoice invoice)
        {
            if(invoice == null)
            {
                AddError("Object Invoice Tidak boleh kosong");
            }
            if (invoice.InvoiceNo <= 0)
            {
                AddError("InvoiceNo", "Nomor Invoice Harus Diisi.");
            }
            if (string.IsNullOrEmpty( Convert.ToString (invoice.InvoiceDate)))
            {
                AddError("Code", "Tanggal Harus Diisi.");
            }
            if(invoice.InvoiceDetails.Count <= 0)
            {
                AddError("InvoiceDetails", "Harus memiliki invoice detail");
            }
            return GetServiceState();
        }
        public bool ValidateInsert(Invoice invoice)
        {
            if (ValidateBase(invoice)==false)
            {
                return GetServiceState();
            }
            Invoice existInvoice = _tokoUnitOfWork.Invoice.getSingle(invoice.Id).Result;
            if (existInvoice!=null)
            {
                AddError("InvoiceNo", "Invoice No Tidak Boleh sama");
            }
            return GetServiceState();
        }
        public bool ValidateUpdate(Invoice invoice,int id)
        {
            if (ValidateBase(invoice) == false)
            {
                return GetServiceState();
            }
            if(invoice.Id != id)
            {
                AddError("Id", "Id tidak boleh diganti");
            }
            if(id==0 || invoice.Id == 0)
            {
                AddError("Id", "Id harus diisi");
            }


            return GetServiceState();
        }

        protected override Invoice BindToObject(Dictionary<string, object> map)
        {
            Invoice invoice = new Invoice();
            invoice.Id = Convert.ToInt32(map["id"]);
            return invoice;
        }

        protected override bool ProcessData(Invoice invoice)
        {
            return true;
        }
    }



}
