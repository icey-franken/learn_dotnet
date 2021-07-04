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
        //now we register this in the configure services method in startup.cs - add scoped 


        public bool CityExists(int cityId)
        {
            return _context.Cities
                .Any(c => c.Id == cityId);
        }

        public void AddPointOfInterestForCity(int cityId, PointOfInterest pointOfInterest)
        {
            var city = GetCity(cityId, false);
            //this only add to object context - in memory. Has NOT added to db - to do that we call save changes on context 
            city.PointsOfInterest.Add(pointOfInterest);
        }

        public void UpdatePointOfInterestForCity(int cityId, PointOfInterest pointOfInterest)
        {
            //for our implementation this method happens to be empty, because EF core takes care of it for us.
        }

        public void DeletePointOfInterest(PointOfInterest pointOfInterest)
        {
            _context.PointsOfInterest.Remove(pointOfInterest);
        }

        public bool Save()
        {
            //returns number of entities changed. 
            return (_context.SaveChanges() >= 0);
        }
    }
}
