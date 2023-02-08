using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SistemaContas.Data.Entities;
using SistemaContas.Data.Helpers;
using SistemaContas.Data.Repositories;
using SistemaContas.Presentation.Models;
using System.Diagnostics;
using System.Reflection;

namespace SistemaContas.Presentation.Controllers
{
    [Authorize]
    public class CategoriaController : Controller
    {
        public IActionResult Consulta()
        {
            var model = new List<CategoriaConsultaViewModel>();

            try
            {
                var categoriaRepository = new CategoriaRepository();
                foreach (var item in categoriaRepository.GetByUsuario(UsuarioAutenticado.Id))
                {
                    var categoriaConsultaViewModel = new CategoriaConsultaViewModel();
                    categoriaConsultaViewModel.Id = item.Id;
                    categoriaConsultaViewModel.Nome = item.Nome;

                    model.Add(categoriaConsultaViewModel);
                }
            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = "Falha ao consultar categorias: " + e.Message;
            }

            return View(model);
        }

        public IActionResult Cadastro()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Cadastro(CategoriaViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    // Verificar se o email informado ja esta cadastrado no banco de dados
                    var categoriaRepository = new CategoriaRepository();

                    //criar um objeto usuário
                    var categoria = new Categoria();


                    Debug.WriteLine(UsuarioAutenticado.Id);
                    Debug.WriteLine(UsuarioAutenticado.Nome);
                    //capturar os dados do usuário enviados pelo formulário
                    categoria.Id = Guid.NewGuid();
                    categoria.Nome = model.Nome;
                    categoria.IdUsuario = UsuarioAutenticado.Id;

                    //gravando o usuario no banco de dados
                    categoriaRepository.Add(categoria);

                    TempData["MensagemSucesso"] = "Parabéns! Sua Categoria foi cadastrada com sucesso!";
                    ModelState.Clear(); // limpar todos os campos do formulário

                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = $"Falha ao cadastrar a Categoria: {e.Message}.";
                }

            }
            else
            {
                TempData["MensagemAlerta"] = "Ocorreram erros de validação no preenchimento do formulário.";
            }
            return View();
        }

        public IActionResult Edicao(Guid id)
        {
            var model = new CategoriaEdicaoViewModel();

            try
            {
                var categoriaRepository = new CategoriaRepository();
                var categoria = categoriaRepository.GetById(id);
                if (categoria != null && categoria.IdUsuario == UsuarioAutenticado.Id)
                {
                    model.Id = categoria.Id;
                    model.Nome = categoria.Nome;

                }
                else
                {
                    TempData["MensagemAlerta"] = "Ocorreram erros de validação no preenchimento do formulário.";
                }

            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = "Falha ao obter categoria: " + e.Message;
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Edicao(CategoriaEdicaoViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var categoriaRepository = new CategoriaRepository();
                    var categoria = categoriaRepository.GetById(model.Id);
                    if (categoria != null && categoria.IdUsuario == UsuarioAutenticado.Id)
                    {
                        categoria.Nome = model.Nome;
                        categoriaRepository.Update(categoria);

                        TempData["MensagemSucesso"] = "Categoria alterada com sucesso!";
                        return RedirectToAction("Consulta");
                    }
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = "Falha ao atualizar categoria: " + e.Message;
                }
            }

            return View(model);
        }
        public IActionResult Exclusao(Guid id)
        {
            try
            {
                //capturar a categoria no banco de dados através do ID
                var categoriaRepository = new CategoriaRepository();
                var categoria = categoriaRepository.GetById(id);

                //verificando se a categoria foi encontrada e se
                //ela pertence ao usuário autenticado
                if (categoria != null && categoria.IdUsuario == UsuarioAutenticado.Id)
                {
                    //capturar a quantidade de contas da categoria selecionada
                    var quantidadeContas = categoriaRepository.QuantidadeContasByIdCategoria(id);
                    if( quantidadeContas == 0) // não ha contas vinculadas para a categoria
                    {
                        categoriaRepository.Delete(categoria);

                        TempData["MensagemSucesso"] = "Categoria excluída com sucesso.";
                        
                    }
                    else
                    {
                        TempData["MensagemAlerta"] = $"Não é possivél excluir essa categoria! Pois exitem '{quantidadeContas}' contas relacionadas a ela!";
                    }

                    //excluindo do banco de dados

                }
            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = "Falha ao excluir categoria: " + e.Message;
            }

            return RedirectToAction("Consulta");

        }

        /// <summary>
        /// Metodo para retornar os dados so usuario autenticado
        /// </summary>
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
