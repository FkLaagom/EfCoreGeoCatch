using Geocaching.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocaching.Database
{
    public static class LoadDatabase
    {
        private static List<Person> _persons;
        private static List<Geocashe> _geocashes;
        private static List<FoundGeocache> _foundGeocashes;
        private static List<KeyValuePair<Person, List<int>>> _foundGeocacheIDs;


        static LoadDatabase()
        {
            _persons = new List<Person>();
            _geocashes = new List<Geocashe>();
            _foundGeocashes = new List<FoundGeocache>();
            _foundGeocacheIDs = new List<KeyValuePair<Person, List<int>>>();
        }

        public static void FromFlatFile(string path)
        {
            //var emptyDatabase = EmptyDatabase();
            // emptyDatabase.Wait();
            //Task.WaitAll(emptyDatabase);
            

            var lines = File.ReadAllLines(path);
            LineToPersson(lines[0]);
            for (int i = 1; i < lines.Length; i++)
            {
                if (lines[i] == "")
                {
                    i++;
                    LineToPersson(lines[i]);
                }
                else if (i + 1 >= lines.Length || lines[i + 1] == "")
                {
                    LineToFoundGeocache(lines[i]);
                }
                else
                {
                    LineToGeoCashes(lines[i]);
                }
            }

            for (int i = 0; i < _foundGeocacheIDs.Count; i++)
            {
                foreach (var id in _foundGeocacheIDs[i].Value)
                {
                    var foundGeocashe = new FoundGeocache
                    {
                        Person = _persons[i],
                        Geocashe = _geocashes[id-1]
                    };
                    _foundGeocashes.Add(foundGeocashe);
                }
            }

            PupulateDatabase();
        }

#warning makeAsync
        private static void PupulateDatabase()
        {
            using (var context = new AppDbContext())
            {
                context.Persons.AddRange(_persons);
                context.Geocashes.AddRange(_geocashes);
                context.FoundGeocaches.AddRange(_foundGeocashes);
                context.SaveChanges();
            }
        }
#warning Remove -> Use Async Below
        private static void EmptyDatabaseSync()
        {
            using(var context = new AppDbContext())
            {
                var foundGeocaches = context.FoundGeocaches.ToList();
                var geocaches = context.Geocashes.ToList();
                var persons = context.Persons.ToList();
                context.FoundGeocaches.RemoveRange(foundGeocaches);
                context.Geocashes.RemoveRange(geocaches);
                context.Persons.RemoveRange(persons);
                context.SaveChanges();
            }
        }

        private static async Task EmptyDatabase()
        {
            using (var context = new AppDbContext())
            {
                //var foundGeocaches = await context.FoundGeocaches.ToListAsync();
                //var geocaches = await context.Geocashes.ToListAsync();
                //var persons = await context.Persons.ToListAsync();

                var foundGeocaches = context.FoundGeocaches.ToList();
                var geocaches = context.Geocashes.ToList();
                var persons = context.Persons.ToList();
                context.FoundGeocaches.RemoveRange(foundGeocaches);
                context.Geocashes.RemoveRange(geocaches);
                context.Persons.RemoveRange(persons);
                await context.SaveChangesAsync();
            }
        }

        private static void LineToGeoCashes(string line)
        {
            var parms = line.Split('|').Select(x => x.Trim()).ToArray();
            var geocashe = new Geocashe
            {
                Person = _persons.LastOrDefault(),
                Longitude = double.Parse(parms[1]),
                Latitude = double.Parse(parms[2]),
                Content = parms[3],
                Message = parms[4],
            };
            _geocashes.Add(geocashe);
        }

        private static void LineToFoundGeocache(string line)
        {
            var ids = line.Substring(6).Split(',').Select(x => x.Trim()).ToArray();
            var foundIds = new List<int>();
            var person = _persons.LastOrDefault();
            foreach (var id in ids)
            {
                foundIds.Add(int.Parse(id));
            }
             _foundGeocacheIDs.Add(new KeyValuePair<Person,List<int>>(person,foundIds));
        }

        private static void LineToPersson(string line)
        {
            var parms = line.Split('|').Select(x => x.Trim()).ToArray();
            var person = new Person()
            {

                FirstName = parms[0],
                LastName = parms[1],
                Country = parms[2],
                City = parms[3],
                StreetName = parms[4],
                StreetNumber = Convert.ToByte(parms[5]),
                Latitude = Convert.ToDouble(parms[6]),
                Longitude = Convert.ToDouble(parms[7])
            };
            _persons.Add(person);
        }
    }
}
