using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace sem2.Models.ViewModels.AdminPanelModels
{
    public class CreateFilmModel
    {
        [Required]
        [MaxLength(64)]
        public string Name { get; set; }
        
        [Required]
        [MaxLength(64)]
        public string OriginalName { get; set; }
        
        [Required]
        [MaxLength(256)]
        public string ShortDescription { get; set; }
        
        [Required]
        [MaxLength(512)]
        public string LongDescription { get; set; }

        [Required]
        [MaxLength(64)]
        public string Producer { get; set; }
        
        [Required]
        [MaxLength(256)]
        public string Actors { get; set; }
        
        public IFormFile LogoImage { get; set; }
        public IFormFile BackgroundImage { get; set; }
        public IFormFile Video { get; set; }

    }
}