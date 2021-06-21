using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Profiles
{
    public class PointOfInterestProfile : Profile
    {
        public PointOfInterestProfile()
        {
            //I don't understand why the order (source destination) is different here
            CreateMap<Entities.PointOfInterest, Models.PointOfInterestDto>();
            CreateMap<Models.PointOfInterestForCreationDto, Entities.PointOfInterest>();

            //this:
            //---
            //CreateMap<Models.PointOfInterestForUpdateDto, Entities.PointOfInterest>();
            //CreateMap<Entities.PointOfInterest, Models.PointOfInterestForUpdateDto>();
            //---

            //is the same as this:
            //---
            CreateMap<Models.PointOfInterestForUpdateDto, Entities.PointOfInterest>()
                .ReverseMap();
            //---
        }
    }
}
