using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocaching.Models
{
    public class Geocashe : IEntity
    {
        [Key]
        public int ID { get; set; }
        public Location Location { get; set; }
        [MaxLength(255)]
        [Required]
        public string Content { get; set; }
        [MaxLength(255)]
        [Required]
        public string Message { get; set; }
        public int? PersonID { get; set; }
        public Person Person { get; set; }
    }
}
