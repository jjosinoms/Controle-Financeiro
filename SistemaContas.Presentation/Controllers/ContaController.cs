using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemaContas.Data.Repositories;
using SistemaContas.Presentation.Models;
using Newtonsoft.Json;

namespace SistemaContas.Presentation.Controllers
{
    [Authorize]
    public class ContaController : Controller
    {
        public IActionResult Consulta()
        {
            return View();
        }
        public IActionResult Cadastro()
        {
            return View();
        }

        public IActionResult Edicao()
        {
            return View();
        }
        public IActionResult Exclusao()
        {
            return View();
        }

        private IdentityViewModel UsuarioAutenticado
        {
            get
            {
                var data = User.Identity.Name;
                return JsonConvert.DeserializeObject<IdentityViewModel>(data);
            }
        }
    }
}
