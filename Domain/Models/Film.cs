using System;
using System.Collections.Generic;

namespace DomainModels.Models
{
    public class Film
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public ICollection<Genre> Genres { get; private set; }
        public ICollection<Subscription> Subscriptions { get; set; }
        public double Rating { get; private set; }
        public ICollection<Review> Reviews { get; private set; }
        public int ReviewsCount { get; private set; }

        private Film()
        {
            
        }

        public Film(string name, DateTime releaseDate, ICollection<Genre> genres, ICollection<Subscription> subscriptions)
        {
            Name = name;
            ReleaseDate = releaseDate;
            Genres = genres;
            Subscriptions = subscriptions;
            ReviewsCount = 0;
        }

        public void UpdateRating(Review review)
        {
            if (review.Film == this)
            {
                var ratingSum = Rating * ReviewsCount + review.Rating;
                ReviewsCount++;
                Rating = (double)ratingSum / ReviewsCount;
            }
        }

        public void RecalculateRating()
        {
            var reviewsCount = 0;
            var ratingSum = 0;
            foreach (var review in Reviews)
            {
                reviewsCount++;
                ratingSum += review.Rating;
            }
            ReviewsCount = ratingSum;
            if (reviewsCount == 0)
                Rating = Review.RatingIfNoReviews;
            Rating = (double)ratingSum / reviewsCount;
        }
    }
}