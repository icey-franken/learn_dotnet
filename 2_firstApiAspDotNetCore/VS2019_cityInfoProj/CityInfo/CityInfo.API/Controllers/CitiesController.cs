using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Controllers
{
    [ApiController]
    //use route attribute at controller level to add a URI 'template'. Now, all routes with 'api/cities' will
    //  go through this controller, and actions within do not need 'api/cities' prefixed.
    [Route("api/cities")]
    //OR
    //[Route("api/[controller]")]
    //we can do this because our controller is named CitiesController. Note that this could create issues when 
    //  refactoring, because if we change the name of our controller, all requests will be routed to a new place.
    public class CitiesController : ControllerBase
    {
        //inject repository
        private readonly ICityInfoRepository _cityInfoRepository;
        private readonly IMapper _mapper;
        public CitiesController(ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            _cityInfoRepository = cityInfoRepository ??
                throw new ArgumentNullException(nameof(cityInfoRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }


        //adding an HttpGet attribute to be executed when request sent to "api/cities"
        [HttpGet]
        //class JsonResult returns jsonified version of whatever passed into constructor of new JsonResult object
        public IActionResult GetCities()
        {
            var cityEntities = _cityInfoRepository.GetCities();
            //IMapper allows us to turn this manual mapping: 
            //---------------------------------------------
            //var results = new List<CityWithoutPointsOfInterestDto>();

            //foreach (var cityEntity in cityEntities)
            //{
            //    results.Add(new CityWithoutPointsOfInterestDto
            //    {
            //        Id = cityEntity.Id,
            //        Description = cityEntity.Description,
            //        Name = cityEntity.Name
            //    });
            //}
            //return Ok(results);
            //---------------------------------------------
            //to this:
            //---------------------------------------------
            return Ok(_mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities));
            //---------------------------------------------
        }

        //remember: we set the route template to api/cities, so we only look for id here to find api/cities/id
        //  curly braces are used to indicate a parameter
        [HttpGet("{id}")]
        public IActionResult GetCity(int id, bool includePointsOfInterest = false)
        {
            var city = _cityInfoRepository.GetCity(id, includePointsOfInterest);

            if (city == null)
            {
                return NotFound();
            }

            if (includePointsOfInterest)
            {
                //var cityResult = new CityDto()
                //{
                //    Id = city.Id,
                //    Name = city.Name,
                //    Description = city.Description
                //};

                //foreach (var poi in city.PointsOfInterest)
                //{
                //    cityResult.PointsOfInterest.Add(
                //        new PointOfInterestDto()
                //        {
                //            Id = poi.Id,
                //            Name = poi.Name,
                //            Description = poi.Description
                //        });
                //}

                return Ok(_mapper.Map<CityDto>(city));
            }
            else
            {
                //var cityWithoutPointsOfInterestResult = new CityWithoutPointsOfInterestDto()
                //{
                //    Id = city.Id,
                //    Name = city.Name,
                //    Description = city.Description
                //};
                //return Ok(cityWithoutPointsOfInterestResult);

                return Ok(_mapper.Map<CityWithoutPointsOfInterestDto>(city));
            }

            //var cityToReturn = CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id == id);

            //if (cityToReturn == null)
            //{
            //    return NotFound();
            //}

            //return Ok(cityToReturn);

            //return new JsonResult(CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id == id));
        }
    }
}
