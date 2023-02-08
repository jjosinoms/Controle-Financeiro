
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaContas.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace SistemaContas.Presentation.Models
{
    public class ContaEdicaoViewModel
    {
        public Guid Id { get; set; } //campo oculto

        [MinLength(8, ErrorMessage = "Por favor, informe no mínimo {1} caracteres.")]
        [MaxLength(150, ErrorMessage = "Por favor, informe no máximo {1} caracteres.")]
        [Required(ErrorMessage = "Por favor, informe o nome da conta.")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "Por favor, informe o nome da conta.")]
        public decimal? Valor { get; set; }

        [Required(ErrorMessage = "Por favor, informe o Valor da conta.")]
        public string? Data { get; set; }

        [Required(ErrorMessage = "Por favor, informe o data da conta.")]
        public int? Tipo { get; set; }

        [Required(ErrorMessage = "Por favor, informe as observações da conta.")]
        public string? Observacoes { get; set; }

        [Required(ErrorMessage = "Por favor, informe a categoria da conta.")]
        public Guid? IdCategoria { get; set; }
        /// <summary>
        /// Lista para exibir nas paginas as opções de categorias
        /// </summary>
        public List<SelectListItem>? Categorias { get; set; }
    }
}
