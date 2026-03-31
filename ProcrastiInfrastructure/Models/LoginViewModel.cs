using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ProcrastiInfrastructure.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Чого ти пошту не ввів? Ану бігом!")]
        [EmailAddress(ErrorMessage = "Це не схоже на пошту.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Де пароль? Він обов'язковий!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
