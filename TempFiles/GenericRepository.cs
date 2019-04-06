using Geocaching.Entitys;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocaching
{
    public class GenericRepository<T> : IGenericRepository<T>
        where T : class, IEntity

    {
        private AppDbContext _context;
        private DbSet<T> _entities;

        public GenericRepository()
        {
            _context = new AppDbContext();
            _entities = _context.Set<T>();
        }

        public Task AddAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task AddRangeAsnc(IEnumerable<T> entitys)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteRangeAsync(IEnumerable<T> entitys)
        {
            throw new NotImplementedException();
        }

        public async Task<List<T>> GetAsync(string[] include = null)
        {
            if (include == null) return await _entities.ToListAsync();
            else return await include.Aggregate(_entities.AsQueryable(), (query, path) => query.Include(path)).ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync()) > 0;
        }

        public Task UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRangeAsync(IEnumerable<T> entitys)
        {
            throw new NotImplementedException();
        }
    }
}
