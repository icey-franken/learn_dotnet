using OdeToFood.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OdeToFood.Data.Services
{
    public class SqlRestaurantData : IRestaurantData
    {
        private readonly OdeToFoodDbContext db;
        public SqlRestaurantData(OdeToFoodDbContext db)
        {
            this.db = db;
        }

        public void Add(Restaurant restaurant)
        {
            db.Restaurants.Add(restaurant);
            db.SaveChanges();
        }

        public void Delete(int id)
        {
            var restaurant = Get(id);
            if(restaurant != null)
            {
                db.Restaurants.Remove(restaurant);
                db.SaveChanges();
            }
        }

        public Restaurant Get(int id)
        {
            return db.Restaurants.FirstOrDefault(r => r.Id == id);
        }

        public IEnumerable<Restaurant> GetAll()
        {
            return db.Restaurants.OrderBy(r => r.Name);
        }

        public void Update(Restaurant restaurant)
        {
            //"I have a new entry for you to track" - it finds restaurant to be updated via primary key
            var entry = db.Entry(restaurant);
            //"I want you to know this entry is in a modified state" - tell EF that something has changed,
            //  that way, when you call save changes, it knows to look at this entry, see that something changed, and save the changes.
            entry.State = EntityState.Modified;
            db.SaveChanges();          
            //THIS IS HOW YOU GET AROUND UPDATING INDIVIDUAL FIELDS!!!!!
        }
    }
}
