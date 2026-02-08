using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Application.IRepositories
{
    public interface IGeneralRepository<T> where T : BaseModel
    {
        Task<T?> Get(Expression<Func<T, bool>> expression);
        IQueryable<T> GetAll();
        Task<T?> GetById(Guid id);
        Task<T?> GetWithTrackingById(Guid id);
        Task<T?> GetWithTracking(Expression<Func<T, bool>> predicate);
        bool Add(T entity);
        void Update(T entity);
        void SoftDelete(T entity);
        void HardDelete(Guid id);
        Task SaveChanges();
        bool Exists(Guid id);
    }
}