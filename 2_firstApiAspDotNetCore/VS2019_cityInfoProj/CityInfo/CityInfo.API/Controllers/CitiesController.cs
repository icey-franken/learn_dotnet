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
        //adding an HttpGet attribute to be executed when request sent to "api/cities"
        [HttpGet]
        //class JsonResult returns jsonified version of whatever passed into constructor of new JsonResult object
        public IActionResult GetCities()
        {
            var citiesToReturn = CitiesDataStore.Current.Cities;
            return Ok(citiesToReturn);
            //return new JsonResult(CitiesDataStore.Current.Cities);
        }

        //remember: we set the route template to api/cities, so we only look for id here to find api/cities/id
        //  curly braces are used to indicate a parameter
        [HttpGet("{id}")]
        public IActionResult GetCity(int id)
        {
            var cityToReturn = CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id == id);

            if (cityToReturn == null)
            {
                return NotFound();
            }

            return Ok(cityToReturn);

            //return new JsonResult(CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id == id));
        }
    }
}
