using System;
using System.Collections.Generic;
using TripInfoREST.API.Entities;
using TripInfoREST.API.Helpers;

namespace TripInfoREST.API.Services
{
    public interface ITripInfoRepository
    {
        PagedList<Destination> GetDestinations(DestinationsResourceParameters destinationsResourceParameters);
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
