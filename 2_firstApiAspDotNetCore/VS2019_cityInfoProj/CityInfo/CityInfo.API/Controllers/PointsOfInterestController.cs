using CityInfo.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;

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
        [HttpGet]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            var city = CitiesDataStore.Current.Cities
                .FirstOrDefault(city => city.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            return Ok(city.PointsOfInterest);
        }

        [HttpGet("{id}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            var city = CitiesDataStore.Current.Cities
                .FirstOrDefault(city => city.Id == cityId);

            if (city == null)
            {
                return NotFound();
            }

            // if city does exist, find the point of interest
            var pointOfInterest = city.PointsOfInterest
                .FirstOrDefault(pointOfInterest => pointOfInterest.Id == id);

            if (pointOfInterest == null)
            {
                return NotFound();
            }

            return Ok(pointOfInterest);

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

            // ensure city that point of interest added to exists
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(
                city => city.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            // determine id of new point of interest.
            //  This will be refactored later
            var maxPointOfInterestId = CitiesDataStore.Current.Cities.SelectMany(
                city => city.PointsOfInterest).Max(point => point.Id);
            var finalPointOfInterest = new PointOfInterestDto()
            {
                Id = ++maxPointOfInterestId,
                Name = pointOfInterest.Name,
                Description = pointOfInterest.Description,
            };
            city.PointsOfInterest.Add(finalPointOfInterest);

            return CreatedAtRoute(
                "GetPointOfInterest",
                new { cityId, id = finalPointOfInterest.Id },
                finalPointOfInterest
            );
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
            var city = CitiesDataStore.Current.Cities
                .FirstOrDefault(city => city.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            //check that point of interest exists in store
            var pointOfInterestFromStore = city.PointsOfInterest
                .FirstOrDefault(point => point.Id == id);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            //if all satisfied - update result
            //since this is a put request, the resource should be FULLY update
            // i.e. name and description (not id)

            pointOfInterestFromStore.Name = pointOfInterest.Name;
            pointOfInterestFromStore.Description = pointOfInterest.Description;

            //return no content (204) because resource updated.
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, int id,
            [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            //check if city can be found
            var city = CitiesDataStore.Current.Cities
                .FirstOrDefault(city => city.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            //then check if point of interest can be found
            var pointOfInterestFromStore = city.PointsOfInterest
                .FirstOrDefault(point => point.Id == id);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            //apply the patch document
            //  transform point of interest we got from the store 
            //  to a point of interest from poiForUpdateDto
            //  (effectively removing the id)
            var pointOfInterestToPatch =
                new PointOfInterestForUpdateDto()
                {
                    Name = pointOfInterestFromStore.Name,
                    Description = pointOfInterestFromStore.Description
                };

            // pass ModelState through to ApplyTo. Then, if there's a problem
            //  with the patchDocument, this error will be added to ModelState
            //  and we can handle it below
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

            //if all is good at this point, then update the point of interest
            pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
            pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            var city = CitiesDataStore.Current.Cities
                .FirstOrDefault(city => city.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestFromStore = city.PointsOfInterest
                .FirstOrDefault(point => point.Id == id);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            city.PointsOfInterest.Remove(pointOfInterestFromStore);

            return NoContent();
        }

    }

}
