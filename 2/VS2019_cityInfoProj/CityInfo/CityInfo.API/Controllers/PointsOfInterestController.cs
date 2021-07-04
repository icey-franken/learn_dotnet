using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities/{cityId}/pointsofinterest")]
    public class PointsOfInterestController : ControllerBase
    {
        // constructor injection
        private readonly ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        public PointsOfInterestController(
            ILogger<PointsOfInterestController> logger,
            IMailService mailService,
            ICityInfoRepository cityInfoRepository,
            IMapper mapper
            )
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
            _mailService = mailService ??
                throw new ArgumentNullException(nameof(mailService));
            _cityInfoRepository = cityInfoRepository ??
                throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }
        //-------------------------------

        [HttpGet]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            try
            {
                if (!_cityInfoRepository.CityExists(cityId))
                {
                    _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interest.");
                    return NotFound();
                }
                var pointsOfInterestForCity = _cityInfoRepository.GetPointsOfInterestForCity(cityId);

                return Ok(_mapper.Map<IEnumerable<PointOfInterestDto>>(pointsOfInterestForCity));

                //var pointsOfInterestForCityResults = new List<PointOfInterestDto>();
                //foreach (var poi in pointsOfInterestForCity)
                //{
                //    pointsOfInterestForCityResults.Add(new PointOfInterestDto()
                //    {
                //        Id = poi.Id,
                //        Name = poi.Name,
                //        Description = poi.Description
                //    });
                //}

                //return Ok(pointsOfInterestForCityResults);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting points of interest for city with id {cityId}.", ex);
                return StatusCode(500, "A problem happened while handling your request, yo.");
            }

            //var city = CitiesDataStore.Current.Cities
            //    .FirstOrDefault(city => city.Id == cityId);

            //if (city == null)
            //{
            //    return NotFound();
            //}

            //return Ok(city.PointsOfInterest);
        }

        [HttpGet("{id}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            try
            {
                if (!_cityInfoRepository.CityExists(cityId))
                {
                    _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interest.");
                    return NotFound();
                }

                var pointOfInterest = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);

                if (pointOfInterest == null)
                {
                    return NotFound();
                }

                return Ok(_mapper.Map<PointOfInterestDto>(pointOfInterest));

                //var pointOfInterestResult = new PointOfInterestDto()
                //{
                //    Id = pointOfInterest.Id,
                //    Name = pointOfInterest.Name,
                //    Description = pointOfInterest.Description
                //};

                //return Ok(pointOfInterestResult);

            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting points of interest for city with id {cityId}.", ex);
                return StatusCode(500, "A problem happened while handling your request, yo.");
            }
        }

        [HttpPost]
        public IActionResult CreatePointOfInterest(int cityId,
            [FromBody] PointOfInterestForCreationDto pointOfInterest)
        {
            if (pointOfInterest.Description == pointOfInterest.Name)
            {
                ModelState.AddModelError(
                    "Description",
                    "The provided description should be different from the name"
                    );
            }

            // checks validations on model (PointOfInterestForCreationDto)
            //  if validations not satisfied, ModelState.IsValid property set to false
            //  note that again this is done automatically by ApiController attribute.
            // BUT since we added a custom model error above, we have to 
            //  check this manually, like so:
            if (!ModelState.IsValid)
            {
                //we pass in ModelState here so that the response object
                //  has access to the error messages (?) on ModelState.errors (?) 
                return BadRequest(ModelState);
            }

            // if pointOfInterest is null, ApiController attribute
            //  will automatically return a bad request response
            //  so we don't need the following lines of code
            //if (pointOfInterest == null)
            //{
            //    return BadRequest();
            //}

            // replace with mapping ---------------------------
            //// ensure city that point of interest added to exists
            //var city = CitiesDataStore.Current.Cities.FirstOrDefault(
            //    city => city.Id == cityId);
            //if (city == null)
            //{
            //    return NotFound();
            //}

            //// determine id of new point of interest.
            ////  This will be refactored later
            //var maxPointOfInterestId = CitiesDataStore.Current.Cities.SelectMany(
            //    city => city.PointsOfInterest).Max(point => point.Id);
            //var finalPointOfInterest = new PointOfInterestDto()
            //{
            //    Id = ++maxPointOfInterestId,
            //    Name = pointOfInterest.Name,
            //    Description = pointOfInterest.Description,
            //};
            //city.PointsOfInterest.Add(finalPointOfInterest);

            //return CreatedAtRoute(
            //    "GetPointOfInterest",
            //    new { cityId, id = finalPointOfInterest.Id },
            //    finalPointOfInterest
            //);
            //----------------------------------


            if (!_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var finalPointOfInterest = _mapper.Map<Entities.PointOfInterest>(pointOfInterest);

            _cityInfoRepository.AddPointOfInterestForCity(cityId, finalPointOfInterest);

            _cityInfoRepository.Save();

            //we don't want to return the ENTITY, we want to return the poi dto
            var createdPointOfInterestToReturn = _mapper
                .Map<PointOfInterestDto>(finalPointOfInterest);

            return CreatedAtRoute(
                "GetPointOfInterest",
                new
                {
                    cityId,
                    id = createdPointOfInterestToReturn.Id
                },
                createdPointOfInterestToReturn);
        }


        [HttpPut("{id}")]
        public IActionResult UpdatePointOfInterest(
            int cityId,
            int id,
            [FromBody] PointOfInterestForUpdateDto pointOfInterest)
        {
            // validate that description is not the same as name
            if (pointOfInterest.Description == pointOfInterest.Name)
            {
                ModelState.AddModelError(
                    "Description",
                    "The provided description should be different from the name.");
            }

            //check validations, INCLUDING custom validation from above
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //check that city exists
            if (!_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }

            //check the poi exists
            var pointOfInterestEntity = _cityInfoRepository.GetPointOfInterestForCity(cityId, id);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            _mapper.Map(pointOfInterest, pointOfInterestEntity);

            _cityInfoRepository.UpdatePointOfInterestForCity(cityId, pointOfInterestEntity);

            _cityInfoRepository.Save();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, int id,
            [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            if (!_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = _cityInfoRepository
                .GetPointOfInterestForCity(cityId, id);
            if (pointOfInterestEntity == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = _mapper
                .Map<PointOfInterestForUpdateDto>(pointOfInterestEntity);

            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (pointOfInterestToPatch.Description == pointOfInterestToPatch.Name)
            {
                ModelState.AddModelError(
                    "Description",
                    "The provided description should be different from the name.");
            }

            if (!TryValidateModel(pointOfInterestToPatch))
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);

            _cityInfoRepository.UpdatePointOfInterestForCity(cityId, pointOfInterestEntity);

            _cityInfoRepository.Save();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            if (!_cityInfoRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterestEntity = _cityInfoRepository
                .GetPointOfInterestForCity(cityId, id);

            if(pointOfInterestEntity == null)
            {
                return NotFound();
            }

            _cityInfoRepository.DeletePointOfInterest(pointOfInterestEntity);

            _cityInfoRepository.Save();



            _mailService.Send("Point of interest deleted.",
                $"Point of interest {pointOfInterestEntity.Name} with id {pointOfInterestEntity.Id} was deleted.");

            return NoContent();
        }

    }

}
