using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainModels
{
    public class User
    {
        public static ImageMetadata DefaultImage => new ImageMetadata()
        {
            ImagePath = "applicationData/profileImages/default.jpg",
            ContentType = "image/jpeg"
        };
        
        [Key]
        [DatabaseGenerated((DatabaseGeneratedOption.None))]
        public int Id { get; set; }

        public int ImageId { get; set; }
        public ImageMetadata Image { get; set; }

        public string Email { get; set; }
        public string FirstName { get; set; }
        public string Surname { get; set; }
        
        public ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
        public ICollection<Film> FavoriteFilms { get; set; }
    }
}