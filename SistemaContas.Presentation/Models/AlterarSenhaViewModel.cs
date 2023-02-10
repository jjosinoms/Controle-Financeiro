﻿using System.ComponentModel.DataAnnotations;

namespace SistemaContas.Presentation.Models
{
    public class AlterarSenhaViewModel
    {
        [Required(ErrorMessage = "Por favor, informa a nova senha.")]
        [MaxLength(20, ErrorMessage = "Por favor, informe no máximo {1} caracteres.")]
        [MinLength(8, ErrorMessage = "Por favor, informe no mínimo {1} caracteres.")]
        public string? NovaSenha { get; set; }
        [Compare("NovaSenha", ErrorMessage = "Senhas não conferem.")]
        [Required(ErrorMessage = "Por favor, confirme a nova senha.")]
        public string? NovaSenhaConfirmacao { get; set; }
    }
}
