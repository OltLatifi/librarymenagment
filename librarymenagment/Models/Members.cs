using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace librarymenagment.Models
{
    public class Member : BaseModel
    {
        public int Id { get; set; } 

        public string FirstName { get; set; } 

        public string LastName { get; set; } 

        public DateTime DateOfBirth { get; set; } 

        public string Email { get; set; } 

        public string PhoneNumber { get; set; } 

        public string Address { get; set; } 

        public DateTime MembershipDate { get; set; } 

        public string MembershipType { get; set; } 

        public bool IsActive { get; set; } 

        public int MaxBorrowLimit { get; set; }

        public ICollection<Book> BorrowedBooks { get; set; }
    }
}