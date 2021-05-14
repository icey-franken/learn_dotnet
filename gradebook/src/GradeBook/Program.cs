using System;
using System.Collections.Generic;

namespace GradeBook
{
    class Program
    {

        static void Main(string[] args)
        {
            var book = new Book();
            book.AddGrade(89.1);
            book.AddGrade(0.69);
            book.AddGrade(420);
            double[] grades = { 10.0, 20.0, 30.0 };
            book.AddGrades(grades);
            book.ShowStats();
        }
    }
}
