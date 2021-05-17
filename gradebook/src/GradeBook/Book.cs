using System;
using System.Collections.Generic;

namespace GradeBook
{
    public class Book
    {
        // define a constructor - no return type (void, etc)
        public Book(string name)
        {
            grades = new List<double>();
            Name = name;
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

        public Statistics GetStatistics()
        {
            var result = new Statistics();
            result.Low = double.MaxValue;
            result.High = double.MinValue;
            result.Average = 0.0;

            foreach (double grade in grades)
            {
                result.Low = Math.Min(grade, result.Low);
                result.High = Math.Max(grade, result.High);
                result.Average += grade;
            }
            result.Average /= grades.Count;
            return result;
        }

        // define a field - CANNOT use implicit typing (var)
        private List<double> grades;
        public string Name;

    }
}