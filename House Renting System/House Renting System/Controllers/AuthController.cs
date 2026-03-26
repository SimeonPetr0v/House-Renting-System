using House_Renting_System.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace House_Renting_System.Controllers
{
    public class AuthController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if(ModelState.IsValid == false)
            {
           return View(model);
            }
            return View();
        }

    }
}
