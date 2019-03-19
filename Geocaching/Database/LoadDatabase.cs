using Geocaching.Models;
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
        // KeyValuePair<int == _idCounter.value, Geocashe>
        private static List<KeyValuePair<int, Geocashe>> _geocashes;
        private static List<FoundGeocache> _foundGeocaches;
        private static int _idCounter;

        static LoadDatabase()
        {
            _persons = new List<Person>();
            _geocashes = new List<KeyValuePair<int, Geocashe>>();
            _foundGeocaches = new List<FoundGeocache>();
            _idCounter = 1;
        }

        public static void FromFlatFile(string path)
        {
            EmptyDatabase();
            var lines = File.ReadAllLines(path);
            LineToPersson(lines[0]);
            for (int i = 1; i < lines.Length; i++)
            {
                if (lines[i] == "")
                {
                    _idCounter++;
                    i++;
                    LineToPersson(lines[i]);
                }
                else if (i+1 >= lines.Length || lines[i+1] == "")
                {
                    LineToFoundGeocache(lines[i]);
                }
                else
                {
                    LineToGeoCashes(lines[i]);
                }
            }
            PupulateDatabase();
        }

#warning MakeAsync
        private static void PupulateDatabase()
        {
            using(var context = new AppDbContext())
            {
                context.Persons.AddRange(_persons);
                context.SaveChanges();

                var persons = context.Persons.ToList();
                var geocashesToDb = new List<Geocashe>();
                foreach (var item in _geocashes)
                {
                    int id = item.Key;
                    var geocasche = item.Value;
                    geocasche.Person = persons.FirstOrDefault(x => x.ID == id);
                    geocashesToDb.Add(geocasche);
                }
                context.Geocashes.AddRange(geocashesToDb);
                context.SaveChanges();

                var geocatches = context.Geocashes.ToList();
                var foundGeocashesToDb = new List<FoundGeocache>();
                foreach (var item in _foundGeocaches)
                {
                    var foundGeocashe = new FoundGeocache()
                    {
                        Geocashe = geocatches.FirstOrDefault(x => x.ID == item.GeocasheID),
                        Person = persons.FirstOrDefault( x => x.ID == item.PersonID)
                    };
                    foundGeocashesToDb.Add(foundGeocashe);
                }

                foreach (var item in foundGeocashesToDb)
                {
                    context.FoundGeocaches.Add(item);
                    context.SaveChanges();
                }

                //foreach (var item in _foundGeocaches)
                //{
                //    context.FoundGeocaches.Add(item);
                //    context.SaveChanges();
                //}

                //context.FoundGeocaches.AddRange(_foundGeocaches);
                //context.FoundGeocaches.AddRange(foundGeocashesToDb);
                //context.SaveChanges();
            }
        }
#warning MakeAsync
        private static void EmptyDatabase()
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

        private static void LineToGeoCashes(string line)
        {
            var parms = line.Split('|').Select(x => x.Trim()).ToArray();
            var geocashe = new Geocashe
            {
                Longitude = double.Parse(parms[1]),
                Latitude = double.Parse(parms[2]),
                Content = parms[3],
                Message = parms[4],
            };
            _geocashes.Add(new KeyValuePair<int, Geocashe>(_idCounter, geocashe));
        }

        private static void LineToFoundGeocache(string line)
        {
            var ids = line.Substring(6).Split(',').Select(x => x.Trim()).ToArray();
            foreach (var id in ids)
            {
                var foundGeoache = new FoundGeocache
                {
                    PersonID = _idCounter,
                    GeocasheID = int.Parse(id)
                };
                _foundGeocaches.Add(foundGeoache);
            }
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
