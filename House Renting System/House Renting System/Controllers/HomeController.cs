using Microsoft.AspNetCore.Mvc;

namespace House_Renting_System.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
 
        public IActionResult Error()
        {
            return View("Error500");
        }

        public IActionResult StatusCodeHandler(int statusCode)
        {
            ViewBag.StatusCode = statusCode;

            if (statusCode == 401 || statusCode == 404)
            {
                return View("ErrorStatus");
            }

            
            return View("ErrorStatus");
        }
    }
}