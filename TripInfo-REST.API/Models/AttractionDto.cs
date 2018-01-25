using System;
namespace TripInfoREST.API.Models
{
    public class AttractionDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid DestinationId { get; set; }
    }
}
