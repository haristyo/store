using store.core;
using store.core.Repository;
using store.data.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace store.data
{
    public class TokoUnitOfWork : ITokoUnitOfWork
    {
        protected AppsContext _dbContext;
        public TokoUnitOfWork(AppsContext dbContext)
        {
            _dbContext = dbContext;
        }
        private IItemRepository _item;
        public IItemRepository Item => _item = _item ?? new ItemRepository(_dbContext);
        private IInvoiceRepository _invoice;
        public IInvoiceRepository Invoice => _invoice = _invoice ?? new InvoiceRepository(_dbContext);
        private IInvoiceDetailRepository _invoiceDetail;
        public IInvoiceDetailRepository InvoiceDetail => _invoiceDetail = _invoiceDetail ?? new InvoiceDetailRepository(_dbContext);

        public Task<int> SaveChangeAsync(CancellationToken cancellationToken = default)
        {
           return _dbContext.SaveChangesAsync(cancellationToken);
        }

    }
}
