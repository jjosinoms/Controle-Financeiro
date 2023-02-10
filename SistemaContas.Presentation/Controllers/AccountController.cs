using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SistemaContas.Data.Entities;
using SistemaContas.Data.Helpers;
using SistemaContas.Data.Repositories;
using SistemaContas.Presentation.Models;
using System.Security.Claims;
using System.Security.Cryptography;
using Bogus;
using SistemaContas.Messages.Services;


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
            if (ModelState.IsValid)
            {
                try
                {
                    // Verificar se o email informado ja esta cadastrado no banco de dados
                    var usuarioRepository = new UsuarioRepository();
                    var usuario = usuarioRepository.GetByEmailAndSenha(model.Email, MD5Helper.Encrypt(model.Senha));

                    if (usuario != null)
                    {
                       
                        #region Realizar a autenticacao do usuario

                        var identityViewModel = new IdentityViewModel();
                        identityViewModel.Id = usuario.Id;
                        identityViewModel.Nome = usuario.Nome;
                        identityViewModel.Email = usuario.Email;
                        identityViewModel.DataHoraAcesso = DateTime.Now;

                        TempData["MensagemSucesso"] = $"Seja bem vindo {identityViewModel.Nome}!";

                        //gravando o cookie de autenticacao
                        var claimsIdentity = new ClaimsIdentity(new[]
                        {
                            new Claim(ClaimTypes.Name, JsonConvert.SerializeObject(identityViewModel))
                        }, CookieAuthenticationDefaults.AuthenticationScheme);

                        var claimPrincipal = new ClaimsPrincipal(claimsIdentity);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal);

                        //Redirecionando o usuario para /Home/Index
                            
                        return RedirectToAction("Index", "Home");


                        #endregion


                    }
                    else
                    {
                        TempData["MensagemAlerta"] = "Acesso Negado! Usuário não encontrado!";
                        ModelState.Clear(); // limpar todos os campos do formulário
                    }

                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = $"Falha ao Logar o usuário: {e.Message}.";
                }

            }
            else
            {
                TempData["MensagemAlerta"] = "Ocorreram erros de validação no prreenchimento do formulário.";
            }
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
                catch (Exception e)
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
            if (ModelState.IsValid)
            {
                try
                {
                    var usuarioRepository = new UsuarioRepository();
                    var usuario = usuarioRepository.GetByEmail(model.Email);    

                    if(usuario != null)
                    {
                        // Faker é uma classe do Bogus
                        var novaSenha = new Faker().Internet.Password();
                        // enviar a senha para o email do usuario
                        var emailDest = usuario.Email;
                        var assunto = "Recuperação de Senha - Sistema Contas";
                        var mensagem = $@"
                                <h3>Olá, { usuario.Nome }</h3>
                                <p> Uma nova senha foi gerada com sucesso para o seu usuário. </p>
                                <p> Acesse o sistema com a senha: {novaSenha} </p>
                                <p> Após acessar o sistema, você pode alterar essa senha para uma nova de sua preferência. </p>
                                </br>
                                <p>Att, </br> Equipe Sistema Contas </p>
                            ";
                        //enviando o email para o usuário
                        EmailService.EnviarMensagem(emailDest, assunto, mensagem);
                        usuarioRepository.Update(usuario.Id, MD5Helper.Encrypt(novaSenha));

                        TempData["MensagemSucesso"] = "Recuperação de senha realizada com sucesso";
                        ModelState.Clear();

                    }
                    else
                    {
                        TempData["MensagemAlerta"] = "Usuário não encontrado! Verifique o E-mail informado.";
                    }

                }
                catch(Exception e)
                {
                    TempData["MensagemErro"] = "Falha ao recuperar senha: " + e.Message;
                }
            }


            return View(model);
        }

        //Account/Logout
        public IActionResult Logout()
        {
            //destruir o cookie de autenticacao (identificacao do usuario)
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            //redirecionar de volta para a pagina de Login
            return RedirectToAction("Login", "Account");
        }

    }
}
