using System.Collections.Generic;
using DomainModels;

namespace sem2.Models
{
    public class FilmDTO
    {
        public int Id;
        public string Name;
        public string LongDescription;
        public string ShortDescription;
        public string Info;
        public string Producer;
        public string OriginalName;
        public decimal Rating;
        public string[] Actors;
        public string LogoImagePath;
        public string BackgroundImagePath;
        public bool IsInFavorites;
    }
}