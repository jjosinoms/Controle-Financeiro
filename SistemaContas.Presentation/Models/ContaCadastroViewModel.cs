using Microsoft.AspNetCore.Authentication;
using SistemaContas.Data.Entities;
using SistemaContas.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace SistemaContas.Presentation.Models
{
    public class ContaCadastroViewModel
    {
        [MinLength(8, ErrorMessage = "Por favor, informe no mínimo {1} caracteres.")]
        [MaxLength(150, ErrorMessage = "Por favor, informe no máximo {1} caracteres.")]
        [Required(ErrorMessage = "Por favor, informe o nome da categoria.")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "Por favor, informe o nome da categoria.")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "Por favor, informe o nome da categoria.")]
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "Por favor, informe o nome da categoria.")]
        public TipoConta Tipo { get; set; }
        [MinLength(8, ErrorMessage = "Por favor, informe no mínimo {1} caracteres.")]
        [MaxLength(150, ErrorMessage = "Por favor, informe no máximo {1} caracteres.")]
        [Required(ErrorMessage = "Por favor, informe o nome da categoria.")]
        public string? Observacoes { get; set; }
        /*
        public Guid IdCategoria { get; set; }
        public Guid IdUsuario { get; set; }
        public Usuario Usuario { get; set; }
        public Categoria Categoria { get; set; }
        */
    }
}
