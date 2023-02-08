using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemaContas.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace SistemaContas.Presentation.Models
{
    public class ContaConsultaResultadoViewModel
    {
        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public decimal? Valor { get; set; }
        public string? Data { get; set; }
        public string? Tipo { get; set; }
        public string? Categoria { get; set; }


    }
}
