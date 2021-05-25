using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DomainModels;

namespace sem2.DomainModels
{
    public class ApplicationUser
    {
        [Key]
        [DatabaseGenerated((DatabaseGeneratedOption.None))]
        public int Id { get; set; }

        public int ImageId { get; set; }
        public ImageMetadata Image { get; set; }

        public string FirstName { get; set; }
        public string Surname { get; set; }
    }
}