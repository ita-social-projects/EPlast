using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EPlast.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult HandleError(int code)
        {
            ViewBag.Code = code;
            
            return View("Error");
        }
    }
}