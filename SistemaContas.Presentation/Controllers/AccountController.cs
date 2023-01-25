using Microsoft.AspNetCore.Mvc;

namespace SistemaContas.Presentation.Controllers
{
    public class AccountController : Controller
    {
        //Account/Login
        public IActionResult Login()
        {
            return View();
        }
        //Account/Register
        public IActionResult Register()
        {
            return View();
        }
        //Account/PasswordRecover
        public IActionResult PasswordRecover()
        {
            return View();
        }
    }
}
