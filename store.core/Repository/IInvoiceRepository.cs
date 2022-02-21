using Ardalis.Specification;
using store.core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace store.core.Repository
{
    public interface IInvoiceRepository
    {
        Task Insert(Invoice model, CancellationToken cancellationToken = default);
        Task Update(Invoice model, int id, CancellationToken cancellationToken = default);
        Task Delete(int id, CancellationToken cancellationToken = default);
        Task<Invoice> getSingle(int id, CancellationToken cancellationToken = default);
        Task<List<Invoice>> GetList(Specification<Invoice> filter, CancellationToken cancellationToken = default);
    }
    public interface IItemRepository
    {
        Task Insert(Item model, CancellationToken cancellationToken = default);
        Task Update(Item model, int id, CancellationToken cancellationToken = default);
        Task Delete(int id, CancellationToken cancellationToken = default);
        Task<Item> getSingle(int id, CancellationToken cancellationToken = default);
        Task<List<Item>> GetList(Specification<Item> filter, CancellationToken cancellationToken = default);
    }
    public interface IInvoiceDetailRepository
    {
        Task Insert(InvoiceDetail model, CancellationToken cancellationToken = default);
        Task Update(InvoiceDetail model, int id, CancellationToken cancellationToken = default);
        Task Delete(int id, CancellationToken cancellationToken = default);
        Task<InvoiceDetail> getSingle(int id, CancellationToken cancellationToken = default);
        Task<List<InvoiceDetail>> GetList(Specification<InvoiceDetail> filter, CancellationToken cancellationToken = default);
    }
}
