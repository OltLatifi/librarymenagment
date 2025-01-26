namespace librarymenagment.Models
{
    public class Loans : BaseModel
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int BookId { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
