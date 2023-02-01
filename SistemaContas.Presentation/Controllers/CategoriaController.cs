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
            return View();
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
                    var data = User.Identity.Name;
                    var identityViewModel = JsonConvert.DeserializeObject<IdentityViewModel>(data);

                    // Verificar se o email informado ja esta cadastrado no banco de dados
                    var categoriaRepository = new CategoriaRepository();

                    //criar um objeto usuário
                    var categoria = new Categoria();


                    Debug.WriteLine(identityViewModel.Id);
                    Debug.WriteLine(identityViewModel.Nome);
                    //capturar os dados do usuário enviados pelo formulário
                    categoria.Id = Guid.NewGuid();
                    categoria.Nome = model.Nome;
                    categoria.IdUsuario = identityViewModel.Id;

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

        public IActionResult Edicao()
        {
            return View();
        }

        public IActionResult Delete()
        {
            return View();
        }

    }
}
