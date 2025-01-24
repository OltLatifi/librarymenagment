using Xunit;
using System;
using librarymenagment.Models;
using librarymenagment.Controllers;
using librarymenagment.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace librarymenagment.Tests
{
    public class CategoryTests
    {
        [Fact]
        public void CreateCategory_WithValidData_ShouldSucceed()
        {
            // Arrange
            var category = new Category
            {
                Name = "Fiction",
                Active = true,
                CreatedAt = DateTime.Now
            };

            // Assert
            Assert.NotNull(category);
            Assert.Equal("Fiction", category.Name);
            Assert.True(category.Active);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("   ")]
        public void CreateCategory_WithInvalidName_ShouldThrowException(string invalidName)
        {
            // Assert
            Assert.Throws<ArgumentException>(() => new Category { Name = invalidName });
        }

        [Fact]
        public void DeactivateCategory_ShouldSetActiveToFalse()
        {
            // Arrange
            var category = new Category
            {
                Name = "Fiction",
                Active = true,
                CreatedAt = DateTime.Now
            };

            // Act
            category.Active = false;
            category.UpdatedAt = DateTime.Now;

            // Assert
            Assert.False(category.Active);
            Assert.NotNull(category.UpdatedAt);
        }

        [Fact]
        public void UpdateCategoryName_ShouldUpdateNameAndTimestamp()
        {
            // Arrange
            var category = new Category
            {
                Name = "Original Name",
                Active = true,
                CreatedAt = DateTime.Now
            };

            // Act
            var newName = "Updated Name";
            category.Name = newName;
            category.UpdatedAt = DateTime.Now;

            // Assert
            Assert.Equal(newName, category.Name);
            Assert.NotNull(category.UpdatedAt);
        }

        [Fact]
        public void Category_ShouldInheritFromBaseModel()
        {
            // Arrange
            var category = new Category();

            // Assert
            Assert.True(category is BaseModel);
        }
    }
} 