using Geocaching.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocaching.Database
{
    interface ICrud
    {
        string ConnectionString { get; set; }
        Task AddPersonAsync(Person person);
        Task AddFoundGeoCacheAsync(FoundGeocache geo);
        Task AddGeoCasheAsync(Geocashe geo);
        Task RemoveFoundGeoCacheAsync(FoundGeocache geo);
    }
}
