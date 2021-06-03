using System.Linq;
using DomainModels;
using Org.BouncyCastle.Asn1.Cmp;
using sem2.Models;

namespace sem2.Views
{
    public static class FilmHelpers
    {
        public static FilmDTO FromFilm(Film film)
        {
            return new FilmDTO
            {
                Id = film.Id,
                Info = film.Info,
                Name = film.Name,
                Producer = film.Producer,
                LongDescription = film.LongDescription,
                OriginalName = film.OriginalName,
                ShortDescription = film.ShortDescription,
                Actors = film.Actors.Split(", "),
                LogoImagePath = film.LogoImagePath,
                BackgroundImagePath = film.BackgroundImagePath,
                Rating = film.Rating
            };
        }
        
        public static FilmDTO FromFilm(Film film, bool isInFavorites)
        {
            return new FilmDTO
            {
                Id = film.Id,
                Info = film.Info,
                Name = film.Name,
                Producer = film.Producer,
                LongDescription = film.LongDescription,
                OriginalName = film.OriginalName,
                ShortDescription = film.ShortDescription,
                Actors = film.Actors.Split(", "),
                LogoImagePath = film.LogoImagePath,
                BackgroundImagePath = film.BackgroundImagePath,
                Rating = film.Rating,
                IsInFavorites = isInFavorites
            };
        }

        
        public static string FilmImagePath(int filmId)
        {
            return "films/" + filmId + ".mp4";
        }    
    }
}