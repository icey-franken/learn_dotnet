using System;
using Xunit;
using System.Collections.Generic;

namespace GradeBook.Tests
{
    public class BookTests
    {
        [Fact]
        public void BookCalculatesAnAverageGrade()
        {
            // arrange
            var book = new Book("");
            book.AddGrade(89.1);
            book.AddGrade(90.5);
            book.AddGrade(77.3);

            // act
            var result = book.GetStatistics();

            // assert
            Assert.Equal(85.6, result.Average, 1);
            Assert.Equal(90.5, result.High, 1);
            Assert.Equal(77.3, result.Low, 1);
            Assert.Equal('B', result.Letter);
        }

        [Fact]
        public void ValidGradeEntriesAreAdded()
        {
            // arrange
            var book = new Book("");
            var dummyList = new List<double>();
            dummyList.Add(50);
            // act
            book.AddGrade(50);
            // assert
            Assert.Equal(book.grades, dummyList);
        }

        [Fact]
        public void InvalidGradeEntriesThrowArgumentException()
        {
            // arrange
            var book = new Book("");

            // act

            // assert
            Assert.Throws<ArgumentException>(() => book.AddGrade(1000));
            Assert.Throws<ArgumentException>(() => book.AddGrade(-1));
        }
    }
}
