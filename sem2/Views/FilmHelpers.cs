using DomainModels;
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
                Actors = film.Actors.Split(", ")
            };
        }

        public static string LogoImagePath(int filmId)
        {
            return "~/film_logos/logo" + filmId + ".png";
        }
        
        public static string BackgroundImagePath(int filmId)
        {
            return "~/film_backgrounds/img" + filmId + ".png";
        }
    }
}