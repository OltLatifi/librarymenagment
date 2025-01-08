namespace librarymenagment.Models
{
    public class Author
    {
        public int id { get; set; }
        public string name { get; set; }
        public string last_name { get; set; }
        public string? bio {  get; set; }
        public DateTime createdAt { get; set; }
        public DateTime? updatedAt { get; set; }

    }
}
