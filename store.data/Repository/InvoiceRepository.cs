using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using store.core.Entities;
using store.core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace store.data.Repository
{
    public class InvoiceRepository : IInvoiceRepository
    {
        protected readonly DbSet<Invoice> _dbSetInvoice;
        protected readonly DbContext _dbContext;
        public InvoiceRepository(DbContext context)
        {
            _dbContext = context;
            _dbSetInvoice = _dbContext.Set<Invoice>();
        }
        public int TanggalkeInt(DateTime dateDate)
        {
            return Convert.ToInt32(dateDate.Year * 60 * 60 * 24 * 31 * 12 + dateDate.Month * 60 * 60 * 24 * 31 + dateDate.Day * 60 * 60 * 24 + dateDate.Hour * 60 * 60 + dateDate.Minute * 60 + dateDate.Second);
        }
        public async Task Insert(Invoice model, CancellationToken cancellationToken = default)
        {
            if (model.InvoiceDate == null)
            {
                model.InvoiceDate = DateTime.Now;
            }

            if(model.InvoiceNo == null)
            {
                model.InvoiceNo = TanggalkeInt(DateTime.Now);
            }
            await _dbSetInvoice.AddAsync(model, cancellationToken);
        }

        public async Task Update(Invoice model, int id, CancellationToken cancellationToken = default)
        {
            //throw new NotImplementedException();
            Invoice existingInvoice = await _dbSetInvoice.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            //await 
            if(existingInvoice == null)
            {
                throw new Exception($"Model Invoice dengan id = {id} tidak ditemukan");
            }
            _dbContext.Entry(existingInvoice).CurrentValues.SetValues(model);
        }
        public async Task Delete(int id, CancellationToken cancellationToken = default)
        {
            //throw new NotImplementedException();
            Invoice existingInvoice = await _dbSetInvoice.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (existingInvoice == null)
            {
                throw new Exception($"Model Invoice dengan id = {id} tidak ditemukan");
            }
            _dbContext.Remove(existingInvoice);
        }

        public async Task<List<Invoice>> GetList(Specification<Invoice> specification, CancellationToken cancellationToken = default)
        {
            IQueryable<Invoice> query = _dbSetInvoice.AsQueryable();
            SpecificationEvaluator evaluator = new SpecificationEvaluator();
            query = evaluator.GetQuery(query, specification);
            //throw new NotImplementedException();
            return query.Include(f=>f.InvoiceDetails).ThenInclude(e=>e.Item).ToListAsync(cancellationToken).Result;
        }

        public async Task<Invoice> getSingle(int id, CancellationToken cancellationToken = default)
        {
            //throw new NotImplementedException();
            //Invoice existingInvoice = 
                return await _dbSetInvoice.Include(f => f.InvoiceDetails).ThenInclude(e => e.Item).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

    }
    public class ItemRepository : IItemRepository
    {
        protected readonly DbSet<Item> _dbSetItem;
        protected readonly DbContext _dbContext;
        public ItemRepository(DbContext context)
        {
            _dbContext = context;
            _dbSetItem = _dbContext.Set<Item>();
        }

        public async Task Insert(Item model, CancellationToken cancellationToken = default)
        {
            await _dbSetItem.AddAsync(model, cancellationToken);
        }

        public async Task Update(Item model, int id, CancellationToken cancellationToken = default)
        {
            //throw new NotImplementedException();
            Item existingItem = await _dbSetItem.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            //await 
            if (existingItem == null)
            {
                throw new Exception($"Model Item dengan id = {id} tidak ditemukan");
            }
            _dbContext.Entry(existingItem).CurrentValues.SetValues(model);
        }
        public async Task Delete(int id, CancellationToken cancellationToken = default)
        {
            //throw new NotImplementedException();
            Item existingItem = await _dbSetItem.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (existingItem == null)
            {
                throw new Exception($"Model Invoice dengan id = {id} tidak ditemukan");
            }
            _dbContext.Remove(existingItem);
        }

        public async Task<List<Item>> GetList(Specification<Item> specification, CancellationToken cancellationToken = default)
        {
            IQueryable<Item> query = _dbSetItem.AsQueryable();
            SpecificationEvaluator evaluator = new SpecificationEvaluator();
            query = evaluator.GetQuery(query, specification);
            //throw new NotImplementedException();
            return await query.ToListAsync(cancellationToken);
        }

        public async Task<Item> getSingle(int id, CancellationToken cancellationToken = default)
        {
            //throw new NotImplementedException();
            //Invoice existingInvoice = 
            return await _dbSetItem.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
    }
    public class InvoiceDetailRepository : IInvoiceDetailRepository
    {
        protected readonly DbSet<InvoiceDetail> _dbSetInvoiceDetail;
        protected readonly DbContext _dbContext;
        public InvoiceDetailRepository(DbContext context)
        {
            _dbContext = context;
            _dbSetInvoiceDetail = _dbContext.Set<InvoiceDetail>();
        }

        public async Task Insert(InvoiceDetail model, CancellationToken cancellationToken = default)
        {
            await _dbSetInvoiceDetail.AddAsync(model, cancellationToken);
        }

        public async Task Update(InvoiceDetail model, int id, CancellationToken cancellationToken = default)
        {
            //throw new NotImplementedException();
            InvoiceDetail existingInvoiceDetail = await _dbSetInvoiceDetail.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            //await 
            if (existingInvoiceDetail == null)
            {
                throw new Exception($"Model Item dengan id = {id} tidak ditemukan");
            }
            _dbContext.Entry(existingInvoiceDetail).CurrentValues.SetValues(model);
        }
        public async Task Delete(int id, CancellationToken cancellationToken = default)
        {
            //throw new NotImplementedException();
            InvoiceDetail existingInvoiceDetail = await _dbSetInvoiceDetail.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (existingInvoiceDetail == null)
            {
                throw new Exception($"Model Invoice dengan id = {id} tidak ditemukan");
            }
            _dbContext.Remove(existingInvoiceDetail);
        }

        public async Task<List<InvoiceDetail>> GetList(Specification<InvoiceDetail> specification, CancellationToken cancellationToken = default)
        {
            IQueryable<InvoiceDetail> query = _dbSetInvoiceDetail.AsQueryable();
            SpecificationEvaluator evaluator = new SpecificationEvaluator();
            query = evaluator.GetQuery(query, specification);
            //throw new NotImplementedException();
            return query.Include(f=>f.Item).ToListAsync(cancellationToken).Result;
        }

        public async Task<InvoiceDetail> getSingle(int id, CancellationToken cancellationToken = default)
        {
            //throw new NotImplementedException();
            //Invoice existingInvoice = 
            return await _dbSetInvoiceDetail.Include(f => f.Item).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
    }
}
