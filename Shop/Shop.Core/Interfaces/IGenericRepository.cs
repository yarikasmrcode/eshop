using Shop.Core.Entities;
using Shop.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Core.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetEntityWithSpecification(ISpecification<T> specification);
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> specification);
        Task<int> CountAsync(ISpecification<T> spec);

        //we only track those, not add to db. uow adds to db, not repo
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
