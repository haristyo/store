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
        Task<Item> Update(Invoice model, int id, CancellationToken cancellationToken = default);
        Task Delete(int id, CancellationToken cancellationToken = default);


    }
}
