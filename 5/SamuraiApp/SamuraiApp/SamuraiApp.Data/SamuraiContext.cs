using Microsoft.EntityFrameworkCore;
using SamuraiApp.Domain;

namespace SamuraiApp.Data
{
    public class SamuraiContext : DbContext
    {
        public SamuraiContext(DbContextOptions<SamuraiContext> options) : base(options)
        {

        }
        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Battle> Battles { get; set; }
        public DbSet<Horse> Horses { get; set; }
        public DbSet<SamuraiBattleStat> SamuraiBattleStats { get; set; }

        //this is now in startup.cs in our SamuraiAPI
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder
        //        .UseSqlServer("Data Source= (localdb)\\MSSQLLocalDB; Initial Catalog=SamuraiAppData")
        //        .LogTo(
        //            Console.WriteLine,
        //            new[] {
        //                DbLoggerCategory.Database.Command.Name,
        //                DbLoggerCategory.Database.Transaction.Name
        //            },
        //            LogLevel.Information
        //        )
        //        .EnableSensitiveDataLogging();
        //}

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

