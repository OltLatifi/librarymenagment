using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace librarymenagment.Models
{
    public class PublishingHouse : BaseModel
    {

        public int Id { get; set; }

        public DateTime FoundedDate { get; set; }
        public string Name { get; set; }

        public ICollection<Author> Authors { get; set; }

        public ICollection<Book> PublishedBooks { get; set; }

        public string Address { get; set; }

    }
}
