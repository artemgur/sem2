namespace DomainModels.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public Film Film { get; set; }
        public int Rating { get; set; }
    }
}