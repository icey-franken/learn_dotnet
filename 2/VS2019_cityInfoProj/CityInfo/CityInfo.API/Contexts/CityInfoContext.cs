using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Context
{
    public class CityInfoContext : DbContext
    {
        //"A DbSet can be used to query and save instances of its entity type,
        //so linked queries against the dbset will be translated into queries against the database"

        //we need to register context with application so it's available for dependency injection.
        public DbSet<City> Cities { get; set; }
        public DbSet<PointOfInterest> PointsOfInterest { get; set; }

        public CityInfoContext(DbContextOptions<CityInfoContext> options) : base(options)
        {
            //ensure that if database doesn't exist, it is created. In a code first approach this makes sense -
            // the db is created by the code.
            //this ensures that the database is created when the city info context constructor initializes (instance creation)
            //remember: just REGISTERING city info context (on build/run of solution) in our startup file
            //  does NOT call the constructor. I think that comes later - dependency injection?
            //Database.EnsureCreated();
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("connectionString")
        //    base.OnConfiguring(optionsBuilder);
        //}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //use OnModelCreating to access modelBUilder:
            //  can manually construct model if things are complicated or if you want to be explicit
            //  can be used to seed the database
            modelBuilder.Entity<City>()
                .HasData(new City()
                {
                    Id = 1,
                    Name = "A Place",
                    Description = "A Description"
                }
                );
            modelBuilder.Entity<PointOfInterest>()
                .HasData(new PointOfInterest()
                {
                    Id = 1,
                    CityId = 1,
                    Name = "POI 1",
                    Description = "POI 1 des"
                });
            modelBuilder.Entity<PointOfInterest>()
                .HasData(new PointOfInterest()
                {
                    Id = 2,
                    CityId = 1,
                    Name = "POI 2",
                    Description = "POI 2 des"
                });
            base.OnModelCreating(modelBuilder);
        }
    }
}
