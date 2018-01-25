using System;
using System.Collections.Generic;

namespace TripInfoREST.API.Models
{
    public class DestinationForCreationDto
    {
        public string Name { get; set; }
        public string State { get; set; }
        public DateTimeOffset DateOfVisit { get; set; }
        public string Genre { get; set; }

        public ICollection<AttractionForCreationDto> Attractions { get; set; }
        = new List<AttractionForCreationDto>();
    }
}
