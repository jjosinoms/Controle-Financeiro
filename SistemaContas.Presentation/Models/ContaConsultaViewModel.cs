using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaContas.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace SistemaContas.Presentation.Models
{
    public class ContaConsultaViewModel
    {
        [Required(ErrorMessage = "Por favor, informe a data de Início.")]
        public string? DataIni { get; set; }
        [Required(ErrorMessage = "Por favor, informe a data de Término.")]
        public string? DataFim { get; set; }
        public List<ContaConsultaResultadoViewModel>? Resultado { get; set; }


    }
}
