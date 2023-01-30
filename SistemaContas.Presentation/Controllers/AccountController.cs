using Microsoft.AspNetCore.Mvc;
using SistemaContas.Data.Entities;
using SistemaContas.Data.Helpers;
using SistemaContas.Data.Repositories;
using SistemaContas.Presentation.Models;
using System.Security.Cryptography;

namespace SistemaContas.Presentation.Controllers
{
    public class AccountController : Controller
    {
        //Account/Login
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost] // Recebe o SUBMIT do formulário
        public IActionResult Login(LoginViewModel model)
        {
            return View();
        }
        //Account/Register
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost] // Recebe o SUBMIT do formulário
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Verificar se o email informado ja esta cadastrado no banco de dados
                    var usuarioRepository = new UsuarioRepository();

                    if (usuarioRepository.GetByEmail(model.Email) != null)
                    {
                        TempData["MensagemAlerta"] = "o email informado já está cadastrado no sistema, tente outro.";
                    }
                    else
                    {

                        //criar um objeto usuário
                        var usuario = new Usuario();

                        //capturar os dados do usuário enviados pelo formulário
                        usuario.Id = Guid.NewGuid();
                        usuario.Nome = model.Nome;
                        usuario.Email = model.Email;
                        usuario.Senha = MD5Helper.Encrypt(model.Senha);

                        //gravando o usuario no banco de dados
                        usuarioRepository.Add(usuario);

                        TempData["MensagemSucesso"] = "Parabéns! Sua conta foi cadastrada com sucesso!";
                        ModelState.Clear(); // limpar todos os campos do formulário
                    }

                }
                catch(Exception e)
                {
                    TempData["MensagemErro"] = $"Falha ao cadastrar o usuário: {e.Message}.";
                }
                
            }
            else
            {
                TempData["MensagemAlerta"] = "Ocorreram erros de validação no prreenchimento do formulário.";
            }
            return View();
        }
        //Account/PasswordRecover
        public IActionResult PasswordRecover()
        {
            return View();
        }
        [HttpPost] // Recebe o SUBMIT do formulário
        public IActionResult PasswordRecover(PasswordRecoverViewModel model)
        {
            return View();
        }
    }
}
