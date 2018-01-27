using System;
using System.Collections.Generic;
using System.Linq;
using TripInfoREST.API.Entities;
using TripInfoREST.API.Helpers;
using TripInfoREST.API.Models;

namespace TripInfoREST.API.Services
{
    public class TripInfoRepository : ITripInfoRepository
    {
        private TripContext _context;
        private IPropertyMappingService _propertyMappingService;

        public TripInfoRepository(TripContext context, IPropertyMappingService propertyMappingService)
        {
            _context = context;
            _propertyMappingService = propertyMappingService;
        }   

        public void AddDestination(Destination destination)
        {
            destination.Id = Guid.NewGuid();
            _context.Destinations.Add(destination);

            // the repository fills the id (instead of using identity columns)
            if (destination.Attractions.Any())
            {
                foreach (var attraction in destination.Attractions)
                {
                    attraction.Id = Guid.NewGuid();
                }
            }
        }

        public void AddAttractionForDestination(Guid destinationId, Attraction attraction)
        {
            var destination = GetDestination(destinationId);
            if (destination != null)
            {
                // if there isn't an id filled out (ie: we're not upserting),
                // we should generate one
                if (attraction.Id == Guid.Empty)
                {
                    attraction.Id = Guid.NewGuid();
                }
                destination.Attractions.Add(attraction);
            }
        }

        public bool DestinationExists(Guid destinationId)
        {
            return _context.Destinations.Any(d => d.Id == destinationId);
        }

        public void DeleteDestination(Destination destination)
        {
            _context.Destinations.Remove(destination);
        }

        public void DeleteAttraction(Attraction attraction)
        {
            _context.Attractions.Remove(attraction);
        }

        public Destination GetDestination(Guid destinationId)
        {
            return _context.Destinations.FirstOrDefault(d => d.Id == destinationId);
        }

        public IEnumerable<Destination> GetDestinations()
        {
            return _context.Destinations.OrderBy(d => d.Name).ThenBy(d => d.State).ToList();
        }

        public PagedList<Destination> GetDestinations(
            DestinationsResourceParameters destinationsResourceParameters)
        {
            //var collectionBeforePaging = _context.Destinations
            //    .OrderBy(a => a.FirstName)
            //    .ThenBy(a => a.LastName).AsQueryable();

            var collectionBeforePaging =_context.Destinations.ApplySort(destinationsResourceParameters.OrderBy, _propertyMappingService.GetPropertyMapping<DestinationDto, Destination>());

            if (!string.IsNullOrEmpty(destinationsResourceParameters.Genre))
            {
                // trim & ignore casing
                var genreForWhereClause = destinationsResourceParameters.Genre
                    .Trim().ToLowerInvariant();
                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Genre.ToLowerInvariant() == genreForWhereClause);
            }

            if (!string.IsNullOrEmpty(destinationsResourceParameters.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = destinationsResourceParameters.SearchQuery
                    .Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging
                    .Where(a => a.Genre.ToLowerInvariant().Contains(searchQueryForWhereClause)
                           || a.Name.ToLowerInvariant().Contains(searchQueryForWhereClause)
                           || a.State.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }

            return PagedList<Destination>.Create(collectionBeforePaging,
                destinationsResourceParameters.PageNumber,
                destinationsResourceParameters.PageSize);
        }

        public IEnumerable<Destination> GetDestinations(IEnumerable<Guid> destinationIds)
        {
            return _context.Destinations.Where(a => destinationIds.Contains(a.Id))
                           .OrderBy(a => a.Name)
                           .OrderBy(a => a.State)
                           .ToList();
        }


        public void UpdateDestination(Destination destination)
        {
            // no code in this implementation
        }

        public Attraction GetAttractionForDestination(Guid destinationId, Guid attractionId)
        {
            return _context.Attractions
                           .Where(a => a.DestinationId == destinationId && a.Id == attractionId).FirstOrDefault();
        }

        public IEnumerable<Attraction> GetAttractionsForDestination(Guid destinationId)
        {
            return _context.Attractions
                           .Where(a => a.DestinationId == destinationId).OrderBy(a => a.Name).ToList();
        }

        public void UpdateAttractionForDestination(Attraction attraction)
        {
            // no code in this implementation
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}