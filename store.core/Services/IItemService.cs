using store.core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace store.core.Services
{
    public interface IService
    {
        Dictionary<string, List<string>> GetError();
        Boolean GetServiceState();
    }
    public interface IItemService : IService
    {
        Task<Item> Insert(Item item,CancellationToken cancellationToken=default);
        Task<Item> Update(Item item, int id, CancellationToken cancellationToken = default);
        Task<bool> Delete(int id, CancellationToken cancellationToken = default);
        Task<Item> getSingle(int id, CancellationToken cancellationToken = default);
        Task<List<Item>> GetList(CancellationToken cancellationToken = default);
    }
    public interface IInvoiceService : IService
    {
        Task<Invoice> Insert(Invoice invoice, CancellationToken cancellationToken = default);
        Task<Invoice> Update(Invoice invoice, int id, CancellationToken cancellationToken = default);
        Task<bool> Delete(int id, CancellationToken cancellationToken = default);
        Task<Invoice> getSingle(int id, CancellationToken cancellationToken = default);
        Task<List<Invoice>> GetList(CancellationToken cancellationToken = default);
    }
    //public interface IInvoiceDetailService : IService
    //{
    //    Task<InvoiceDetail> Insert(InvoiceDetail invoiceDetail, CancellationToken cancellationToken = default);
    //    Task<InvoiceDetail> Update(InvoiceDetail invoiceDetail, int id, CancellationToken cancellationToken = default);
    //    Task<bool> Delete(int id, CancellationToken cancellationToken = default);
    //    Task<InvoiceDetail> getSingle(int id, CancellationToken cancellationToken = default);
    //    Task<List<InvoiceDetail>> GetList(CancellationToken cancellationToken = default);
    //}
}
