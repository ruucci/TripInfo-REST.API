using System;
namespace TripInfoREST.API.Models
{
    public class DestinationDto
    {
        public Guid Id { get; set; }
        public string Location { get; set; }
        public string Age { get; set; }
        public string Genre { get; set; }
    }
}