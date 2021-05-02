namespace DomainModels.Models
{
    public class Review
    {
        public const int MinRating = 1;
        public const int MaxRating = 5;
        public const int RatingIfNoReviews = -1;
        
        public int Id { get; private set; }
        public string Content { get; private set; }
        public Film Film { get; private set; }
        public int Rating { get; private set; }

        private Review()
        {
            
        }
        
        public static Review CreateReview(string content, Film film, int rating)
        {
            if (MinRating > rating || MaxRating < rating)
                return null;
            return new Review
            {
                Content = content,
                Film = film,
                Rating = rating
            };
        }

    }
}