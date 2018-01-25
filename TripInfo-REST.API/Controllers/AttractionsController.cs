using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TripInfoREST.API.Entities;
using TripInfoREST.API.Helpers;
using TripInfoREST.API.Models;
using TripInfoREST.API.Services;

namespace TripInfoREST.API.Controllers
{
    [Route("api/destinations/{destinationId}/attractions")]
    public class AttractionsController : Controller
    {
        private ITripInfoRepository _tripInfoRepository;
        private ILogger<AttractionsController> _logger;

        public AttractionsController(ITripInfoRepository tripInfoRepository, ILogger<AttractionsController> logger)
        {
            _tripInfoRepository = tripInfoRepository;
            _logger = logger;
        }

        [HttpGet()]
        public IActionResult GetAttractionsForDestination(Guid destinationId)
        {
            if (!_tripInfoRepository.DestinationExists(destinationId))
            {
                return NotFound();
            }

            var attractionsForDestinationFromRepo = _tripInfoRepository.GetAttractionsForDestination(destinationId);

            var attractionsForDestination = Mapper.Map<IEnumerable<AttractionDto>>(attractionsForDestinationFromRepo);

            return Ok(attractionsForDestination);
        }

        [HttpGet("{id}", Name = "GetAttractionForDestination")]
        public IActionResult GetAttractionForDestination(Guid destinationId, Guid id)
        {
            if (!_tripInfoRepository.DestinationExists(destinationId))
            {
                return NotFound();
            }

            var attractionForDestinationFromRepo = _tripInfoRepository.GetAttractionForDestination(destinationId, id);
            if (attractionForDestinationFromRepo == null)
            {
                return NotFound();
            }

            var attractionForDestination = Mapper.Map<AttractionDto>(attractionForDestinationFromRepo);
            return Ok(attractionForDestination);
        }

        [HttpPost()]
        public IActionResult CreateAttractionForDestination(Guid destinationId,
            [FromBody] AttractionForCreationDto attraction)
        {
            if (attraction == null)
            {
                return BadRequest();
            }

            if (attraction.Description == attraction.Name)
            {
                ModelState.AddModelError(nameof(AttractionForCreationDto),
                    "The provided description should be different from the name.");
            }

            if (!ModelState.IsValid)
            {
                // return 422
                return new UnprocessableEntityObjectResult(ModelState);
            }


            if (!_tripInfoRepository.DestinationExists(destinationId))
            {
                return NotFound();
            }

            var attractionEntity = Mapper.Map<Attraction>(attraction);

            _tripInfoRepository.AddAttractionForDestination(destinationId, attractionEntity);

            if (!_tripInfoRepository.Save())
            {
                throw new Exception($"Creating a attraction for destination {destinationId} failed on save.");
            }

            var attractionToReturn = Mapper.Map<AttractionDto>(attractionEntity);

            return CreatedAtRoute("GetAttractionForDestination",
                new { destinationId = destinationId, id = attractionToReturn.Id },
                attractionToReturn);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAttractionForDestination(Guid destinationId, Guid id)
        {
            if (!_tripInfoRepository.DestinationExists(destinationId))
            {
                return NotFound();
            }

            var attractionForDestinationFromRepo = _tripInfoRepository.GetAttractionForDestination(destinationId, id);
            if (attractionForDestinationFromRepo == null)
            {
                return NotFound();
            }

            _tripInfoRepository.DeleteAttraction(attractionForDestinationFromRepo);

            if (!_tripInfoRepository.Save())
            {
                throw new Exception($"Deleting attraction {id} for destination {destinationId} failed on save.");
            }

            _logger.LogInformation(100, $"Attraction {id} for destination {destinationId} was deleted.");

            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAttractionForDestination(Guid destinationId, Guid id,
            [FromBody] AttractionForUpdateDto attraction)
        {
            if (attraction == null)
            {
                return BadRequest();
            }

            if (attraction.Description == attraction.Name)
            {
                ModelState.AddModelError(nameof(AttractionForUpdateDto),
                    "The provided description should be different from the name.");
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }


            if (!_tripInfoRepository.DestinationExists(destinationId))
            {
                return NotFound();
            }

            var attractionForDestinationFromRepo = _tripInfoRepository.GetAttractionForDestination(destinationId, id);
            if (attractionForDestinationFromRepo == null)
            {
                var attractionToAdd = Mapper.Map<Attraction>(attraction);
                attractionToAdd.Id = id;

                _tripInfoRepository.AddAttractionForDestination(destinationId, attractionToAdd);

                if (!_tripInfoRepository.Save())
                {
                    throw new Exception($"Upserting attraction {id} for destination {destinationId} failed on save.");
                }

                var attractionToReturn = Mapper.Map<AttractionDto>(attractionToAdd);

                return CreatedAtRoute("GetAttractionForDestination",
                    new { destinationId = destinationId, id = attractionToReturn.Id },
                    attractionToReturn);
            }

            Mapper.Map(attraction, attractionForDestinationFromRepo);

            _tripInfoRepository.UpdateAttractionForDestination(attractionForDestinationFromRepo);

            if (!_tripInfoRepository.Save())
            {
                throw new Exception($"Updating attraction {id} for destination {destinationId} failed on save.");
            }

            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdateAttractionForDestination(Guid destinationId, Guid id,
            [FromBody] JsonPatchDocument<AttractionForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            if (!_tripInfoRepository.DestinationExists(destinationId))
            {
                return NotFound();
            }

            var attractionForDestinationFromRepo = _tripInfoRepository.GetAttractionForDestination(destinationId, id);

            if (attractionForDestinationFromRepo == null)
            {
                var attractionDto = new AttractionForUpdateDto();
                patchDoc.ApplyTo(attractionDto, ModelState);

                if (attractionDto.Description == attractionDto.Name)
                {
                    ModelState.AddModelError(nameof(AttractionForUpdateDto),
                        "The provided description should be different from the name.");
                }

                TryValidateModel(attractionDto);

                if (!ModelState.IsValid)
                {
                    return new UnprocessableEntityObjectResult(ModelState);
                }

                var attractionToAdd = Mapper.Map<Attraction>(attractionDto);
                attractionToAdd.Id = id;

                _tripInfoRepository.AddAttractionForDestination(destinationId, attractionToAdd);

                if (!_tripInfoRepository.Save())
                {
                    throw new Exception($"Upserting attraction {id} for destination {destinationId} failed on save.");
                }

                var attractionToReturn = Mapper.Map<AttractionDto>(attractionToAdd);
                return CreatedAtRoute("GetAttractionForDestination",
                    new { destinationId = destinationId, id = attractionToReturn.Id },
                    attractionToReturn);
            }

            var attractionToPatch = Mapper.Map<AttractionForUpdateDto>(attractionForDestinationFromRepo);

            patchDoc.ApplyTo(attractionToPatch, ModelState);

            // patchDoc.ApplyTo(attractionToPatch);

            if (attractionToPatch.Description == attractionToPatch.Name)
            {
                ModelState.AddModelError(nameof(AttractionForUpdateDto),
                    "The provided description should be different from the name.");
            }

            TryValidateModel(attractionToPatch);

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            Mapper.Map(attractionToPatch, attractionForDestinationFromRepo);

            _tripInfoRepository.UpdateAttractionForDestination(attractionForDestinationFromRepo);

            if (!_tripInfoRepository.Save())
            {
                throw new Exception($"Patching attraction {id} for destination {destinationId} failed on save.");
            }

            return NoContent();
        }

    }
}
