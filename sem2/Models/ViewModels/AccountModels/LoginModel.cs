using System.ComponentModel.DataAnnotations;

namespace sem2.Models.ViewModels.AccountModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Не указан email адрес")]
        [MaxLength(64)]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Не указан пароль")]
        [MaxLength(64)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        public string RememberMe { get; set; }
    }
}