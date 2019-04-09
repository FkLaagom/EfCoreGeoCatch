using Geocaching.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocaching.Database
{
    public interface ICrud<T>
    {
            Task CreateAsync(T entity);
            Task DeleteAsync(T entity);
            Task<List<T>> GetListAsync(bool eager);
            Task SaveChangesAsync();
    }
}
