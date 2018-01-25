using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TripInfoREST.API.Entities;
using TripInfoREST.API.Helpers;
using TripInfoREST.API.Models;
using TripInfoREST.API.Services;

namespace TripInfoREST.API.Controllers
{
    [Route("api/destinationcollections")]
    public class DestinationCollectionsController : Controller
    {
        private ITripInfoRepository _tripInfoRepository;

        public DestinationCollectionsController(ITripInfoRepository tripInfoRepository)
        {
            _tripInfoRepository = tripInfoRepository;
        }

        [HttpPost]
        public IActionResult CreateDestinationCollection(
            [FromBody] IEnumerable<DestinationForCreationDto> destinationCollection)
        {
            if (destinationCollection == null)
            {
                return BadRequest();
            }

            var destinationEntities = Mapper.Map<IEnumerable<Destination>>(destinationCollection);

            foreach (var destination in destinationEntities)
            {
                _tripInfoRepository.AddDestination(destination);
            }

            if (!_tripInfoRepository.Save())
            {
                throw new Exception("Creating an destination collection failed on save.");
            }

            var destinationCollectionToReturn = Mapper.Map<IEnumerable<DestinationDto>>(destinationEntities);
            var idsAsString = string.Join(",",
                destinationCollectionToReturn.Select(a => a.Id));

            return CreatedAtRoute("GetDestinationCollection",
                new { ids = idsAsString },
                destinationCollectionToReturn);
            //return Ok();
        }

        // (key1,key2, ...)

        [HttpGet("({ids})", Name = "GetDestinationCollection")]
        public IActionResult GetDestinationCollection(
            [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }

            var destinationEntities = _tripInfoRepository.GetDestinations(ids);

            if (ids.Count() != destinationEntities.Count())
            {
                return NotFound();
            }

            var destinationsToReturn = Mapper.Map<IEnumerable<DestinationDto>>(destinationEntities);
            return Ok(destinationsToReturn);
        }
    }
}
