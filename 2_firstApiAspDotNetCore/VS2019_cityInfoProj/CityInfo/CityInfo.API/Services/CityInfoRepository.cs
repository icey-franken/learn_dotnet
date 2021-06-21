using CityInfo.API.Context;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Services
{
    //this is where we provide persistence logic - EF core, a different service, etc
    //when I typed ': ICityInfoRepository' the below code was autofilled with methods containing only 'throw new NotImplementedException();'
    //   profile & repository in CCC vs this - repo and Irepo?
    public class CityInfoRepository : ICityInfoRepository
    {
        //DEPENDENCY INJECTION!!!
        private CityInfoContext _context;
        public CityInfoRepository(CityInfoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<City> GetCities()
        {
            return _context.Cities
                .OrderBy(c => c.Name)
                .ToList();
        }

        public City GetCity(int cityId, bool includePointsOfInterest)
        {
            if (includePointsOfInterest)
            {
                return _context.Cities
                    .Include(c => c.PointsOfInterest)
                    .Where(c => c.Id == cityId)
                    .FirstOrDefault();
            }
            else
            {
                return _context.Cities
                    .Where(c => c.Id == cityId)
                    .FirstOrDefault();
            }
        }

        public PointOfInterest GetPointOfInterestForCity(int cityId, int pointOfInterestId)
        {
            return _context.PointsOfInterest
                .Where(p => p.CityId == cityId && p.Id == pointOfInterestId)
                .FirstOrDefault();
        }

        public IEnumerable<PointOfInterest> GetPointsOfInterestForCity(int cityId)
        {
            return _context.PointsOfInterest
                .Where(p => p.CityId == cityId)
                .OrderBy(p => p.Name)
                .ToList();
        }
        //now we register this in the configure services method - add scoped 
    }
}
