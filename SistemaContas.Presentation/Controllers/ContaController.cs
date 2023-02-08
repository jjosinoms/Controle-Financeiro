using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SistemaContas.Data.Entities;
using SistemaContas.Data.Enums;
using SistemaContas.Data.Repositories;
using SistemaContas.Presentation.Models;

namespace SistemaConta.Presentation.Controllers
{
    [Authorize]
    public class ContaController : Controller
    {
        public IActionResult Cadastro()
        {
            var model = new ContaCadastroViewModel();
            model.Categorias = ObterCategorias();

            return View(model);
        }

        [HttpPost]
        public IActionResult Cadastro(ContaCadastroViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var conta = new Conta();

                    conta.Id = Guid.NewGuid();
                    conta.Nome = model.Nome;
                    conta.Valor = model.Valor;
                    conta.Data = model.Data;
                    conta.Tipo = model.Tipo;
                    conta.Observacoes = model.Observacoes;
                    conta.IdCategoria = model.IdCategoria;
                    conta.IdUsuario = UsuarioAutenticado.Id;

                    var contaRepository = new ContaRepository();
                    contaRepository.Add(conta);

                    TempData["MensagemSucesso"] = "Conta cadastrada com sucesso";
                    model = new ContaCadastroViewModel();
                    ModelState.Clear();
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = "Falha ao cadastrar conta: " + e.Message;
                }
            }
            else
            {
                TempData["MensagemAlerta"] = "Ocorreram erros de validação no preenchimento do formulário.";
            }

            model.Categorias = ObterCategorias();
            return View(model);
        }

        public IActionResult Consulta()
        {
            var model = new ContaConsultaViewModel();
            var contaRepository = new ContaRepository();

            try
            {
                var dataAtual = DateTime.Now;
                var dataIni = new DateTime(dataAtual.Year, dataAtual.Month, 1);
                var dataFim = dataIni.AddMonths(1).AddDays(-1);

                model.DataIni = dataIni.ToString("yyyy-MM-dd");
                model.DataFim = dataFim.ToString("yyyy-MM-dd");

                ObterContas(model);

            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = "Falha ao consultar contas" + e.Message;
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Consulta(ContaConsultaViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ObterContas(model);
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = "Falha ao consultar contas: " + e.Message;
                }
            }

            return View(model);
        }

        public IActionResult Edicao()
        {
            return View();
        }

        /// <summary>
        /// Método para retornar uma lista de itens de seleção contendo categorias
        /// </summary>
        private List<SelectListItem> ObterCategorias()
        {
            //consultar as categorias do banco de dados do usuário autenticado
            var categoriaRepository = new CategoriaRepository();
            var categorias = categoriaRepository.GetByUsuario(UsuarioAutenticado.Id);

            var lista = new List<SelectListItem>();
            foreach (var item in categorias)
            {
                var selectListItem = new SelectListItem();
                selectListItem.Value = item.Id.ToString(); //valor que será capturado pelo campo
                selectListItem.Text = item.Nome; //texto que será exibido pelo campo

                lista.Add(selectListItem);
            }

            return lista;
        }

        /// <summary>
        /// Método para retornar os dados do usuário autenticado
        /// </summary>
        private IdentityViewModel UsuarioAutenticado
        {
            get
            {
                var data = User.Identity.Name;
                return JsonConvert.DeserializeObject<IdentityViewModel>(data);
            }
        }

        private void ObterContas(ContaConsultaViewModel model)
        {
            var contaRepository = new ContaRepository();

            var contas = contaRepository.GetByUsuarioAndDatas(UsuarioAutenticado.Id, DateTime.Parse(model.DataIni), DateTime.Parse(model.DataFim));

            model.Resultado = new List<ContaConsultaResultadoViewModel>();

            foreach (var item in contas)
            {
                var resultado = new ContaConsultaResultadoViewModel();
                resultado.Id = item.Id;
                resultado.Nome = item.Nome;
                resultado.Data = item.Data.ToString("ddd, dd/MM/yyyy");
                resultado.Valor = item.Valor;
                resultado.Tipo = item.Tipo.ToString();

                model.Resultado.Add(resultado);
            }
        }
    }
}



