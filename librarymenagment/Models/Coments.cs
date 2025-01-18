namespace librarymenagment.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

public class Coments : BaseModel
{
    public int Id { get; set; }


    [Required]
    public string Description { get; set; }




    [Required(ErrorMessage = "Please select an user.")]
    [ForeignKey("User")]
    public int UserId { get; set; }


    [Required(ErrorMessage = "Please select a book.")]
    [ForeignKey("Book")]
    public int BookId { get; set; }


    public virtual Book? Book { get; set; }
}