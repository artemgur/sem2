using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DomainModels
{
    public class Film
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public string LongDescription { get; set; }
        public string ShortDescription { get; set; }
        public string Info { get; set; }
        public decimal Rating { get; set; }
        
        //public ICollection<int> InFavoritesOfUsersId { get; set; }
        public ICollection<User> InFavoritesOfUsers { get; set; }
        public string Producer { get; set; }
        public string OriginalName { get; set; }
        public string Actors { get; set; }

        public string LogoImagePath { get; set; }
        public string BackgroundImagePath { get; set; }
        public string VideoPath { get; set; }
    }
}