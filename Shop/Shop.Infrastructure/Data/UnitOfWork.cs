using Shop.Api.Data;
using Shop.Core.Entities;
using Shop.Core.Interfaces;
using Shop.Infrastructure.Data.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _storeContext;
        private Hashtable _repositories;
        public UnitOfWork(StoreContext context)
        {
            _storeContext = context;
        }
        public async Task<int> Complete()
        {
            return await _storeContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _storeContext.Dispose();
        }

        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
            if (_repositories == null)
            {
                _repositories = new Hashtable();
            }

            var type = typeof(T).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType
                    (typeof(T)), _storeContext);

                _repositories.Add(type, repositoryInstance);
            }

            return (IGenericRepository<T>) _repositories[type];
        }
    }
}
