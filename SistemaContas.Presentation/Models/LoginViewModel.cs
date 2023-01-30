using System.ComponentModel.DataAnnotations;

namespace SistemaContas.Presentation.Models
{
    public class LoginViewModel
    {
        [EmailAddress(ErrorMessage = "Por favor, informe um endereço de email válido.")]
        [Required(ErrorMessage = "Por favor, informe seu E-mail.")]
        public string? Email { get; set; }

        [MinLength(8, ErrorMessage = "Por favor, insira no mínimo {1} caracteres")]
        [MaxLength(20, ErrorMessage = "Por favor, insira no máximo {1} caracteres")]
        [Required(ErrorMessage = "Por favor, informe sua Senha.")]
        public string? Senha { get; set; }

    }
}
