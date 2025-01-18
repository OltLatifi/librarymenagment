using System.ComponentModel.DataAnnotations.Schema;

namespace librarymenagment.Models
{
    public class Author : BaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string? Bio { get; set; }

        [ForeignKey("PublishingHouse")]
        public int? PublishingHouseId { get; set; }
    }
}
