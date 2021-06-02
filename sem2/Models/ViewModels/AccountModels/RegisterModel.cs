using System.ComponentModel.DataAnnotations;

namespace sem2.Models.ViewModels.AccountModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Не указан email адрес")]
        [EmailAddress(ErrorMessage = "Не корректный email адрес")]
        [MaxLength(64)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Не указано имя")]
        [MaxLength(64)]
        public string FirstName { get; set; }
        
        [Required(ErrorMessage = "Не указана фамилия")]
        [MaxLength(64)]
        public string Surname { get; set; }
        
        [Required(ErrorMessage = "Не указан пароль")]
        [MaxLength(64)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "Подтвердите пароль")]
        [MaxLength(64)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}