using Xunit;
using System;
using librarymenagment.Models;
using librarymenagment.Controllers;
using librarymenagment.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace librarymenagment.Tests
{
    public class CommentTests
    {
        [Fact]
        public void CreateComment_WithValidData_ShouldSucceed()
        {
            // Arrange
            var comment = new Coments
            {
                Description = "Great book!",
                UserId = "1",
                BookId = 1,
                CreatedAt = DateTime.Now,
                Active = true
            };

            // Assert
            Assert.NotNull(comment);
            Assert.Equal("Great book!", comment.Description);
            Assert.Equal("1", comment.UserId);
            Assert.Equal(1, comment.BookId);
            Assert.True(comment.Active);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void CreateComment_WithInvalidDescription_ShouldThrowException(string invalidDescription)
        {
            // Assert
            Assert.Throws<ArgumentException>(() => new Coments { Description = invalidDescription });
        }
    }
} 