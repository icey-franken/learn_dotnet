using CourseManager.API.DbContexts;
using CourseManager.API.Entities;
using CourseManager.API.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Xunit;

namespace CourseManager.API.Test
{
    public class AuthorRepositoryTests
    {
        // Method_State_ExpectedBehavior
        [Fact]
        public void GetAuthors_PageSizeIsThree_ReturnsThreeAuthors()
        {
            //Arrange

            //When we pass options in to the CourseContext constructor,
            //I believe we override what is set up in Startup (using MySQL provider)??
            //Either way - this ensures that our context uses the in memory db provider
            var options = new DbContextOptionsBuilder<CourseContext>().UseInMemoryDatabase($"CourseDatabaseForTesting{Guid.NewGuid()}").Options;
            using (var context = new CourseContext(options))
            {
                //seeding db
                //context.Countries.AddRange(
                //    new Entities.Country() { Id = "BE", Description = "Belgium" },
                //    new Entities.Country() { Id = "US", Description = "United States" }
                //);
                context.Authors.AddRange(
                    new Entities.Author() { FirstName = "Kevin", LastName = "Dockx", CountryId = "BE" },
                    new Entities.Author() { FirstName = "Gill", LastName = "Cleeren", CountryId = "BE" },
                    new Entities.Author() { FirstName = "Julie", LastName = "Lerman", CountryId = "BE" },
                    new Entities.Author() { FirstName = "Shawn", LastName = "Wildermuth", CountryId = "BE" },
                    new Entities.Author() { FirstName = "Deborah", LastName = "Kurata", CountryId = "US" }
                );

                context.SaveChanges();
            }

            using (var context = new CourseContext(options))
            {

                var authorRepository = new AuthorRepository(context);

                //Act
                var authors = authorRepository.GetAuthors(1, 3);

                //Assert
                Assert.Equal(3, authors.Count());
            }
        }


        [Fact]
        public void GetAuthor_EmptyGuid_ThrowsArgumentException()
        {
            var options = new DbContextOptionsBuilder<CourseContext>().UseInMemoryDatabase($"CourseDatabaseForTesting{Guid.NewGuid()}").Options;

            using (var context = new CourseContext(options))
            {
                var authorRepository = new AuthorRepository(context);

                //Action act = () => authorRepository.GetAuthor(Guid.Empty);
                void act() => authorRepository.GetAuthor(Guid.Empty);
                Assert.Throws<ArgumentException>(act);
            }
        }

        [Fact]
        public void AddAuthor_NullCountry_DefaultsToBelgium()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<CourseContext>().UseInMemoryDatabase($"CourseDatabaseForTesting{Guid.NewGuid()}").Options;

            using (var context = new CourseContext(options))
            {
                context.Countries.Add(new Entities.Country(){ Id = "BE", Description = "Belgium" });
                context.SaveChanges();
            }

            using (var context = new CourseContext(options))
            {
                var authorRepository = new AuthorRepository(context);
                var authorToAdd = new Author()
                {
                    FirstName = "Kevin",
                    LastName = "Dockx"
                };
                //Act
                authorRepository.AddAuthor(authorToAdd);
                authorRepository.SaveChanges();
            }

            using (var context = new CourseContext(options))
            {
                //Assert
                var authorRepository = new AuthorRepository(context);
                var addedAuthor = authorRepository.GetAuthors().Where(a => a.FirstName == "Kevin").FirstOrDefault();
                Assert.Equal("BE", addedAuthor.CountryId);
            }

        }
    }
}
