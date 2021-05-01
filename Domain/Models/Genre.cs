namespace DomainModels.Models
{
    public class Genre
    {
        public Film Film { get; set; }
        public GenreEnum GenreName { get; set; }
    }
}