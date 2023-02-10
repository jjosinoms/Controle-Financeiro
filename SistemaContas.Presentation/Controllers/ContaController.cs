using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SistemaContas.Data.Entities;
using SistemaContas.Data.Enums;
using SistemaContas.Data.Repositories;
using SistemaContas.Presentation.Models;
using SistemaContas.Reports.Services;

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
                    conta.Tipo = model.Tipo == 1 ? TipoConta.Receber : TipoConta.Pagar; 
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

        public IActionResult Exclusao(Guid id)
        {
            try
            {
                var contaRepository = new ContaRepository();
                var conta = contaRepository.GetById(id);
                if (conta != null && conta.IdUsuario == UsuarioAutenticado.Id)
                {
                    //excluindo do banco de dados
                    contaRepository.Delete(conta);

                    TempData["MensagemSucesso"] = "Conta excluída com sucesso.";
                }

            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = "Falha ao excluir conta" + e.Message;
            }
            return RedirectToAction("Consulta");
        }

        public IActionResult Edicao(Guid id)
        {
            var model = new ContaEdicaoViewModel();

            try
            {
                var contaRepository = new ContaRepository();
                var conta = contaRepository.GetById(id);

                if (conta != null && conta.IdUsuario == UsuarioAutenticado.Id)
                {
                    model.Id = conta.Id;
                    model.Nome = conta.Nome;
                    model.Valor = conta.Valor;
                    model.Observacoes = conta.Observacoes;
                    model.IdCategoria = conta.IdCategoria;
                    model.Data = conta.Data.ToString("yyyy-MM-dd");
                    model.Tipo = conta.Tipo == TipoConta.Receber ? 1 : 2;
                    model.Categorias = ObterCategorias();

                }
                else
                {
                    TempData["MensagemAlerta"] = "Ocorreram erros de validação no preenchimento do formulário.";
                }

            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = "Falha ao obter conta: " + e.Message;
            }

            return View(model);
        }
        [HttpPost]
        public IActionResult Edicao(ContaEdicaoViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var contaRepository = new ContaRepository();
                    var conta = contaRepository.GetById(model.Id);

                    if (conta != null && conta.IdUsuario == UsuarioAutenticado.Id)
                    {
                        conta.Nome = model.Nome;
                        conta.Data = DateTime.Parse(model.Data);
                        conta.Valor = model.Valor.Value;
                        conta.Tipo = model.Tipo == 1 ? TipoConta.Receber : TipoConta.Pagar;
                        conta.IdCategoria = model.IdCategoria.Value;
                        conta.Observacoes = model.Observacoes;

                        contaRepository.Update(conta);

                        TempData["MensagemSucesso"] = "Conta atualizada com sucesso.";
                        return RedirectToAction("Consulta");
                    }
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = "Falha ao atualizar conta: " + e.Message;
                }
            }

            model.Categorias = ObterCategorias();
            return View(model);

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

        public IActionResult Relatorio()
        {
            try
            {
                //consultar as categorias do usuário autenticado
                var contaRepository = new ContaRepository();
                var contas = contaRepository.GetByUsuario(UsuarioAutenticado.Id);

                //gerar um relatorio excel com as categorias
                var contasReportService = new ContasReportService();
                var relatorio = contasReportService.GerarRelatorio(contas);

                //Download do relatorio

                return File(relatorio, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "relatorio_contas.xlsx");

            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = "Falha ao gerar relatório" + e.Message;
            }

            return RedirectToAction("Consulta");
        }
    }
}



