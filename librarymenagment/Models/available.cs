using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Data;

namespace librarymenagment.Models
{
    public class Available : BaseModel
    {

        public int Id { get; set; }

      
        public string Name { get; set; }
        public DateTime DataPorosise { get; set; }

        public string Statusi {  get; set; }

        

        public ICollection<Book> PublishedBooks { get; set; }

        public string Address { get; set; }

    }
}
