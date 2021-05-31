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
        public string[] Actors;

        public int LogoId;
        public ImageMetadata Logo;

        public int BackgroundId;
        public ImageMetadata Background;
    }
}