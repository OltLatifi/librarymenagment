namespace librarymenagment.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }

    [Required]
    public string Description { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Copies must be at least 1.")]
    public int Copies { get; set; }

    [Required(ErrorMessage = "Please select an author.")]
    [ForeignKey("Author")]
    public int AuthorId { get; set; }

    [Required(ErrorMessage = "Please select a category.")]
    [ForeignKey("Category")]
    public int CategoryId { get; set; }

    public virtual Author? Author { get; set; }
    public virtual Category? Category { get; set; }
}
