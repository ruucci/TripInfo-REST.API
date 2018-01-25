using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TripInfoREST.API.Entities;
using TripInfoREST.API.Models;
using TripInfoREST.API.Services;

namespace TripInfoREST.API.Controllers
{
    [Route("api/destinations")]
    public class DestinationsController : Controller
    {
        private ITripInfoRepository _tripInfoRepository;

        public DestinationsController(ITripInfoRepository tripInfoRepository)
        {
            _tripInfoRepository = tripInfoRepository;
        }

        [HttpGet()]
        public IActionResult GetDestinations()
        {
            var destinationsFromRepo = _tripInfoRepository.GetDestinations();

            var destinations = Mapper.Map<IEnumerable<DestinationDto>>(destinationsFromRepo);
            return Ok(destinations);
        }

        [HttpGet("{id}", Name = "GetDestination")]
        public IActionResult GetDestination(Guid id)
        {
            var destinationFromRepo = _tripInfoRepository.GetDestination(id);

            if (destinationFromRepo == null)
            {
                return NotFound();
            }

            var destination = Mapper.Map<DestinationDto>(destinationFromRepo);
            return Ok(destination);
        }

        [HttpPost]
        public IActionResult CreateDestination([FromBody] DestinationForCreationDto destination)
        {
            if (destination == null)
            {
                return BadRequest();
            }

            var destinationEntity = Mapper.Map<Destination>(destination);

            _tripInfoRepository.AddDestination(destinationEntity);

            if (!_tripInfoRepository.Save())
            {
                throw new Exception("Creating an destination failed on save.");
               // return StatusCode(500, "A problem happened with handling your request.");
            }

            var destinationToReturn = Mapper.Map<DestinationDto>(destinationEntity);

            return CreatedAtRoute("GetDestination",
                new { id = destinationToReturn.Id },
                destinationToReturn);
        }

        [HttpPost("{id}")]
        public IActionResult BlockDestinationCreation(Guid id)
        {
            if (_tripInfoRepository.DestinationExists(id))
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }

            return NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteDestination(Guid id)
        {
            var destinationFromRepo = _tripInfoRepository.GetDestination(id);
            if (destinationFromRepo == null)
            {
                return NotFound();
            }

            _tripInfoRepository.DeleteDestination(destinationFromRepo);

            if (!_tripInfoRepository.Save())
            {
                throw new Exception($"Deleting destination {id} failed on save.");
            }

            return NoContent();
        }
 
    }
}
