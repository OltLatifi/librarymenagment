namespace librarymenagment.Models
{
    public abstract class BaseModel
    {
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public bool Active { get; set; } = false;
    }
} 