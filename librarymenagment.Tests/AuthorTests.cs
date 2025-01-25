using Xunit;
using System;
using librarymenagment.Models;
using librarymenagment.Controllers;
using librarymenagment.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace librarymenagment.Tests
{
    public class AuthorTests
    {
        [Fact]
        public void CreateAuthor_WithValidData_ShouldSucceed()
        {
            // Arrange
            var author = new Author
            {
                Name = "John",
                LastName = "Doe",
                Active = true,
                CreatedAt = DateTime.Now
            };

            // Assert
            Assert.NotNull(author);
            Assert.Equal("John", author.Name);
            Assert.Equal("Doe", author.LastName);
            Assert.True(author.Active);
        }

        [Theory]
        [InlineData("", "Doe")]
        [InlineData(null, "Doe")]
        [InlineData("John", "")]
        [InlineData("John", null)]
        public void CreateAuthor_WithInvalidNameOrLastName_ShouldThrowException(string name, string lastName)
        {
            // Assert
            Assert.Throws<ArgumentException>(() => new Author 
            { 
                Name = name, 
                LastName = lastName 
            });
        }

        [Fact]
        public void DeactivateAuthor_ShouldSetActiveToFalse()
        {
            // Arrange
            var author = new Author
            {
                Name = "John",
                LastName = "Doe",
                Active = true,
                CreatedAt = DateTime.Now
            };

            // Act
            author.Active = false;
            author.UpdatedAt = DateTime.Now;

            // Assert
            Assert.False(author.Active);
            Assert.NotNull(author.UpdatedAt);
        }
    }
} 