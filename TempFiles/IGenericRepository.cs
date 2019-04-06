using Geocaching.Entitys;
using Geocaching.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Geocaching
{
    public interface IGenericRepository<T>
    where T : IEntity
    {
        Task AddAsync(T entity);
        Task AddRangeAsnc(IEnumerable<T> entitys);
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(IEnumerable<T> entitys);
        Task UpdateAsync(T entity);
        Task UpdateRangeAsync(IEnumerable<T> entitys);
        Task<List<T>> GetAsync(string[] include = null);
        Task<bool> SaveChangesAsync();
    }
}
