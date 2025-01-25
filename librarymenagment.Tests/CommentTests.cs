using Xunit;
using System;

namespace librarymenagment.Tests
{
    public class CommentTests
    {
        [Fact]
        public void CreateComment_WithValidData_ShouldSucceed()
        {
            // Arrange
            var comment = new Comment
            {
                Description = "Great book!",
                UserId = 1,
                BookId = 1,
                CreatedAt = DateTime.Now,
                Active = true
            };

            // Assert
            Assert.NotNull(comment);
            Assert.Equal("Great book!", comment.Description);
            Assert.Equal(1, comment.UserId);
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
            Assert.Throws<ArgumentException>(() => new Comment { Description = invalidDescription });
        }

        [Fact]
        public void DeactivateComment_ShouldSetActiveToFalse()
        {
            // Arrange
            var comment = new Comment
            {
                Description = "Test comment",
                UserId = 1,
                BookId = 1,
                CreatedAt = DateTime.Now,
                Active = true
            };

            // Act
            comment.Deactivate();

            // Assert
            Assert.False(comment.Active);
            Assert.NotNull(comment.UpdatedAt);
        }
    }
} 