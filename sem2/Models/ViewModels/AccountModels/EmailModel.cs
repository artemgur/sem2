using System.ComponentModel.DataAnnotations;

namespace sem2.Models.ViewModels.AccountModels
{
    public class EmailModel
    {
        [Required(ErrorMessage = "Не указан email адрес")]
        [EmailAddress(ErrorMessage = "Не корректный email адрес")]
        [MaxLength(64)]
        public string Email { get; set; }
    }
}