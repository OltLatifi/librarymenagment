using Xunit;
using System;
using librarymenagment.Models;
using librarymenagment.Controllers;
using librarymenagment.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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
                AuthorId = 1,
                CategoryId = 1,
                Active = true,
            };

            // Assert
            Assert.NotNull(book);
            Assert.Equal("Test Book", book.Title);
            Assert.Equal(1, book.AuthorId);
            Assert.Equal(1, book.CategoryId);
            Assert.True(book.Active);
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