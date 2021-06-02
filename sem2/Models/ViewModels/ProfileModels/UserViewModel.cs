using System.ComponentModel.DataAnnotations;

namespace sem2.ViewModels.ProfileModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(64)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(64)]
        public string Surname { get; set; }
        public string ImagePath { get; set; }
        public string Email { get; set; }
    }
}