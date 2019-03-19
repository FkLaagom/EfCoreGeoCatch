using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocaching.Models
{
    public class Geocashe
    {
        [Key]
        public int ID { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        [MaxLength(255)]
        [Required]
        public string Content { get; set; }
        [MaxLength(255)]
        [Required]
        public string Message { get; set; }
        public Person Person { get; set; }
    }
}
