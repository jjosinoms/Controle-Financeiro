namespace SistemaContas.Presentation.Models
{
    /// <summary>
    /// Modelo de dados para as informações exibidas na página de Dashboard
    /// </summary>
    public class DashboardViewModel
    {
        public string? DataIni { get; set; }
        public string? DataFim { get; set; }
        public decimal? TotalReceber  { get; set; }
        public decimal? TotalPagar { get; set; }
        public decimal? Saldo { get; set; }
        public string? Situacao { get; set; }
    }
}
