namespace librarymenagment.Models
{
    public abstract class BaseModel
    {
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool Active { get; set; } = false;
    }
} 