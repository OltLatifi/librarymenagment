namespace librarymenagment.Models;
using System.ComponentModel.DataAnnotations;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }

    [Required]
    public string Description { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Copies must be at least 1.")]
    public int Copies { get; set; }

    [Required(ErrorMessage = "Please select an author.")]
    public int AuthorId { get; set; }

    [Required(ErrorMessage = "Please select a category.")]
    public int CategoryId { get; set; }

    public Author Author { get; set; }
    public Category Category { get; set; }
}
