using Microsoft.EntityFrameworkCore;
using SamuraiApp.Domain;

namespace SamuraiApp.Data
{
    public class SamuraiContext : DbContext
    {
        //the constructor was only needed when hooking EF Core's DbContext up to our ASP.NET controller.
        //public SamuraiContext(DbContextOptions<SamuraiContext> options) : base(options)
        //{

        //}

        //here we have two constructors!!!! If opt is passed in, it'll use the second one. Otherwise it'll use the first constructor.
        public SamuraiContext()
        { }
        public SamuraiContext(DbContextOptions opt)
            : base(opt)
        { }

        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Battle> Battles { get; set; }
        public DbSet<Horse> Horses { get; set; }
        public DbSet<SamuraiBattleStat> SamuraiBattleStats { get; set; }

        //this is now in startup.cs in our SamuraiAPI
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //basically, if an options builder is not passed in (configured), do this?
            //The options builder would be passed in from the constructor, above.
            //So, if we're hooked up to ASP.NET using dependency injection to instantiate our DbContext, this will be skipped.
            //Otherwise, if we're just testing, this db provider will be used instead.
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer("Data Source= (localdb)\\MSSQLLocalDB; Initial Catalog=SamuraiTestData");
                //.LogTo(
                //    Console.WriteLine,
                //    new[] {
                //        DbLoggerCategory.Database.Command.Name,
                //        DbLoggerCategory.Database.Transaction.Name
                //    },
                //    LogLevel.Information
                //)
                //.EnableSensitiveDataLogging();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Samurai>()
                .HasMany(s => s.Battles)
                .WithMany(b => b.Samurais)
                .UsingEntity<BattleSamurai>
                (
                    bs => bs.HasOne<Battle>().WithMany(),
                    bs => bs.HasOne<Samurai>().WithMany()
                )
                .Property(bs => bs.DateJoined)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<SamuraiBattleStat>()
                .HasNoKey()
                .ToView("SamuraiBattleStats");
        }
    }
}

