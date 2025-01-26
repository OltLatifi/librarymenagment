using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace librarymenagment.Models
{
    public class Events : BaseModel
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

    }
}
