using Xunit;
using System;

namespace librarymenagment.Tests
{
    public class BookTests
    {
        [Fact]
        public void Book_Creation_WithValidData_ShouldSucceed()
        {
            // Arrange
            var book = new Book
            {
                Title = "Test Book",
                Author = "Test Author",
                PublicationYear = 2024,
                Available = true,
            };

            // Assert
            Assert.NotNull(book);
            Assert.Equal("Test Book", book.Title);
            Assert.Equal("Test Author", book.Author);
            Assert.Equal(2024, book.PublicationYear);
            Assert.True(book.Available);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void Book_Creation_WithInvalidTitle_ShouldThrowException(string invalidTitle)
        {
            // Assert
            Assert.Throws<ArgumentException>(() => new Book { Title = invalidTitle });
        }
    }
}