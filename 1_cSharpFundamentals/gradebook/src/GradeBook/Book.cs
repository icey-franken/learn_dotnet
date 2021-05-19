using System;
using System.Collections.Generic;
using System.IO;

namespace GradeBook
{
    public delegate void GradeAddedDelegate(object sender, EventArgs args);

    public class NamedObject
    {
        public NamedObject(string name)
        {
            Name = name;
        }

        public string Name
        {
            get;
            set;
        }
    }

    public interface IBook
    {
        void AddGrade(double grade);
        Statistics GetStatistics();
        string Name { get; }
        event GradeAddedDelegate GradeAdded;
    }

    public abstract class Book : NamedObject, IBook
    {
        protected Book(string name) : base(name)
        {
        }

        public abstract event GradeAddedDelegate GradeAdded;

        // this is an abstract method - tells you I want all derived types from this base abstract class to have a method AddGrade
        public abstract void AddGrade(double grade);

        public abstract Statistics GetStatistics();
    }

    public class DiskBook : Book
    {
        public DiskBook(string name) : base(name)
        {
        }

        public override event GradeAddedDelegate GradeAdded;

        public override void AddGrade(double grade)
        {
            // using keywork guarantees that writer.Dispose() called at end of curlies
            using (var writer = File.AppendText($"{Name}.txt"))
            {
                writer.WriteLine(grade);
                if (GradeAdded != null)
                {
                    GradeAdded(this, new EventArgs());
                }
            }
        }

        public override Statistics GetStatistics()
        {
            var result = new Statistics();
            using (var reader = File.OpenText($"{Name}.txt"))
            {
                var line = reader.ReadLine();
                while (line != null)
                {
                    var number = double.Parse(line);
                    result.Add(number);
                    line = reader.ReadLine();
                }
            }
            // for (var index = 0; index < grades.Count; index++)
            // {
            //     result.Add(grades[index]);
            // }
            return result;
        }
    }


    public class InMemoryBook : Book
    {
        // define a constructor - no return type (void, etc)
        //: base() -> "accessing constructor on base class"
        public InMemoryBook(string name) : base(name)
        {
            grades = new List<double>();
            Name = name;
        }
        // define a method
        public override void AddGrade(double grade)
        {
            // validate grade
            if (grade <= 100 && grade >= 0)
            {
                // store in an object instantiated by this class  as a piece of state
                grades.Add(grade);
                if (GradeAdded != null) // "if someone is listening..."
                {
                    // we use 'this' because 'this' is the sender (object)
                    GradeAdded(this, new EventArgs());
                }
            }
            else
            {
                throw new ArgumentException($"Invalid {nameof(grade)}");
            }
        }

        // book.GradeAdded - delegate invoked whenever grade added
        public override event GradeAddedDelegate GradeAdded;


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

        public override Statistics GetStatistics()
        {
            var result = new Statistics();

            for (var index = 0; index < grades.Count; index++)
            {
                result.Add(grades[index]);
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
        // public string Name
        // {
        //     get
        //     {
        //         return name;
        //     }
        //     set
        //     {
        //         try
        //         {

        //             if (!string.IsNullOrEmpty(value))
        //             {
        //                 name = value;
        //             }
        //             else
        //             {
        //                 throw new FormatException("GradeBook must have a name.");
        //             }
        //         }
        //         catch (FormatException ex)
        //         {
        //             Console.WriteLine(ex.Message);
        //         }
        //     }
        // }
    }
}