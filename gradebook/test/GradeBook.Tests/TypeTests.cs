using System;
using Xunit;

namespace GradeBook.Tests
{

    // delegate e.g. to log messages
    // the delegate below describes any method that returns a string and takes a string
    public delegate string WriteLogDelegate(string logMessage);

    public class TypeTests
    {

        int count = 0;

        [Fact]
        public void WriteLogDelegateCanPointToMethod()
        {
            WriteLogDelegate log = new WriteLogDelegate(ReturnMessage);
            log += ReturnMessage;
            log += IncrementCount;
            // log = new WriteLogDelegate(ReturnMessage);
            // equivalent to log = ReturnMessage //compiler is smart enough to figure this out

            var result = log("Hello");
            Assert.Equal("hello", result);
            Assert.Equal(3, count);
        }

        // dummy function that returns a string and takes a string
        string ReturnMessage(string message)
        {
            count++;
            return message;
        }

        string IncrementCount(string message)
        {
            count++;
            return message.ToLower();
        }

        [Fact]
        public void Test1()
        {
            var x = GetInt();
            SetInt(ref x);

            Assert.Equal(42, x);
        }

        private void SetInt(ref int x)
        {
            x = 42;
        }

        private int GetInt()
        {
            return 3;
        }

        [Fact]
        public void GetBookReturnsDifferentObjects()
        {
            // arrange
            var book1 = GetBook("Book 1");
            var book2 = GetBook("Book 2");

            // act

            // assert
            Assert.Equal("Book 1", book1.Name);
            Assert.Equal("Book 2", book2.Name);
            Assert.NotSame(book1, book2);
        }


        [Fact]
        public void TwoVarsCanReferenceSameObject()
        {
            // arrange
            var book1 = GetBook("Book 1");
            var book2 = book1;

            // act

            // assert
            Assert.Equal("Book 1", book1.Name);
            Assert.Equal("Book 1", book2.Name);
            Assert.True(object.ReferenceEquals(book1, book2));
            Assert.Same(book1, book2);
        }

        private Book GetBook(string name)
        {
            return new Book(name);
        }


        [Fact]
        public void CanSetNameFromReference()
        {
            // arrange
            var book1 = GetBook("Book 1");
            SetName(book1, "New Name");

            // act

            // assert
            Assert.Equal("New Name", book1.Name);
        }

        private void SetName(Book book, string name)
        {
            book.Name = name;
        }


        [Fact]
        public void CSharpIsPassByValue()
        {
            // arrange
            var book1 = GetBook("Book 1");
            GetBookSetName(book1, "New Name");

            // act

            // assert
            Assert.Equal("Book 1", book1.Name);
        }

        private void GetBookSetName(Book book, string name)
        {
            book = new Book(name);
        }

        [Fact]
        public void CSharpCanPassByReference()
        {
            // arrange
            var book1 = GetBook("Book 1");
            GetBookSetNameByReference(ref book1, "New Name");

            // act

            // assert
            Assert.Equal("New Name", book1.Name);
        }

        private void GetBookSetNameByReference(ref Book book, string name)
        {
            book = new Book(name);
        }

        [Fact]
        public void StringsBehaveLikeValueTypes()
        {
            string name = "Scoot";
            var upperName = MakeUpperCase(name);

            Assert.Equal("Scoot", name);
            Assert.Equal("SCOOT", upperName);
        }

        private string MakeUpperCase(string parameter)
        {
            return parameter.ToUpper();
        }
    }
}