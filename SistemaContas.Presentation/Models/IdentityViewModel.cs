namespace SistemaContas.Presentation.Models
{
    public class IdentityViewModel
    {
        /// <summary>
        /// Modelo de dados para as informações de autenticação
        /// de usuários que serão gravados em Cookie
        /// </summary>
        public Guid Id{ get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public DateTime? DataHoraAcesso{ get; set; }


    }
}
