using OdeToFood.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdeToFood.Data.Services
{
    public class OdeToFoodDbContext : DbContext
    {
        //we indicate tables as DbSet
        //this tells EF that we have a table containing data of type Restaurant (as specified in Models directory)
        //  validation decorators from the model also apply (e.g. Required, MaxLength, etc.) - these will be added to db by EF
        public DbSet<Restaurant> Restaurants { get; set; }
    }
}
