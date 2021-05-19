using System;

namespace GradeBook
{
    public class Statistics
    {
        public double Average
        {
            get
            {
                return SumOfGrades / CountOfGrades;
            }
        }
        public double High;
        public double Low;
        public char Letter
        {
            get
            {
                switch (Average)
                {
                    case var d when d >= 90.0:
                        return 'A';
                    case var d when d >= 80.0:
                        return 'B';
                    case var d when d >= 70.0:
                        return 'C';
                    case var d when d >= 60.0:
                        return 'D';
                    default:
                        return 'F';
                }
            }
        }
        private int CountOfGrades;
        private double SumOfGrades;

        public Statistics()
        {
            High = double.MinValue;
            Low = double.MaxValue;
            SumOfGrades = 0.0;
            CountOfGrades = 0;
        }

        public void Add(double number)
        {
            SumOfGrades += number;
            CountOfGrades++;
            Low = Math.Min(number, Low);
            High = Math.Max(number, High);
        }

    }
}