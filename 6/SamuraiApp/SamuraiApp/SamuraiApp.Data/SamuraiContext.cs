using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SamuraiApp.Domain;
using System;
using System.Linq;

namespace SamuraiApp.Data
{
    public class SamuraiContext : DbContext
    {
        //here we have two constructors!!!! If opt is passed in, it'll use the second one. Otherwise it'll use the first constructor.
        public SamuraiContext()
        { }
        public SamuraiContext(DbContextOptions opt)
            : base(opt)
        { }

        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Battle> Battles { get; set; }
        public DbSet<SamuraiBattle> SamuraiBattles { get; set; }
        //public DbSet<Horse> Horses { get; set; }
        //public DbSet<SamuraiBattleStat> SamuraiBattleStats { get; set; }

        //public static readonly LoggerFactory MyConsoleLoggerFactory =
        //    new LoggerFactory(new[] {
        //        new ConsoleLoggerProvider((category, level)
        //            => category == DbLoggerCategory.Database.Command.Name
        //            && level == LogLevel.Information, true) });

        //this is now in startup.cs in our SamuraiAPI
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //basically, if an options builder is not passed in (configured), do this?
            //The options builder would be passed in from the constructor, above.
            //So, if we're hooked up to ASP.NET using dependency injection to instantiate our DbContext, this will be skipped.
            //Otherwise, if we're just testing, this db provider will be used instead.
            //if (!optionsBuilder.IsConfigured)
            //{
            optionsBuilder
                //.UseLoggerFactory(MyConsoleLoggerFactory)
                //.UseSqlServer("Data Source= (localdb)\\MSSQLLocalDB; Initial Catalog=SamuraiTestData");
                .UseSqlServer("Server = (localdb)\\MSSQLLocalDB; Database = SamuraiAppData; Trusted_Connection = True");
            //.EnableSensitiveDataLogging(true);

            optionsBuilder.LogTo(
                Console.WriteLine,
                new[] {
                        DbLoggerCategory.Database.Command.Name,
                    //DbLoggerCategory.Database.Transaction.Name
                },
                LogLevel.Information
            )
            .EnableSensitiveDataLogging();
            //}
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SamuraiBattle>()
                .HasKey(s => new { s.SamuraiId, s.BattleId });


            //why is this needed? We specified in the Entity class that these properties of DateTime exist.
            modelBuilder.Entity<Battle>().Property(b => b.StartDate).HasColumnType("Date");
            modelBuilder.Entity<Battle>().Property(b => b.EndDate).HasColumnType("Date");
            
            //modelBuilder.Entity<Samurai>()
            //    .HasMany(s => s.Battles)
            //    .WithMany(b => b.Samurais)
            //    .UsingEntity<BattleSamurai>
            //    (
            //        bs => bs.HasOne<Battle>().WithMany(),
            //        bs => bs.HasOne<Samurai>().WithMany()
            //    )
            //    .Property(bs => bs.DateJoined)
            //    .HasDefaultValueSql("getdate()");

            //modelBuilder.Entity<SamuraiBattleStat>()
            //    .HasNoKey()
            //    .ToView("SamuraiBattleStats");

            //modelBuilder.Entity<Samurai>()
            //    .HasOne(s => s.SecretIdentity)
            //    .WithOne(si => si.Samurai).HasForeignKey<SecretIdentity>("SamuraiId");
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            var timestamp = DateTime.Now;
            foreach (var entry in ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                entry.Property("LastModified").CurrentValue = timestamp;

                if (entry.State == EntityState.Added)
                {
                    entry.Property("Created").CurrentValue = timestamp;
                }
            }
            return base.SaveChanges();
        }
    }
}

