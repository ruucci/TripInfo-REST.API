using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TripInfoREST.API.Entities;
using TripInfoREST.API.Helpers;
using TripInfoREST.API.Models;
using TripInfoREST.API.Services;

namespace TripInfoREST.API.Controllers
{
    [Route("api/destinations")]
    public class DestinationsController : Controller
    {
        private ITripInfoRepository _tripInfoRepository;
        private IMailService _mailService;
        private IUrlHelper _urlHelper;

        public DestinationsController(ITripInfoRepository tripInfoRepository, IMailService mailService, IUrlHelper urlHelper)
        {
            _tripInfoRepository = tripInfoRepository;
            _mailService = mailService;
            _urlHelper = urlHelper;
        }

        [HttpGet(Name = "GetDestinations")]
        public IActionResult GetDestinations(DestinationsResourceParameters destinationssResourceParameters)
        {
            var destinationsFromRepo = _tripInfoRepository.GetDestinations(destinationssResourceParameters);

            var previousPageLink = destinationsFromRepo.HasPrevious ?
                CreateDestinationsResourceUri(destinationssResourceParameters,
                ResourceUriType.PreviousPage) : null;

            var nextPageLink = destinationsFromRepo.HasNext ?
                CreateDestinationsResourceUri(destinationssResourceParameters,
                ResourceUriType.NextPage) : null;

            var paginationMetadata = new
            {
                totalCount = destinationsFromRepo.TotalCount,
                pageSize = destinationsFromRepo.PageSize,
                currentPage = destinationsFromRepo.CurrentPage,
                totalPages = destinationsFromRepo.TotalPages,
                previousPageLink = previousPageLink,
                nextPageLink = nextPageLink
            };

            Response.Headers.Add("X-Pagination",
                Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));


            var destinations = Mapper.Map<IEnumerable<DestinationDto>>(destinationsFromRepo);
            return Ok(destinations);
        }

        private string CreateDestinationsResourceUri(
            DestinationsResourceParameters destinationsResourceParameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetDestinations",
                      new
                      {
                          searchQuery = destinationsResourceParameters.SearchQuery,
                          genre = destinationsResourceParameters.Genre,
                          pageNumber = destinationsResourceParameters.PageNumber - 1,
                          pageSize = destinationsResourceParameters.PageSize
                      });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetDestinations",
                      new
                      {
                          searchQuery = destinationsResourceParameters.SearchQuery,
                          genre = destinationsResourceParameters.Genre,
                          pageNumber = destinationsResourceParameters.PageNumber + 1,
                          pageSize = destinationsResourceParameters.PageSize
                      });

                default:
                    return _urlHelper.Link("GetDestinations",
                    new
                    {
                        searchQuery = destinationsResourceParameters.SearchQuery,
                        genre = destinationsResourceParameters.Genre,
                        pageNumber = destinationsResourceParameters.PageNumber,
                        pageSize = destinationsResourceParameters.PageSize
                    });
            }
        }

        [HttpGet("{id}", Name = "GetDestination")]
        public IActionResult GetDestination(Guid id, [FromQuery] string fields)
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

            _mailService.Send("Destination has been deleted.", $"Destination {destinationFromRepo.Name} with id {destinationFromRepo.Id} was deleted");

            return NoContent();
        }
 
    }
}
