using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TripInfoREST.API.Entities
{
    public class Destination
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string State { get; set; }

        [Required]
        public DateTimeOffset DateOfVisit { get; set; }

        [Required]
        [MaxLength(50)]
        public string Genre { get; set; }

        public ICollection<Attraction> Attractions { get; set; }
            = new List<Attraction>();
    }
}
