using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Profiles
{
    public class CityProfile :Profile
    {
        public CityProfile()
        {
            //map from City Entity to city without points of interest dto
            //maps property names on source object to same property names on destination object
            //ignores null reference exceptions (if property doesn't exist, it'll ignore)
            //usually you don't have to provide your own mappings - automapper is smart
            CreateMap<Entities.City, Models.CityWithoutPointsOfInterestDto>();
            CreateMap<Entities.City, Models.CityDto>();
        }
    }
}
