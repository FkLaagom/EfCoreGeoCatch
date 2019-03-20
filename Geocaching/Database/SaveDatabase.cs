using Geocaching.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Geocaching.Database
{
    public static class SaveDatabase
    {
        public static void ToFlatFile(string path)
        {
            var linesToWrite = new List<string>();
            var geocashes = new List<Geocashe>();
            List<Person> persons;
            using (var context = new AppDbContext())
                persons = context.Persons.Include(x => x.Geocashes).Include(x => x.FoundGeocaches).ToList();
            persons.ForEach(x => x.Geocashes.ToList().ForEach(g => geocashes.Add(g)));
            persons.ForEach(p => {
                linesToWrite.Add($"{p.FirstName} | {p.LastName} | {p.Country} | {p.City} | {p.StreetName} | {p.StreetNumber} | {p.Latitude} | {p.Longitude}");
                p.Geocashes.ToList().ForEach(g => linesToWrite.Add($"{geocashes.IndexOf(g) + 1} | {g.Latitude} | {g.Longitude} | {g.Content} | {g.Message}"));
                linesToWrite.Add($"Found: {string.Join(", ", p.FoundGeocaches.Select(x => geocashes.IndexOf(x.Geocashe)).Select(X => (X + 1).ToString()).ToArray())}");
                linesToWrite.Add("");
            });
            File.WriteAllLines(path,linesToWrite);
        }
    }

}
