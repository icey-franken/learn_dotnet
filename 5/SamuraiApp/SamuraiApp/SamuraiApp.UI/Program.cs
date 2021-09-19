using System;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace SamuraiApp.UI
{
    class Program
    {
        private static SamuraiContext _context = new SamuraiContext();
        //private static SamuraiContextNoTracking _context = new SamuraiContextNoTracking();

        private static void Main(string[] args)
        {
            Console.WriteLine("Press any key...");
            Console.ReadKey();
            QueryAndUpdateBattles_Disconnected();
        }

        private static void AddSamuraisByName(params string[] names)
        {
            foreach(string name in names)
            {
                _context.Samurais.Add(new Samurai { Name = name });
            }
            _context.SaveChanges();
        }
        private static void AddBattlesByName(params string[] names)
        {
            foreach(string name in names)
            {
                _context.Battles.AddRange(new Battle { Name = name });
            }
            _context.SaveChanges();
        }
        private static void GetSamurais()
        {
            var samurais = _context.Samurais
                .TagWith("ConsoleApp.Program.GetSamurais method")
                .ToList();
            Console.WriteLine($"Samurai count is {samurais.Count}");
            foreach(var samurai in samurais)
            {
                Console.WriteLine(samurai.Name);
            }
        }
        private static void QueryFilters()
        {
            //var name = "Sampson";
            //var samurais = _context.Samurais.Where(s => s.Name == name).ToList();
            var target = "J";
            var samurais = _context.Samurais.Where(s => s.Name.StartsWith(target)).ToList();
        }
        private static void QueryAggregates()
        {
            //var name = "Sampson";
            //var samurais = _context.Samurais.Where(s => s.Name == name).ToList();
            var name = "Sampson";
            var samurai = _context.Samurais.FirstOrDefault(s => s.Name == name);
        }
        private static void RetrieveAndUpdateSamurai()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Name += "San";
            _context.SaveChanges();
        }
        private static void RetrieveAndUpdateMultipleSamurais()
        {
            var samurais = _context.Samurais.Skip(1).Take(4).ToList();
            samurais.ForEach(s => s.Name += "San");
            _context.SaveChanges();
        }
        private static void MultipleDatabaseOperations()
        {
            var samurai = _context.Samurais.FirstOrDefault();
            samurai.Name += "San";
            _context.Samurais.Add(new Samurai { Name = "Isaac" });
            _context.SaveChanges();
        }
        private static void RetrieveAndDeleteASamurai()
        {
            var samurai = _context.Samurais.FirstOrDefault(s=>s.Id == 19);
            if (samurai != null)
            {
                _context.Samurais.Remove(samurai);
                _context.SaveChanges();
            }
        }
        private static void QueryAndUpdateBattles_Disconnected()
        {
            List<Battle> disconnectedBattles;
            using (var context1 = new SamuraiContext())
            {
                disconnectedBattles = _context.Battles.ToList();
            }//context1 is disposed
            disconnectedBattles.ForEach(b =>
            {
                b.StartDate = new DateTime(1570, 01, 01);
                b.EndDate = new DateTime(1570, 12, 1);
            });
            using (var context2 = new SamuraiContext())
            {
                context2.UpdateRange(disconnectedBattles);
                context2.SaveChanges();
            }
        }
    }

}
