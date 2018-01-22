using System;
using System.Collections.Generic;
using TripInfoREST.API.Entities;

namespace TripInfoREST.API.Services
{
    public interface ITripInfoRepository
    {
        IEnumerable<Destination> GetDestinations();
        Destination GetDestination(Guid destinationId);
        IEnumerable<Destination> GetDestinations(IEnumerable<Guid> destinationIds);
        void AddDestination(Destination destination);
        void DeleteDestination(Destination destination);
        void UpdateDestination(Destination destination);
        bool DestinationExists(Guid destinationId);
        IEnumerable<Attraction> GetAttractionsForDestination(Guid destinationId);
        Attraction GetAttractionForDestination(Guid destinationId, Guid attractionId);
        void AddAttractionForDestination(Guid destinationId, Attraction attraction);
        void UpdateAttractionForDestination(Attraction attraction);
        void DeleteAttraction(Attraction attraction);
        bool Save();
    }
}
