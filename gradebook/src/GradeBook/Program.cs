using System;
using System.Collections.Generic;

namespace GradeBook
{
    class Program
    {

        static void Main(string[] args)
        {
            var book = new Book("Isaac's grade book");
            book.AddGrade(89.1);
            book.AddGrade(0.69);
            book.AddGrade(420);
            double[] grades = { 10.0, 20.0, 30.0 };
            book.AddGrades(grades);

            var stats = book.GetStatistics();

            Console.WriteLine($"The average grade is {stats.Average:N2}.");
            Console.WriteLine($"The highest grade is {stats.High:N1}.");
            Console.WriteLine($"The lowest grade is {stats.Low:N1}.");

        }
    }
}
