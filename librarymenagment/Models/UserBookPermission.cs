namespace librarymenagment.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

public class UserBookPermission : BaseModel
{
    public int Id { get; set; }

    [ForeignKey("User")]
    public string? UserId { get; set; }

    [ForeignKey("Book")]
    public int BookId { get; set; }


    public virtual Book? Book { get; set; }
    public virtual IdentityUser? User { get; set; }
}