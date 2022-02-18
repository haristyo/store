using store.core.Repository;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace store.core
{
    public interface ITokoUnitOfWork
    {
        IItemRepository Item { get; }
        IInvoiceRepository Invoice { get; }
        IInvoiceDetailRepository InvoiceDetail { get; }
        Task<int> SaveChangeAsync(CancellationToken cancellationToken = default);

    }
}
