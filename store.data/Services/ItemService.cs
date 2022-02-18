using store.core;
using store.core.Entities;
using store.core.Services;
using System;
using System.Collections.Generic;
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
        public Task Delete(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
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

        public Task<Item> Update(Invoice model, int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
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
}
