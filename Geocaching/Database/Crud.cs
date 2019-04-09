using Geocaching.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocaching.Database
{
    class Crud<T> : ICrud<T> where T : class, IEntity, new()
    {
        private AppDbContext _context = new AppDbContext();

        public async Task CreateAsync(T entity)
        {
            _context.Set<T>().Add(entity);
            await SaveChangesAsync();
        }

        public async Task<List<T>> GetListAsync(bool eagerLoad = false)
        {
            var query = _context.Set<T>().AsQueryable();
            if (eagerLoad)
            {
                foreach (var property in _context.Model.FindEntityType(typeof(T)).GetNavigations())
                    query = query.Include(property.Name);
            }
            return await query.ToListAsync();
        }
        
        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await SaveChangesAsync();
        }

        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
