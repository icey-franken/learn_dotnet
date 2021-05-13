using System;
using System.Collections.Generic;

namespace GradeBook
{
    class Program
    {
        class Book 
        {
            
        }

        static void Main(string[] args)
        {
            var book = new Book()

            var grades = new List<double>() { 5, 7, 9, 5 };
            grades.Add(9);
            
            double total = 0;
            foreach (double number in grades)
            {
                total += number;
            }
            var average = total / grades.Count;
            Console.WriteLine($"The average grade is {average:N2}.");
            
            if (args.Length > 0)
            {
                Console.WriteLine($"Hello, {args[0]}!");
            }
            else
            {
                Console.WriteLine("Hello, stranger!");
            }
        }
    }
}
