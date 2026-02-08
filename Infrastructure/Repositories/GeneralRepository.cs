using Application.IRepositories;
using Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class GeneralRepository<T> : IGeneralRepository<T> where T : BaseModel
    {
        private readonly Context _context;
        private readonly DbSet<T> _dbset;

        public GeneralRepository(Context context)
        {
            _context = context;
            _dbset = _context.Set<T>();
        }

        public bool Add(T entity)
        {
            _dbset.Add(entity);
            return true;
        }

        public void SoftDelete(T entity)
        {
            entity.IsDeleted = true;
            Update(entity);
        }

        public async Task<T?> Get(Expression<Func<T, bool>> expression)
        {
            return await GetAll().Where(expression).FirstOrDefaultAsync();
        }

        public IQueryable<T> GetAll()
        {
            return _dbset.Where(x => !x.IsDeleted);
        }

        public async Task<T?> GetById(Guid id)
        {
            return await _dbset.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public Task<T?> GetWithTrackingById(Guid id)
        {
            return GetWithTracking(x => x.Id == id && !x.IsDeleted);
        }

        public Task<T?> GetWithTracking(Expression<Func<T, bool>> predicate)
        {
            return _dbset
                .Where(predicate)
                .AsTracking()
                .FirstOrDefaultAsync();
        }


        public void HardDelete(Guid id)
        {
            _dbset
                .Where(x => x.Id == id)
                .ExecuteDelete();
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            _dbset.Update(entity);
        }

        public bool Exists(Guid id)
        {
            return _dbset.Any(x => x.Id == id);
        }

        public async Task<IEnumerable<TResult>> GetFilter<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector)
        {
            return await _context.Set<T>()
                .Where(predicate)
                .Select(selector)
                .ToListAsync();
        }
    }
}