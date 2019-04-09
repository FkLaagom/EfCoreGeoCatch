using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocaching.Models
{
    public class Person
    {
        [Key]
        public int ID { get; set; }
        public Location Location { get; set; }
        [MaxLength(50)]
        [Required]
        public string FirstName { get; set; }
        [MaxLength(50)]
        [Required]
        public string LastName { get; set; }
        [MaxLength(50)]
        [Required]
        public string Country { get; set; }
        [MaxLength(50)]
        [Required]
        public string City { get; set; }
        [MaxLength(50)]
        [Required]
        public string StreetName { get; set; }
        public byte StreetNumber { get; set; }

        public ICollection<Geocashe> Geocashes { get; set; }
        public ICollection<FoundGeocache> FoundGeocaches { get; set; }

    }
}
