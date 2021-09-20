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
            ModifyingRelatedDataWhenNotTracked();
        }

        private static void AddSamuraisByName(params string[] names)
        {
            foreach (string name in names)
            {
                _context.Samurais.Add(new Samurai { Name = name });
            }
            _context.SaveChanges();
        }
        private static void AddBattlesByName(params string[] names)
        {
            foreach (string name in names)
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
            foreach (var samurai in samurais)
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
            var samurai = _context.Samurais.FirstOrDefault(s => s.Id == 19);
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
        private static void InsertNewSamuraiWithAQuote()
        {
            var samurai = new Samurai
            {
                Name = "Kambei Shimada",
                Quotes = new List<Quote>
                {
                    new Quote { Text = "I've come to save you" }
                }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }
        private static void InsertNewSamuraiWithManyQuotes()
        {
            var samurai = new Samurai
            {
                Name = "Kyuzo",
                Quotes = new List<Quote>
                {
                    new Quote { Text = "Watch out for my sharp sword!" },
                    new Quote { Text = "I told you to watch out for the sharp sword! Oh well!"}
                }
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }
        private static void AddQuoteToExistingSamuraiWhileTracked()
        {
            var samurai = _context.Samurais.Skip(1).FirstOrDefault();
            samurai.Quotes.Add(new Quote
            {
                Text = "I bet you're happy that I've saved you!"
            });
            _context.SaveChanges();
        }
        private static void AddQuoteToExistingSamuraiNotTracked(int samuraiId)
        {
            var samurai = _context.Samurais.Find(samuraiId);
            samurai.Quotes.Add(new Quote
            {
                Text = "Now that I saved you, will you feed me dinner?"
            });
            using (var newContext = new SamuraiContext())
            {
                newContext.Samurais.Attach(samurai);
                newContext.SaveChanges();
            };
        }
        private static void Simpler_AddQuoteToExistingSamuraiNotTracked(int samuraiId)
        {
            var quote = new Quote { Text = "Thanks for dinner!", SamuraiId = samuraiId };
            using var newContext = new SamuraiContext();
            newContext.Quotes.Add(quote);
            newContext.SaveChanges();
        }
        private static void EagerLoadSamuraiWithQuotes()
        {
            //var samuraiWithQuotes = _context.Samurais.Include(s => s.Quotes).ToList();
            //var splitQuery = _context.Samurais.AsSplitQuery().Include(s => s.Quotes).ToList();
            var filteredInclude = _context.Samurais
                .Include(s => s.Quotes.Where(q => q.Text.Contains("hanks")))
                .Where(s => s.Name.Contains("Julie"))
                .Where(s => s.Quotes.Count > 0).ToList();
            var filterPrimaryEntityWithInclude =
                _context.Samurais.Where(s => s.Name.Contains("Sampson"))
                .Include(s => s.Quotes).FirstOrDefault();
        }
        private static void ProjectSomeProperties()
        {
            var someProperties = _context.Samurais.Select(s => new IdName(s.Id, s.Name)).ToList();
        }
        public struct IdName
        {
            public IdName(int id, string name)
            {
                Id = id;
                Name= name;
            }
            public int Id;
            public string Name;
        }
        private static void ProjectSamuraisWithQuotes()
        {
            var somePropsWithQuotes = _context.Samurais
                .Select(s => new IdNameQuotes(s.Id, s.Name, s.Quotes.Count))
                .ToList();
            var samuraisAndQuotes = _context.Samurais
                .Select(s => new { 
                    Samurai = s, 
                    HappyQuotes = s.Quotes.Where(q => q.Text.Contains("happy")) 
                }).ToList();
            var firstSamurai = samuraisAndQuotes[0].Samurai.Name += " The Happiest";
            _context.SaveChanges();
        }
        public struct IdNameQuotes
        {
            public IdNameQuotes(int id, string name, int numberOfQuotes)
            {
                Id = id;
                Name = name;
                NumberOfQuotes = numberOfQuotes;
            }
            public int Id;
            public string Name;
            public int NumberOfQuotes;
        }
        private static void ExplicitLoadQuotes()
        {
            var samurai = _context.Samurais.Find(1);
            _context.Entry(samurai).Collection(s => s.Quotes).Load();
            _context.Entry(samurai).Reference(s => s.Horse).Load();
        }
        private static void FilteringWithRelatedData()
        {
            var samurais = _context.Samurais
                .Where(s => s.Quotes.Any(q => q.Text.Contains("happy")))
                //.Include(s => s.Quotes)
                .ToList();
        }
        private static void ModifyingRelatedDataWhenTracked()
        {
            var samurai = _context.Samurais.Include(s => s.Quotes)
                .FirstOrDefault(s => s.Id == 1);
            samurai.Quotes[0].Text = "Did you hear that?";
            _context.SaveChanges();
        }
        private static void ModifyingRelatedDataWhenNotTracked()
        {
            var samurai = _context.Samurais.Include(s => s.Quotes)
                .FirstOrDefault(s => s.Id == 2);
            var quote = samurai.Quotes[0];
            quote.Text += "Did you hear that again?";

            using var newContext = new SamuraiContext();
            //newContext.Quotes.Update(quote);
            newContext.Entry(quote).State = EntityState.Modified;
            newContext.SaveChanges();
        }
    }
}
