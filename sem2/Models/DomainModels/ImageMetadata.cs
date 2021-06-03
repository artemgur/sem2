using System.ComponentModel.DataAnnotations;

namespace DomainModels
{
    public class ImageMetadata
    {
        [Key]
        public int Id { get; set; }

        public string ImagePath { get; set; }
        public string ContentType { get; set; } = "application/octet-stream";
    }
}