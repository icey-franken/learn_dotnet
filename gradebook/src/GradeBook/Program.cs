using System;
using System.Collections.Generic;

namespace GradeBook
{
    class Program
    {

        static void Main(string[] args)
        {
            var book = new Book("Isaac's grade book");
            book.GradeAdded += OnGradeAdded;

            while (true)
            {
                Console.WriteLine("Enter a grade or 'q' to quit.");
                var input = Console.ReadLine();
                if (input == "q")
                {

                    break;
                }
                try
                {
                    var grade = double.Parse(input);
                    book.AddGrade(grade);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    Console.WriteLine("Thanks, teach!");
                }
            }

            var stats = book.GetStatistics();

            Console.WriteLine($"The average grade is {stats.Average:N2}.");
            Console.WriteLine($"The highest grade is {stats.High:N1}.");
            Console.WriteLine($"The lowest grade is {stats.Low:N1}.");
            Console.WriteLine($"The letter is {stats.Letter}");

        }

        static void OnGradeAdded(object sender, EventArgs e)
        {
            System.Console.WriteLine("A grade was added");
        }
    }
}
