using System.ComponentModel.DataAnnotations;

namespace sem2.Models.ViewModels
{
    public class PasswordResetModel
    {
        [Required(ErrorMessage = "Не указан пароль")]
        [MaxLength(64)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        
        [Required(ErrorMessage = "Подтвердите пароль")]
        [MaxLength(64)]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }

        public string Email { get; set; }
        public string Key { get; set; }
    }
}