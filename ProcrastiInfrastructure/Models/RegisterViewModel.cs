using System.ComponentModel.DataAnnotations;

namespace ProcrastiInfrastructure.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Не залишай пустим ім'я, будь ласка.")]
        [MinLength(3, ErrorMessage = "Ім'я має бути не менше 3 символів.")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Без пошти не цікаво жити, введи її.")]
        [EmailAddress(ErrorMessage = "Це не схоже на пошту.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Треба пароль, дуже треба.")]
        [MinLength(6, ErrorMessage = "Пароль має бути не менше 6 символів.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
