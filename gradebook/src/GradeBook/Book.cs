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
            System.Console.WriteLine("asdf");
            // validate grade
            if (grade <= 100 && grade >= 0)
            {
                // store in an object instantiated by this class  as a piece of state
                grades.Add(grade);
            }
            else
            {
                System.Console.WriteLine("asd; f");
                throw new ArgumentException($"Invalid {nameof(grade)}");
            }
        }


        public void AddLetterGrade(char letter)
        {
            switch (letter)
            {
                case 'A':
                    AddGrade(90);
                    break;
                case 'B':
                    AddGrade(80);
                    break;
                case 'C':
                    AddGrade(70);
                    break;
                case 'D':
                    AddGrade(60);
                    break;
                default:
                    AddGrade(0);
                    break;
            }
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

            // foreach (double grade in grades)
            // {
            //     result.Low = Math.Min(grade, result.Low);
            //     result.High = Math.Max(grade, result.High);
            //     result.Average += grade;
            // }
            // result.Average /= grades.Count;
            // return result;


            // var index = 0;
            // while (index < grades.Count)
            // {
            //     result.Low = Math.Min(grades[index], result.Low);
            //     result.High = Math.Max(grades[index], result.High);
            //     result.Average += grades[index];
            //     index++;
            // };
            // result.Average /= grades.Count;
            // return result;

            for (var index = 0; index < grades.Count; index++)
            {
                result.Low = Math.Min(grades[index], result.Low);
                result.High = Math.Max(grades[index], result.High);
                result.Average += grades[index];
            }
            result.Average /= grades.Count;

            switch (result.Average)
            {
                case var d when d >= 90.0:
                    result.Letter = 'A';
                    break;
                case var d when d >= 80.0:
                    result.Letter = 'B';
                    break;
                case var d when d >= 70.0:
                    result.Letter = 'C';
                    break;
                case var d when d >= 60.0:
                    result.Letter = 'D';
                    break;
                default:
                    result.Letter = 'F';
                    break;
            }

            return result;


        }

        // define a field - CANNOT use implicit typing (var)
        public List<double> grades;

        public List<double> Grades
        {
            get
            {
                return grades;
            }
            set
            {
                Grades = grades;
            }
        }

        // the FIELD name
        public string name;

        // the PROPERTY Name
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                try
                {

                    if (!string.IsNullOrEmpty(value))
                    {
                        name = value;
                    }
                    else
                    {
                        throw new FormatException("GradeBook must have a name.");
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}