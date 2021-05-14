using System;
using System.Collections.Generic;

namespace GradeBook
{
    class Book
    {
        // define a constructor - no return type (void, etc)
        public Book()
        {
            grades = new List<double>();
        }
        // define a method
        public void AddGrade(double grade)
        {
            // validate grade

            // store in an object instantiated by this class  as a piece of state
            grades.Add(grade);
        }

        public void AddGrades(double[] grades)
        {
            foreach (double grade in grades)
            {
                this.AddGrade(grade);
            }
        }

        public void ShowStats()
        {

            var total = 0.0;
            var highGrade = double.MinValue;
            var lowGrade = double.MaxValue;
            foreach (double number in grades)
            {
                lowGrade = Math.Min(number, lowGrade);
                highGrade = Math.Max(number, highGrade);
                total += number;
            }
            var average = total / grades.Count;
            Console.WriteLine($"The average grade is {average:N2}.\n The highest grade is {highGrade:N1}.\n The lowest grade is {lowGrade:N1}.");
        }

        // define a field - CANNOT use implicit typing (var)
        private List<double> grades;
        private string name;

    }
}