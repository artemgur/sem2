using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace sem2.Models.ViewModels.AdminPanelModels
{
    public class CreateSubscriptionModel
    {
        [Required]
        [MaxLength(64)]
        public string Name { get; set; }
        [Required]
        [MaxLength(256)]
        public string Description { get; set; }
        [Required]
        [Range(0, 1000000000)]
        public decimal Price { get; set; }
        
        [Required]
        [Range(1, 10000)]
        public int Duration { get; set; }
        
        [Required]
        public List<string> Permissions { get; set; }
        
        public IFormFile LogoImage { get; set; }
    }
}