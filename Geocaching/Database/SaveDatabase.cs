using Geocaching.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Geocaching.Database
{
    public static class SaveDatabase
    {
        public static async Task ToFlatFile(string path)
        {
            var linesToWrite = new List<string>();
            var geocashes = new List<Geocashe>();
            List<Person> persons;
            using (var context = new AppDbContext())
                persons = await context.Persons.Include(x => x.Geocashes).Include(x => x.FoundGeocaches).ToListAsync();
            persons.ForEach(x => x.Geocashes.ToList().ForEach(g => geocashes.Add(g)));
            persons.ForEach(p =>
            {
                if (linesToWrite.Any())
                    linesToWrite.Add("");
                linesToWrite.Add($"{p.FirstName} | {p.LastName} | {p.Country} | {p.City} | {p.StreetName} | {p.StreetNumber} | {p.Location.Latitude} | {p.Location.Longitude}");
                p.Geocashes.ToList().ForEach(g => linesToWrite.Add($"{geocashes.IndexOf(g) + 1} | {g.Location.Latitude} | {g.Location.Longitude} | {g.Content} | {g.Message}"));
                linesToWrite.Add($"Found: {string.Join(", ", p.FoundGeocaches.Select(x => geocashes.IndexOf(x.Geocashe)).Select(X => (X + 1).ToString()).ToArray())}");
            });
            File.WriteAllText(path, string.Join("\r\n", linesToWrite));
        }
    }
}